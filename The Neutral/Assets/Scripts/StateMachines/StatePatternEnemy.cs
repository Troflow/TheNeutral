using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class StatePatternEnemy : MonoBehaviour
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
        public CombatState combatState;

        [HideInInspector]
        public ChaseState chaseState;

        [HideInInspector]
        public AlertState alertState;

        [HideInInspector]
        public PatrolState patrolState;

        [HideInInspector]
        public NavMeshAgent navMeshAgent;

        [HideInInspector]
        public Animator anim;

        [HideInInspector]
        public AnimationUtilities animHelper;

        private void Awake()
        {
            chaseState = new ChaseState(this);
            alertState = new AlertState(this);
            patrolState = new PatrolState(this);
            combatState = new CombatState(this);
            navMeshAgent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            animHelper = new AnimationUtilities();
        }

        // Use this for initialization
        void Start()
        {
            currentState = patrolState;
        }

        // Update is called once per frame
        void Update()
        {
            currentState.UpdateState();
        }

        //a callback that triggers when the player(anything) enters the trigger we set up
        private void OnTriggerEnter(Collider other)
        {
            currentState.OnTriggerEnter(other);
        }
    }

}