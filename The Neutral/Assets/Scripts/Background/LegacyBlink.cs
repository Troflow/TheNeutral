using UnityEngine;
using System.Collections;

public class LegacyBlink : MonoBehaviour {

	public Light light1;
	public float orig;
	public int lightRestoreInterval;
	public float timePerInterval;
	private bool reset;
	// Use this for initialization
	void Start () {
		//get light attached and current intensity
		light1 = GetComponent<Light> ();
		orig = light1.intensity;
		reset = true;
		lightRestoreInterval = 10;
		//this interval time is multipled by a random number
		timePerInterval = 0.09f;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(1)) {
			light1.intensity = 0;
			reset = false;
		}
		if (Input.GetMouseButtonUp(1) && !reset) {
			Debug.Log ("Starting CoRoutine");
			StartCoroutine(Blindness());
		}

	}

	IEnumerator Blindness () {
		Debug.Log ("Starting: "+Time.time);
		reset = true;

		//yield return new WaitForSeconds(Random.Range(1,3));
		for (float x=0; x<orig; x+=orig/lightRestoreInterval)
		{
			yield return new WaitForSeconds(Random.Range (1,3)*timePerInterval);
			light1.intensity = x;
		}
		Debug.Log ("Ending: " + Time.time);
		light1.intensity = orig;
	}
}
