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

		// Use this for initialization
		void Start () {
			minorContainer = new GameObject[50];
		}


		void Update () {
			if (Input.GetKeyDown("l")) {
				minorContainer[MinorCount]=(GameObject)Instantiate(minor_spawn,new Vector3(minor_spawn.transform.position.x+Random.Range(-15,15),minor_spawn.transform.position.y,minor_spawn.transform.position.z+Random.Range(-15,15)),minor_spawn.transform.rotation);
				MinorCount+=1;
				Debug.Log("Minors in game: " + MinorCount);
			}


			if (Input.GetKeyDown("x")) {
				for (int x=0; x<MinorCount; x++)
				{
					majorSpawnPoint.x+=minorContainer[x].transform.position.x;
					majorSpawnPoint.y=major_spawn.transform.position.y;;
					majorSpawnPoint.z+=minorContainer[x].transform.position.z;
				}
				majorSpawnPoint.x = majorSpawnPoint.x/MinorCount;
				majorSpawnPoint.z = majorSpawnPoint.z/MinorCount;

				StartCoroutine("ForceMove",majorSpawnPoint);
			}

		}

		IEnumerator ForceMove(Vector3 location) {
			Debug.Log (location);
			for (int x=0; x<80; x++)
			{
				for (int z=0; z<MinorCount; z++) {

					if (minorContainer[z].transform.position == location) {
						Debug.Log("MINONS HAVE REACHED");
					}
					else minorContainer[z].GetComponent<NavMeshAgent>().Move((location - minorContainer[z].transform.position).normalized/5);
					minorContainer[z].transform.localScale = minorContainer[z].transform.localScale - minorContainer[z].transform.localScale/50;
					if (x==79) {
						minorContainer[z].SetActive(false);
					}
				}
				yield return new WaitForSeconds(0.05f);
			}
			GameObject major_final = (GameObject)Instantiate(major_spawn,majorSpawnPoint,major_spawn.transform.rotation);
			major_final.SetActive(true);
			MajorNavMeshController realMajor = (MajorNavMeshController)major_final.GetComponent("MajorNavMeshController");
			Animator tmpanim = realMajor.GetComponent<Animator> ();
			tmpanim.SetBool(Animator.StringToHash("isSpawn"),true);
		}
	}
}
