using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class MajorCombatState : IEnemyState
    {
        //each state class needs this
        private readonly MajorStatePatternEnemy enemy;
        private float spawnTimer;

        public MajorCombatState(MajorStatePatternEnemy statePatternEnemy)
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
            spawnTimer = 0f;
        }

        public void ToCombatState()
        {

        }

        private void Attack()
        {
            enemy.meshRendererFlag.material.color = Color.black;
            //spawnTimer += Time.deltaTime;

            //if (spawnTimer > 10)
            //{
            //    IZone zone = GameObject.Find(enemy.gameObject.GetComponent<MinorNavMeshController>().getSpawnZone()).GetComponent<IZone>();
            //    zone.spawn(enemy.transform.rotation);
            //    spawnTimer = 0f;
            //}
            //AnimatorStateInfo stateInfo = enemy.anim.GetCurrentAnimatorStateInfo(0);
            //if (stateInfo.IsName("Merge") || enemy.anim.IsInTransition(0))
            //{
            //    if (enemy.animHelper.AnimationFinished(stateInfo, stateInfo.length - 2.5f))
            //    {
            //        enemy.anim.SetBool(Animator.StringToHash("isMerge"), false);
            //        ToChaseState();
            //    }
            //}
            //else
            //{
            //    enemy.navMeshAgent.Stop();
            //    enemy.anim.SetBool(Animator.StringToHash("isMerge"), true);
            //    //enemy.anim.Play("Merge");
            //}

        }
    }

}
