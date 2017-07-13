using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class MajorCombatState : IEnemyState
    {
        //each state class needs this
        private readonly MajorStatePatternEnemy enemy;
        private float spawnTimer;
        private bool isAttacking;
        public MajorCombatState(MajorStatePatternEnemy statePatternEnemy)
        {
            enemy = statePatternEnemy;
            isAttacking = false;
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
            spawnTimer = 0f;
        }

        public void ToCombatState()
        {

        }

        private void Attack()
        {

            AnimatorStateInfo stateInfo = enemy.anim.GetCurrentAnimatorStateInfo(0);

            if (isAttacking)
            {
                if (enemy.sphere.gameObject.transform.localScale.x <= 15f)
                {
                    enemy.stopAI();
                    GameObject.Find(enemy.GetComponent<MajorNavMeshController>().getSpawnZone()).GetComponent<IZone>().spawn(EnemyType.Minor, Quaternion.identity);
                    enemy.destroyGameObject();
                }
                return;
            }

            isAttacking = true;


            if (stateInfo.IsName("Spawn"))
            {
                isAttacking = false;
                return;
            }

            enemy.meshRendererFlag.material.color = Color.black;
            enemy.anim.SetTrigger("isAttack");


            enemy.StartCoroutine(CombatHelper.SphereExpandAndRetractFromCurrent(enemy.sphere, 3f, 200));

            return;
        }
    }

}
