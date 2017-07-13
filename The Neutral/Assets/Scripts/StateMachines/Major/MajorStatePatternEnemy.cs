using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Neutral
{
    public class MajorStatePatternEnemy : MonoBehaviour, IStatePatternEnemy
    {

        //in alert mode, this is the variable which controls the speed at which it turns around to look for the player
        public float searchingTurnSpeed = 120f;
        //how long to search for the player in alert mode
        public float searchingDuration = 4f;
        //how far the raycast goes to find the player and start chasing
        public float sightRange = 30f;
        //how far the raycast goes to initiate combat moves
        public float combatRange = 20f;
        //stores waypoints
        public IList<Transform> waypoints;
        //empty gameobject at eye-level for the enemy so that we cast from a sensable place where we are looking from
        public Transform eyes;
        //this is for lifting the "look" so they are not looking at the players feet, but at the head/body area
        public Vector3 offset = new Vector3(0, 0.5f, 0);
        //cube above enemy head (shows state?)
        public MeshRenderer meshRendererFlag;

        public GameObject sphere;

        [HideInInspector]
        //reference to players transform
        public Transform target;

        [HideInInspector]
        public IEnemyState currentState;

        [HideInInspector]
        public MajorCombatState combatState;

        [HideInInspector]
        public UnityEngine.AI.NavMeshAgent navMeshAgent;

        [HideInInspector]
        public Animator anim;

        [HideInInspector]
        public AnimationUtilities animHelper;

        private bool stop;

        // Use this for initialization
        void Start()
        {

            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            anim = GetComponent<Animator>();
            animHelper = new AnimationUtilities();

            stop = false;

            waypoints = new List<Transform>();

            sphere = GetComponentInChildren<SphereCollider>().gameObject;

            combatState = new MajorCombatState(this);

            currentState = combatState;
            Debug.Log("SPHERE LOCALSCALE: " + sphere.gameObject.transform.localScale);
        }

        // Update is called once per frame
        void Update()
        {
            if (!stop) currentState.UpdateState();

        }

        //a callback that triggers when the player(anything) enters the trigger we set up
        private void OnTriggerEnter(Collider other)
        {
            currentState.OnTriggerEnter(other);
        }

        public void stopAI()
        {
            stop = true;
        }

        public void resumeAI()
        {
            stop = false;
        }

        public void setWaypoints(IList<Transform> waypoints)
        {
            waypoints.Shuffle();
            this.waypoints = waypoints;
            
        }

        public void destroyGameObject()
        {
            Destroy(this.gameObject);
        }
      
        /*
         * Major initial sphere expansion should take up zone radius
         * Major disappears
         * Zone restarts (minor spawns)
         */
    }

}