using UnityEngine;
using System.Collections;

namespace Neutral {
	public class CombineMinor : MonoBehaviour {
		private int MinorCount=0;
		private Vector3 majorSpawnPoint = new Vector3();
		public GameObject minor_spawn;
		public GameObject major_spawn;
	
		public GameObject[] minorContainer;

		bool finishedForceMove=false;


		//THESE SET OF VALUES ARE FOR THE MERGE MOVEMENT, AND CLONE SPAWNS, ETC.
		public float animationTime;
		public int intervals;
		//calculated by animationTime/intervals so no need to instantiate
		private float timePerInterval;

		//we assume that the minors are split into 3 groups, one in the centre, other 2 += some X distance away
		//with this assumption, we can normalize the distance and apply the same amount of distance per interval
		public int groupDistance;
		//this gives it slight variation, BE CAREFUL THOUGH! higher variation = less towards the centre at the end
		//default is the radius of the minor ~1.8
		public int tolerance;
		//done with internal calculations
		private float distancePerIntervalDivisor;



		// Use this for initialization
		void Start () {
			minorContainer = new GameObject[50];
			distancePerIntervalDivisor = 1/((float)groupDistance/(float)intervals);
			timePerInterval = animationTime/intervals;
			Debug.Log ("DPIV: "+ distancePerIntervalDivisor);
		}


		void Update () {
			if (Input.GetKeyDown("l")) {
				minorContainer[MinorCount]=(GameObject)Instantiate(minor_spawn,
				                                                   new Vector3(minor_spawn.transform.position.x+(((-1)^(MinorCount%2)*groupDistance)+Random.Range(-tolerance,tolerance)),
				                                                               minor_spawn.transform.position.y,
				           				 									   minor_spawn.transform.position.z+(((-1)^(MinorCount%2)*groupDistance)+Random.Range(-tolerance,tolerance))),
				                                                   minor_spawn.transform.rotation);
				MinorCount+=1;
				Debug.Log("Minors in game: " + MinorCount);
			}


			if (Input.GetKeyDown("x")) {
				for (int x=0; x<MinorCount; x++)
				{
					majorSpawnPoint.x+=minorContainer[x].transform.position.x;
					majorSpawnPoint.y=major_spawn.transform.position.y;
					majorSpawnPoint.z+=minorContainer[x].transform.position.z;
				}
				majorSpawnPoint.x = majorSpawnPoint.x/MinorCount;
				majorSpawnPoint.z = majorSpawnPoint.z/MinorCount;

				StartCoroutine("ForceMove",majorSpawnPoint);
			}

		}

		IEnumerator ForceMove(Vector3 location) {
			Debug.Log (location);
			for (int x=0; x<intervals; x++)
			{
				for (int z=0; z<MinorCount; z++) {
					if (x==0){
						minorContainer[z].GetComponent<NavMeshAgent>().radius = 0.01f;
						minor_spawn.SetActive(false);
					}

					if (minorContainer[z].transform.position == location) {
						Debug.Log("MINONS HAVE REACHED");
					}
					else {
						minorContainer[z].GetComponent<NavMeshAgent>().Move((location - minorContainer[z].transform.position).normalized/distancePerIntervalDivisor);
					}
					minorContainer[z].transform.localScale = minorContainer[z].transform.localScale - minorContainer[z].transform.localScale/80;
					minorContainer[z].transform.position.Set(minorContainer[z].transform.position.x, 
					                                         minorContainer[z].transform.position.y+minorContainer[z].transform.position.y/5, 
					                                         minorContainer[z].transform.position.z);
					if (x==intervals-1) {
						minorContainer[z].SetActive(false);
					}
				}
				yield return new WaitForSeconds(timePerInterval);
			}
			majorSpawnPoint.y = minor_spawn.transform.position.y;
			GameObject major_final = (GameObject)Instantiate(major_spawn,majorSpawnPoint,major_spawn.transform.rotation);
			major_final.SetActive(true);
			//MajorNavMeshController realMajor = (MajorNavMeshController)major_final.GetComponent("MajorNavMeshController");
			//Animator tmpanim = realMajor.GetComponent<Animator> ();
			//tmpanim.SetBool(Animator.StringToHash("isSpawn"),true);
			Animator anim = major_final.GetComponent<Animator> ();
			anim.SetBool("isSpawn",true);
		}
	}
}
