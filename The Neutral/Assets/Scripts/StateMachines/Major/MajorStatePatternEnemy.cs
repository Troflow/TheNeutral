using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class MajorStatePatternEnemy : MonoBehaviour
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
        public Transform[] wayPoints;
        //empty gameobject at eye-level for the enemy so that we cast from a sensable place where we are looking from
        public Transform eyes;
        //this is for lifting the "look" so they are not looking at the players feet, but at the head/body area
        public Vector3 offset = new Vector3(0, 0.5f, 0);
        //cube above enemy head (shows state?)
        public MeshRenderer meshRendererFlag;

        [HideInInspector]
        //reference to players transform
        public Transform target;

        [HideInInspector]
        public IEnemyState currentState;

        [HideInInspector]
        public MajorCombatState combatState;

        [HideInInspector]
        public MajorChaseState chaseState;

        [HideInInspector]
        public MajorAlertState alertState;

        [HideInInspector]
        public MajorPatrolState patrolState;

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
            chaseState = new MajorChaseState(this);
            alertState = new MajorAlertState(this);
            patrolState = new MajorPatrolState(this);
            combatState = new MajorCombatState(this);
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            anim = GetComponent<Animator>();
            animHelper = new AnimationUtilities();
            currentState = patrolState;
            stop = false;

            for (int x=0; x<10; x++)
            {
                GameObject waypoint = GameObject.Find("MinorWaypoint" + (x + 1));
                if (waypoint == null)
                {
                    break;
                }
                wayPoints[x] = GameObject.Find("MinorWaypoint" + (x + 1)).transform;
            }
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
    }

}