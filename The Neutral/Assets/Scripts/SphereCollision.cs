using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollision : MonoBehaviour {

    private Color[] colorTransitions;

    private float duration = 1.5f;
    private int currIndex = 0;

    Renderer sphereRenderer;

    private float currentLerpProgress = 0;

    // Use this for initialization
    void Start () {
        colorTransitions = new Color[4];
        colorTransitions[0] = Color.black;
        colorTransitions[1] = Color.gray;
        colorTransitions[2] = Color.blue;
        colorTransitions[3] = Color.cyan;

        sphereRenderer = GetComponent<Renderer>();

    }
	
	// Update is called once per frame
	void Update () {
		
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
        Debug.Log(col.gameObject.name);
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

    }
}
