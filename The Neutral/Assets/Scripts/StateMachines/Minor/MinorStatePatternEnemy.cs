using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Neutral
{
    public class MinorStatePatternEnemy : MonoBehaviour, IStatePatternEnemy
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
        [HideInInspector]
        public IList<Transform> waypoints;
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
        public MinorCombatState combatState;

        [HideInInspector]
        public MinorChaseState chaseState;

        [HideInInspector]
        public MinorAlertState alertState;

        [HideInInspector]
        public MinorPatrolState patrolState;

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
            chaseState = new MinorChaseState(this);
            alertState = new MinorAlertState(this);
            patrolState = new MinorPatrolState(this);
            combatState = new MinorCombatState(this);

            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            anim = GetComponent<Animator>();

            animHelper = new AnimationUtilities();
            currentState = patrolState;

            stop = false;

            waypoints = new List<Transform>();
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
    }

}