using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class MinorChaseState : IEnemyState
    {
        //each state class needs this
        private readonly MinorStatePatternEnemy enemy;

        public MinorChaseState(MinorStatePatternEnemy statePatternEnemy)
        {
            enemy = statePatternEnemy;
        }

        public void UpdateState()
        {
            Look();
            Chase();
        }

        public void OnTriggerEnter(Collider other)
        {

        }

        public void ToPatrolState()
        {

        }

        public void ToAlertState()
        {
            enemy.currentState = enemy.alertState;
        }

        public void ToChaseState()
        {
            
        }

        public void ToCombatState()
        {
            enemy.currentState = enemy.combatState;
        }

        private void Look()
        {
            RaycastHit hit;
            //direction from the eyes to the target
            Vector3 enemyToTarget = (enemy.target.position + enemy.offset) - enemy.eyes.transform.position;
            
            //check if hit within the combat range and if was the player
            if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.combatRange) && hit.collider.CompareTag("Player-Sphere"))
            {
                enemy.target = hit.transform;
                ToCombatState();
            }
            else
            {
                ToChaseState();
            }

            //check if hit within the sight range AND if the thing hit was the player
            if (Physics.Raycast(enemy.eyes.transform.position, enemyToTarget, out hit, enemy.sightRange) && hit.collider.CompareTag("Player-Sphere"))
            {
                enemy.target = hit.transform;
                ToChaseState();
            }
            else
            {
                ToAlertState();
            }
        }

        private void Chase()
        {
            enemy.meshRendererFlag.material.color = Color.red;
            enemy.navMeshAgent.destination = enemy.target.position;
            enemy.navMeshAgent.isStopped = false;
        }
    }

}
