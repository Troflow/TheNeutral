﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchTile : MonoBehaviour {

	public bool isAware;

	public void OnTriggerEnter(Collider pColl)
	{
		if (pColl.CompareTag("Player") && isAware)
		{
			transform.parent.GetComponent<TouchTileField> ().touched (transform);
		}
	}
}
