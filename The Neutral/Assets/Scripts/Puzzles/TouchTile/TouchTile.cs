using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class TouchTile : MonoBehaviour {

		public bool isAware;

		public void OnTriggerEnter(Collider pColl)
		{
            //Debug.Log(pColl.name);
			if (pColl.CompareTag("Player-Sphere"))
			{
				transform.parent.GetComponent<TouchTileField> ().touched (isAware, transform, pColl.transform);
			}
		}
	}
}
