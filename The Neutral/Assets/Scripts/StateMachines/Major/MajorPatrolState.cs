using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class MajorPatrolState : IEnemyState
    {
        //each state class needs this
        private readonly MajorStatePatternEnemy enemy;

        //count through the waypoints and this stores the next one we want to move towards
        private int nextWayPoint;

        public MajorPatrolState(MajorStatePatternEnemy statePatternEnemy)
        {
            enemy = statePatternEnemy;
        }

        public void UpdateState()
        {
            Look();
            Patrol();
        }

        public void OnTriggerEnter(Collider other)
        {
            //if the object tags with player collides with the trigger, go to the alert state
            if (other.gameObject.CompareTag("Player"))
            {
                ToAlertState();
            }
        }

        public void ToPatrolState()
        {
            Debug.Log("CAN NOT TRANSITION TO SAME STATE");
        }

        public void ToAlertState()
        {
            enemy.currentState = enemy.alertState;
        }

        public void ToChaseState()
        {
            enemy.currentState = enemy.chaseState;
        }

        public void ToCombatState()
        {
         
        }

        private void Look ()
        {
            RaycastHit hit;
            //check if hit within the sight range AND if the thing hit was the player
            if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player-Sphere"))
            {
                enemy.target = hit.transform;
                ToChaseState();
            }
        }

        private void Patrol ()
        {
            enemy.meshRendererFlag.material.color = Color.green;
            //destination is the next waypoint in the array
            enemy.navMeshAgent.destination = enemy.wayPoints[nextWayPoint].position;

            AnimatorStateInfo stateInfo = enemy.anim.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("Respawn")) {
                enemy.navMeshAgent.isStopped = false;
            }
            else {
                enemy.navMeshAgent.isStopped = true;
            }
            
            //is the remaining distance less than the stopping distance AND do we no longer have more paths we are trying to patrol
            if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending)
            {
                //need modulo to loop back (after hits last one in array, goes back to 0)
                nextWayPoint = (nextWayPoint + 1) % enemy.wayPoints.Length ;
            }
        }

    }

}
