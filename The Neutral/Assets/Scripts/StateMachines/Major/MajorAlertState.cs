using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class MajorAlertState : IEnemyState
    {
        //each state class needs this
        private readonly MajorStatePatternEnemy enemy;
        private float searchTimer;

        public MajorAlertState(MajorStatePatternEnemy statePatternEnemy)
        {
            enemy = statePatternEnemy;
        }

        public void UpdateState()
        {
            Look();
            Search();
        }

        public void OnTriggerEnter(Collider other)
        {

        }

        public void ToPatrolState()
        {
            enemy.currentState = enemy.patrolState;
            searchTimer = 0f;
        }

        public void ToAlertState()
        {
            Debug.Log("CAN NOT TRANSITION TO SAME STATE");
            
        }

        public void ToChaseState()
        {
            enemy.currentState = enemy.chaseState;
            searchTimer = 0f;
        }

        public void ToCombatState()
        {

        }

        private void Look()
        {
            RaycastHit hit;
            //check if hit within the sight range AND if the thing hit was the player
            if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
            {
                enemy.target = hit.transform;
                ToChaseState();
            }
        }

        private void Search()
        {
            enemy.meshRendererFlag.material.color = Color.yellow;
            enemy.navMeshAgent.isStopped = true;
            //rotate us around the Y Axis
            enemy.transform.Rotate(0, enemy.searchingTurnSpeed * Time.deltaTime, 0);
            //keep adding deltatime to our search timer, then we check if it goes past
            searchTimer += Time.deltaTime;



            if (searchTimer >= enemy.searchingDuration)
            {
                ToPatrolState();
            }
        }
    }

}
