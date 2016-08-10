using UnityEngine;
using System.Collections;
//using System.Drawing; find way for unity to include or use general conversion of rgb -> hsv

public class SkyboxController : MonoBehaviour {

	public Skybox skybox;
	public Camera maincam;
	public float r;
	public float g;
	public float b;
	public float h,s,v;

	public bool AutoChange;

	public Color skbox_new;
	// Use this for initialization
	void Start () {
		skybox = GetComponent<Skybox> ();
		maincam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("hsv: "+ h + "," + s +"," + v);
		maincam.backgroundColor = skbox_new;
		if (Input.GetKey ("a")) {
			skybox.enabled = false;
		} 
		else if (Input.GetKey ("b")) {
			skybox.enabled = true;
		}

		if (Input.GetKey ("up")) {
			g = 1 + g%255;
		}
		if (Input.GetKey ("left")) {
			r = 1 + r%255;
		}
		if (Input.GetKey ("right")) {
			b = 1 + b%255;
		}


		if (AutoChange) {
			skbox_new = new Color(r/255,g/255,b/255);
			EditorGUIUtility.RGBToHSV(skbox_new,out h,out s,out v); 
			skybox.material.SetColor ("_Color",skbox_new);
		}

		else if (Input.GetKey ("return")) {
			skbox_new = new Color(r/255,g/255,b/255);
			EditorGUIUtility.RGBToHSV(skbox_new,out h,out s,out v); 
			skybox.material.SetColor ("_Color",skbox_new);
		}

		if (Input.GetKey ("s")) {

			StartCoroutine(DeSaturation());
		}
	}

	public IEnumerator DeSaturation() {
		for (float sa=s; sa>=0.1f; sa-=0.05f)
		{
			skybox.material.SetColor ("_Color", EditorGUIUtility.HSVToRGB(h,sa,v));
			Debug.Log("current saturation: "+sa);
			yield return new WaitForSeconds(0.2f);

		}
	}

}

	
		//skybox.material.SetColor ("_Color", h,s,v);
		//maincam.backgroundColor = skbox_new;
