using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
    public class SphereCollision : MonoBehaviour
    {
        // holds the current objects that were collided with the sphere within a single move
        private IList<GameObject> aggregatedCollisions;

        // some standard objects we need for interaction with rest of game
        Renderer sphereRenderer;
        CombatUtilities combatUtilities;
        Animator remyAnimator;
        MinorNavMeshController minorController;

        // coroutine booleans
        private bool isZoneCheck;
        private bool isKnockingBack;

        // Use this for initialization
        void Start()
        {

            sphereRenderer = GetComponent<Renderer>();

            combatUtilities = GetComponentInParent<CombatUtilities>();
            remyAnimator = GetComponentInParent<Animator>();
            minorController = GetComponent<MinorNavMeshController>();


            aggregatedCollisions = new List<GameObject>();
            isZoneCheck = false;
            isKnockingBack = false;

        }

        // Update is called once per frame
        void Update()
        {

        }


        private IEnumerator KnockBackEntity(Vector3 direction, float knockbackNormalization)
        {
            // knockbacks are not allowed until this coroutine finishes
            isKnockingBack = true;

            var startTime = Time.time;

            // hardcoded value to be replaced by length of knockback animation
            // currently, since the dash time = 0.7s and it feels clunky to be knocked back for 0.7s, I have put the time at 0.35
            // and doubled the distance moved. This means it's twice as fast but for half as long = the same distance as a dash
            while (Time.time - startTime < 0.35)
            {

                Player.agent.Move(2* direction.normalized * Time.deltaTime * Mathf.Abs(PlayerState.getDashSpeed() + knockbackNormalization ));
                yield return new WaitForEndOfFrame();
            }

            // only allow another knockback once the is fully retracted
            while (Mathf.RoundToInt(combatUtilities.getCurrentSphereSize()) != Mathf.RoundToInt(combatUtilities.getDefaultSphereSize()))
            {
                yield return new WaitForEndOfFrame();
            }

            // allow another knockback to take place now
            isKnockingBack = false;
        }

        //KNOCKBACK LOGIC:
        /*
         * If attacker 1 uses tier on enemy where edges just collide then
         * move back dash distance (distance where attacker can reach knocked-back enemy within one dash)
         * Otherwise:
         * move back dash distance + (difference of the center positions of the spheres at the current location)
        */
        private void computeKnockback(Collider col, float remySphereSize)
        {

            // no need to compute anything if we already being knocked back
            if (isKnockingBack) return;

            // get the two spheres that collided and compute distances and directions we need for knockback
            var playerSphere = this.transform.GetComponent<SphereCollider>();
            var opponentSphere = col.GetComponent<SphereCollider>();
            var centerSphereDistance = (playerSphere.transform.position - opponentSphere.transform.position).magnitude;
            var direction = (playerSphere.transform.position - opponentSphere.transform.position) / centerSphereDistance;

            // will give us our extra knockback value
            var knockbackNormalization = 0;

            // if the current sphere size at the time of the collision was roughly the maximum, it means it hit on the edge
            if (Mathf.RoundToInt(remySphereSize) >= Mathf.RoundToInt(combatUtilities.getMaxSphereSize()))
            {
                print("sphere collided at edge, no extra knockback distance");
            }

            // otherwise we add some extra knockback distance based off of how much our sphere actually expanded
            else
            {
                knockbackNormalization = Mathf.RoundToInt(combatUtilities.getMaxSphereSize()) - Mathf.RoundToInt(remySphereSize);
                print("applying an extra " + knockbackNormalization + " units of knockback");
            }

            // start our knockback action
            StartCoroutine(KnockBackEntity(direction, knockbackNormalization));

        }

        public void OnTriggerEnter(Collider col)
        {

            // collision occured on a generic enemy that has a sphere
            if (col.gameObject.tag == "Enemy-Sphere")
            {
                float remySphereSizeOnCollision = combatUtilities.getCurrentSphereSize();

                // apply color arithmetic by adding color
                col.GetComponentInParent<EnemyCombatColorController>().addColorToColorBook(GetComponentInParent<PlayerState>().getCurrentCombatColor());

                // start computation of knockback
                computeKnockback(col, remySphereSizeOnCollision);

            }

            // collision occured on a minor
            if (col.gameObject.CompareTag("Minor-Sphere"))
            {
                // if minors can not collide, it means they are already in the combination phase so no need for computing logic
                if (!minorController.canMinorCollide()) return;

                // make sure they can not collide as it will cause problems during combining while it hits remy/other objects
                minorController.setMinorCollision(false);

                // we add all the unique minors we've currently collided with during the current sphere expansion to a list
                GameObject minorHit = minorController.gameObject;
                if (!aggregatedCollisions.Contains(minorHit))
                {

                    aggregatedCollisions.Add(minorHit);

                    // start the death animation and stop pathing
                    remyAnimator.SetBool("isDead", true);
                    col.GetComponentInParent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
                }

                // if we are not currently checking the zone for collisions, then start the check
                if (!isZoneCheck)
                {
                    StartCoroutine(MinorCollision());
                }
            }

        }

        private IEnumerator MinorCollision()
        {
            isZoneCheck = true;

            // while remy's animation is currently going, it means the collisions have not finished aggregating yet
            // so we wait until we are not in a combat state before we compute the total amount of minors hit
            AnimatorStateInfo stateInfo = remyAnimator.GetCurrentAnimatorStateInfo(0);
            while (combatUtilities.isCombatAnimationPlaying(stateInfo))
            {
                stateInfo = remyAnimator.GetCurrentAnimatorStateInfo(0);
                yield return new WaitForEndOfFrame();
            }

            // need to find the zone we are in so we can apply logic to that specific one
            // can't be in more than one zone at a time so can assume all in same zone within aggregated collisions
            IZone zone = GameObject.Find(aggregatedCollisions[0].GetComponent<MinorNavMeshController>().getSpawnZone()).GetComponent<IZone>();

            // remove the minor from the zone and destroy the gameobject
            foreach (GameObject minor in aggregatedCollisions)
            {
                zone.delete(minor);
                Destroy(minor);
            }

            // randomly choose between 4 and 5 minors to spawn
            int spawnCount = Random.Range(4, 6);

            // if we only hit one minor and there are no more entities in the zone
            // we do special logic to spawn exactly two minors in the zone
            if (aggregatedCollisions.Count == 1 && zone.entitiesInZone() == 0)
            {
                spawnCount = 2;
                for (int x = 0; x < spawnCount; x++)
                {
                    zone.spawn(EnemyType.Minor, Quaternion.identity);
                }
            }

            // otherwise, if we hit an odd number of minors
            else if (aggregatedCollisions.Count % 2 != 0)
            {
                // if the total amount of minors in the zone + the number that would have spawned is more than 7
                // then we spawn a major
                if (zone.entitiesInZone() + spawnCount >= 7)
                {
                    zone.spawn(EnemyType.Major, Quaternion.identity);
                }

                // otherwise we give the player another chance and just spawn the allotted number of minors
                else
                {

                    for (int x = 0; x < spawnCount; x++)
                    {
                        zone.spawn(EnemyType.Minor, Quaternion.identity);
                    }
                }

            }

            isZoneCheck = false;
            aggregatedCollisions.Clear();
        }
    }
}

