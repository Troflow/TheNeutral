using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
    public class SphereCollision : MonoBehaviour
    {

        private Color[] colorTransitions;
        private IList<GameObject> aggregatedCollisions;

        private float duration = 1.5f;
        private int currIndex = 0;

        Renderer sphereRenderer;

        private float currentLerpProgress = 0;

        private bool isZoneCheck;

        // Use this for initialization
        void Start()
        {
            colorTransitions = new Color[4];
            colorTransitions[0] = Color.black;
            colorTransitions[1] = Color.gray;
            colorTransitions[2] = Color.blue;
            colorTransitions[3] = Color.cyan;

            sphereRenderer = GetComponent<Renderer>();

            aggregatedCollisions = new List<GameObject>();
            isZoneCheck = false;

        }

        // Update is called once per frame
        void Update()
        {

        }

        public IEnumerator SphereColorLerp(Renderer other, Color colorStart, Color colorEnd)
        {
            while (currentLerpProgress < 1)
            {
                //float lerp = Mathf.PingPong(Time.time, duration) / duration;
                other.material.SetColor("_EmissionColor", Color.Lerp(colorStart, colorEnd, currentLerpProgress));
                currentLerpProgress += Time.deltaTime / duration;
                yield return new WaitForSeconds(Time.deltaTime);
            }

        }

        public void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Player-Sphere" || col.gameObject.tag == "Enemy-Sphere")
            {
                Renderer sphereToLerp;
                //determine their current level of sphere expansion
                if (col.gameObject.transform.localScale.x > transform.localScale.x)
                {
                    sphereToLerp = sphereRenderer;
                }
                else
                {
                    sphereToLerp = col.gameObject.GetComponent<Renderer>();
                }

                currentLerpProgress = 0;

                StartCoroutine(SphereColorLerp(sphereToLerp, colorTransitions[currIndex % 4], colorTransitions[(currIndex + 1) % 4]));
                currIndex += 1;
                //otherSphere.material.color.a = alphaColorInit.GetValueOrDefault();
            }

            if (col.gameObject.CompareTag("Minor-Sphere"))
            {
                MinorNavMeshController minorController = col.GetComponentInParent<MinorNavMeshController>();
                if (!minorController.canMinorCollide())
                {
                    return;
                }
                minorController.setMinorCollision(false);
                GameObject minorHit = minorController.gameObject;
                if (!aggregatedCollisions.Contains(minorHit))
                {
                    aggregatedCollisions.Add(minorHit);
                    col.GetComponentInParent<Animator>().SetBool("isDead", true);
                    col.GetComponentInParent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
                }
                if (!isZoneCheck)
                {
                    Debug.Log("DOING ZONE CHECK");
                    StartCoroutine(MinorCollision(GetComponentInParent<CombatController>()));
                }
            }
        }

        private IEnumerator MinorCollision(CombatController combatController)
        {
            isZoneCheck = true;
            Animator remyAnimator = GetComponentInParent<Animator>();
            AnimatorStateInfo stateInfo = remyAnimator.GetCurrentAnimatorStateInfo(0);
            while (combatController.isCombatAnimationPlaying(stateInfo))
            {
                stateInfo = remyAnimator.GetCurrentAnimatorStateInfo(0);
                yield return new WaitForEndOfFrame();
            }

            //can't be in more than one zone at a time so can assume all in same zone within aggregated collisions
            IZone zone = GameObject.Find(aggregatedCollisions[0].GetComponent<MinorNavMeshController>().getSpawnZone()).GetComponent<IZone>();

            foreach (GameObject minor in aggregatedCollisions)
            {
                zone.delete(minor);
                Destroy(minor);
            }

            int spawnCount = Random.Range(4, 6);

            if (aggregatedCollisions.Count == 1 && zone.entitiesInZone() == 0)
            {
                spawnCount = 2;
                for (int x = 0; x < spawnCount; x++)
                {
                    zone.spawn(EnemyType.Minor, Quaternion.identity);
                }
            }

            else if (zone.entitiesInZone()+spawnCount >= 7)
            {
                Debug.Log("SPAWNING MAJOR");
                zone.spawn(EnemyType.Major, Quaternion.identity);
                
            }

            else if (aggregatedCollisions.Count %2 != 0)
            {
                Debug.Log("SPAWNING NEW MINOR");
                
                for (int x=0; x<spawnCount; x++)
                {
                    zone.spawn(EnemyType.Minor, Quaternion.identity);
                }
            }
            
            isZoneCheck = false;
            aggregatedCollisions.Clear();           
        }
    }
}

