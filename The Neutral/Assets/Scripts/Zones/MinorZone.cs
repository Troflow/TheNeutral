using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Neutral {

    public class MinorZone : MonoBehaviour, IZone
    {
        [HideInInspector]
        private GameObject minor_spawn;
        [HideInInspector]
        private GameObject major_spawn;

        [HideInInspector]
        public Transform[] waypoints;

        public bool randomizeWaypoints;
        [HideInInspector]
        public int randomizedWaypointCount;

        //this list contains all the minor gameobjects in the zone
        private IList<GameObject> group;
        //this list contains a record of all unique minors that were once and currently in the zone
        private IList<int> allInstanceIDs;

        private bool minorTransitionInProgress;


        // Use this for initialization
        void Start()
        {

            if (randomizeWaypoints)
            {
                if (randomizedWaypointCount <= 0)
                {
                    throw new System.Exception("Can not have 0 waypoints");
                }
                waypoints = new Transform[randomizedWaypointCount];
                for (int x=0; x<randomizedWaypointCount; x++)
                {
                    GameObject waypoint = Instantiate(new GameObject("MinorWaypoint" + (x + 1)), this.transform, false);
                    waypoint.transform.position = (Random.insideUnitSphere * 100) + this.transform.position;
                    waypoints[x] = waypoint.transform;
                }

            }
            else
            {
                for (int x = 0; x < 999; x++)
                {
                    Transform waypoint = transform.FindChild("MinorWaypoint" + (x + 1));
                    if (waypoint == null)
                    {
                        break;
                    }
                    waypoints[x] = waypoint.transform;
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
            //we must check if the minor being added is a unique minor
            //this is to circumvent the bug when removing a minor,
            //the auto detecting collision instantly adds it back before the gameobject
            //can actually delete causing wrong behaviour.

            if (allInstanceIDs.Contains(minor.GetInstanceID()))
            {
                //print("Duplicate minor, not adding");
                return;
            }
            allInstanceIDs.Add(minor.GetInstanceID());
            minor.GetComponent<MinorStatePatternEnemy>().setWaypoints(waypoints);
            group.Add(minor);

            //Debug.Log("current minors in zone after add:  " + group.Count);
        }

        public void delete(GameObject minor)
        {
            group.Remove(minor);
            //Debug.Log("current minors in zone after del: " + group.Count);

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
            //Debug.Log(minor.GetComponent<MinorNavMeshController>().getSpawnZone());
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
                    for (int x = 0; x < group.Count; x++)
                    {
                        group[x].GetComponentInParent<MinorNavMeshController>().setMinorCollision(false);
                        majorSpawnPoint.x += group[x].transform.position.x;
                        majorSpawnPoint.y = group[x].transform.position.y;
                        majorSpawnPoint.z += group[x].transform.position.z;
                    }
                    majorSpawnPoint.x = majorSpawnPoint.x / group.Count;
                    majorSpawnPoint.z = majorSpawnPoint.z / group.Count;

                    //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), majorSpawnPoint, Quaternion.identity);
                    if (!minorTransitionInProgress)
                    {
                        StartCoroutine("ForceMove", majorSpawnPoint);
                    }
                    break;


                default:
                    break;
            }
        }


        private IEnumerator ForceMove(Vector3 location)
        {
            minorTransitionInProgress = true;
            for (int x=0; x<group.Count; x++)
            {
                var heading = location - group[x].transform.position;
                var distance = heading.magnitude;
                var minor = group[x];
                minor.transform.SetPositionAndRotation(minor.transform.position, Quaternion.identity);
                StartCoroutine(MoveIndividualMinor(group[x], location, distance));
            }

            float startTime = Time.time;

            while (Time.time - startTime < 3.66667f)
            {
                yield return new WaitForEndOfFrame();
            }

            //BE CAREFUL OF THIS, NOT SURE OF EXPECTED BEHAVIOUR OF OTHER COROUTINES RUNNING
            StopAllCoroutines();

            foreach (GameObject minorInZone in group)
            {
                Destroy(minorInZone);
            }
            group.Clear();
            GameObject major_final = Instantiate(major_spawn, location, major_spawn.transform.rotation);
            major_final.SetActive(true);
            Animator anim = major_final.GetComponent<Animator>();
            anim.SetBool("isSpawn", true);
            minorTransitionInProgress = false;

        }


        private IEnumerator MoveIndividualMinor(GameObject minor, Vector3 location, float distance)
        {
            Destroy(minor.GetComponent<UnityEngine.AI.NavMeshAgent>());
            
            minor.GetComponent<MinorStatePatternEnemy>().stopAI();
            minor.GetComponent<Animator>().SetTrigger("isMerge");
            float i = 0f;
            var startLerpPos = minor.transform.position;

            float startTime = Time.time;

            while (i <= 1f)
            {
                i += Time.deltaTime * ((distance/(100 - (100-distance)))/2);
                minor.transform.position = Vector3.Slerp(startLerpPos, location, i);
                yield return new WaitForEndOfFrame();
            }

        }
        
    }


    [CustomEditor(typeof(MinorZone))]
    public class MinorZoneEditorScript : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var minorZone = target as MinorZone;
            //minorZone.randomizeWaypoints = GUILayout.Toggle(minorZone.randomizeWaypoints, "Randomize Waypoints");

            if (minorZone.randomizeWaypoints)
            {
                minorZone.randomizedWaypointCount = EditorGUILayout.IntField("Waypoints", minorZone.randomizedWaypointCount);
            }
            
        }
    }
}


