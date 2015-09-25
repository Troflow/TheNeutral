using UnityEngine;
using System.Collections;

public class ModifyMaterial : MonoBehaviour {

    private Renderer ren;
    public Material[] materials;
    public int i;
	// Use this for initialization
	void Start () {
        ren = GetComponent<Renderer>();
        materials = Resources.LoadAll<Material>("Materials");
        Debug.Log("We have "+materials.Length+" materials loaded");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp("i"))
        {
            ren.material = materials[i%materials.Length];
            i += 1;
        }

	}
}
