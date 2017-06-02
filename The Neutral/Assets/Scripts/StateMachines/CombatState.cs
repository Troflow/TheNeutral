using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class CombatState : IEnemyState
    {
        //each state class needs this
        private readonly StatePatternEnemy enemy;

        public CombatState(StatePatternEnemy statePatternEnemy)
        {
            enemy = statePatternEnemy;
        }

        public void UpdateState()
        {
            Attack();
        }

        public void OnTriggerEnter(Collider other)
        {

        }

        public void ToPatrolState()
        {

        }

        public void ToAlertState()
        {
            
        }

        public void ToChaseState()
        {
            enemy.currentState = enemy.chaseState;
        }

        public void ToCombatState()
        {

        }

        private void Attack()
        {
            enemy.meshRendererFlag.material.color = Color.black;
            AnimatorStateInfo stateInfo = enemy.anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Merge") || enemy.anim.IsInTransition(0))
            {
                if (enemy.animHelper.AnimationFinished(stateInfo, stateInfo.length - 2.5f))
                {
                    enemy.anim.SetBool(Animator.StringToHash("isMerge"), false);
                    ToChaseState();
                }
            }
            else
            {
                enemy.navMeshAgent.Stop();
                enemy.anim.SetBool(Animator.StringToHash("isMerge"), true);
                //enemy.anim.Play("Merge");
            }

        }
    }

}
