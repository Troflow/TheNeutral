using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPoint : MonoBehaviour {

	public bool hasCollided;

	// Use this for initialization
	void Start () {
		
	}

	public void OnTriggerEnter(Collider col)
	{
		if (!col.CompareTag ("Player")) 
		{
			hasCollided = true;
		}
			
	}
		
	public void OnTriggerExit(Collider col)
	{
		if (!col.CompareTag ("Player")) 
		{
			hasCollided = false;
		}
	}
}
