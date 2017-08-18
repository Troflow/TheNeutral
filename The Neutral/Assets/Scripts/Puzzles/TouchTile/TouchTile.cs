using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class TouchTile : MonoBehaviour {

		public bool isAware;

		public void OnTriggerEnter(Collider pColl)
		{
			if (pColl.CompareTag("Player"))
			{
				transform.parent.GetComponent<TouchTileField> ().touched (isAware, transform, pColl.transform);
			}
		}
	}
}
