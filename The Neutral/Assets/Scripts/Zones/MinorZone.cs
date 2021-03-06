﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Neutral {

    public class MinorZone : MonoBehaviour, IZone
    {

        // prefab of the minors for this zone
        [HideInInspector]
        private GameObject minor_spawn;

        // prefab of the major for this zone
        [HideInInspector]
        private GameObject major_spawn;

        // list of waypoints we set to to the AI's in the zone
        [HideInInspector]
        public List<Transform> waypoints;

        // determines if we use user generated waypoints or randomized ones in the zone
        public bool randomizeWaypoints;

        // number of waypoints to generate if we use random ones
        [HideInInspector]
        public int randomizedWaypointCount;

        // this list contains all the minor gameobjects in the zone
        private IList<GameObject> group;

        // this list contains a record of all unique minors that were once and currently in the zone
        private IList<int> allInstanceIDs;

        // lets us know if we are in the transition phase of the zone
        private bool minorTransitionInProgress;

        // Use this for initialization
        void Start()
        {
            waypoints = new List<Transform>();

            // if we need to randomize waypoints
            if (randomizeWaypoints)
            {
                if (randomizedWaypointCount <= 0)
                {
                    throw new System.Exception("Can not have 0 waypoints");
                }
                
                for (int x=0; x< randomizedWaypointCount; x++)
                {
                    // instantiate the waypoints with this zone as the parent and sets the position relative to the parent
                    GameObject waypoint = Instantiate(new GameObject("MinorWaypoint" + (x + 1)), this.transform, false);
                    // set the position of the waypoint a random point within the current zones sphere
                    waypoint.transform.position = (Random.insideUnitSphere * GetComponent<SphereCollider>().radius) + this.transform.position;
                    waypoints.Add(waypoint.transform);
                }

            }
            
            // if we have waypoints placed in the zone already
            else
            {
                for (int x = 0; x < 999; x++)
                {
                    Transform waypoint = transform.FindChild("MinorWaypoint" + (x + 1));
                    if (waypoint == null)
                    {
                        break;
                    }
                    waypoints.Add(waypoint.transform);
                }
            }


            group = new List<GameObject>();
            allInstanceIDs = new List<int>();

            minor_spawn = Resources.Load("Prefabs/MinorSplitPrefab") as GameObject;
            major_spawn = Resources.Load("Prefabs/MajorPrefab") as GameObject;

            minorTransitionInProgress = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void add(GameObject minor)
        {
            // we must check if the minor being added is a unique minor
            // this is to circumvent the bug when removing a minor,
            // the auto detecting collision instantly adds it back before the gameobject
            // can actually be deleted causing incorrect behaviour.
            if (allInstanceIDs.Contains(minor.GetInstanceID()))
            {
                return;
            }

            // add new minor id to list of ids
            allInstanceIDs.Add(minor.GetInstanceID());
            // set waypoints for AI script
            minor.GetComponent<IStatePatternEnemy>().setWaypoints(waypoints);
            // add minor to zone
            group.Add(minor);

        }

        public void delete(GameObject minor)
        {
            group.Remove(minor);
        }

        public bool contains(GameObject minor)
        {
            foreach (GameObject gameObj in group)
            {
                if (gameObj.GetInstanceID() == minor.GetInstanceID())
                {
                    return true;
                }
            } 
            return false;
        }

        public void setEntityZone(GameObject minor)
        {
            minor.GetComponent<MinorNavMeshController>().setSpawnZone(this.gameObject.name);
        }

        public int entitiesInZone()
        {
            return group.Count;
        }

        public void spawn(EnemyType enemyType, Quaternion rotation)
        {
            switch (enemyType)
            {
                case EnemyType.Minor:
                    float scaleFactor = GetComponentInChildren<SphereCollider>().radius / 2;
                    Instantiate(minor_spawn, new Vector3(
                        minor_spawn.transform.position.x + Random.Range(-scaleFactor, scaleFactor),
                        minor_spawn.transform.position.y,
                        minor_spawn.transform.position.z + Random.Range(-scaleFactor, scaleFactor)),
                        rotation).SetActive(true);
                    break;


                case EnemyType.Major:
                    Vector3 majorSpawnPoint = Vector3.zero;
                    
                    // calculate the major spawn position based off of the
                    // averages of the minor positions
                    for (int x = 0; x < group.Count; x++)
                    {
                        group[x].GetComponentInParent<MinorNavMeshController>().setMinorCollision(false);
                        majorSpawnPoint.x += group[x].transform.position.x;
                        majorSpawnPoint.y = group[x].transform.position.y;
                        majorSpawnPoint.z += group[x].transform.position.z;
                    }
                    majorSpawnPoint.x = majorSpawnPoint.x / group.Count;
                    majorSpawnPoint.z = majorSpawnPoint.z / group.Count;

                    //makes sure to only start one move routine
                    if (!minorTransitionInProgress)
                    {
                        StartCoroutine("MinorToMajorTansformation", majorSpawnPoint);
                    }
                    break;


                default:
                    break;
            }
        }

        private void setMajorSettings(GameObject major)
        {
            major.SetActive(true);
            major.GetComponent<Animator>().SetBool("isSpawn", true);
            major.GetComponent<IStatePatternEnemy>().setWaypoints(waypoints);
        }

        private IEnumerator MinorToMajorTansformation(Vector3 location)
        {
            minorTransitionInProgress = true;

            for (int x=0; x<group.Count; x++)
            {
                // get the distance of each minor to the major spawn location
                var heading = location - group[x].transform.position;
                var distance = heading.magnitude;
                var minor = group[x];
                minor.transform.SetPositionAndRotation(minor.transform.position, Quaternion.identity);
                
                // move each individual minor based off of their distance to the major location
                StartCoroutine(MoveIndividualMinor(group[x], location, distance));
            }

            // the transformation stops based off of a fixed time which has to do with the animation time
            float startTime = Time.time;
            while (Time.time - startTime < 3.66667f)
            {
                yield return new WaitForEndOfFrame();
            }

            // stop all current coroutines aside from this one
            // this will stop the (s)lerping for the minors just
            // before we spawn the major
            StopAllCoroutines(); 

            foreach (GameObject minorInZone in group)
            {
                Destroy(minorInZone);
            }
            group.Clear();

            // instantiate the major and set the default settings
            GameObject major_final = Instantiate(major_spawn, location, major_spawn.transform.rotation);
            setMajorSettings(major_final);
            minorTransitionInProgress = false;
        }


        private IEnumerator MoveIndividualMinor(GameObject minor, Vector3 location, float distance)
        {

            // destroy the navmeshagent to stop all movement/collisions
            Destroy(minor.GetComponent<UnityEngine.AI.NavMeshAgent>());

            // tell the AI controller to stop movement
            minor.GetComponent<IStatePatternEnemy>().stopAI();

            // start the merge animation for the minor
            minor.GetComponent<Animator>().SetTrigger("isMerge");

            var startLerpPos = minor.transform.position;

            float i = 0f;
            while (i <= 1f)
            {
                // set the slerp step with some algorithm that bearly works
                i += Time.deltaTime * ((distance/(100 - (100-distance)))/2);
                minor.transform.position = Vector3.Slerp(startLerpPos, location, i);
                yield return new WaitForEndOfFrame();
            }

        }
        
    }


    // this is to add the functionality when you click a certain boolean value
    // something else pops up. In this case, when you click the randomize waypoints
    // checkbox, an integer field will pop up
    #region Custom Editor Script
    [CustomEditor(typeof(MinorZone))]
    public class MinorZoneEditorScript : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var minorZone = target as MinorZone;

            if (minorZone.randomizeWaypoints)
            {
                minorZone.randomizedWaypointCount = EditorGUILayout.IntField("Waypoints", minorZone.randomizedWaypointCount);
            }
            
        }
    }
    #endregion
}


