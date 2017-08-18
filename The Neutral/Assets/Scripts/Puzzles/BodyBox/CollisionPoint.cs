using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// Collision point class, used to replicate
	/// collision detection and response for isTrigger
	/// boxColliders on the BodyBoxes
	/// </summary>
	public class CollisionPoint : MonoBehaviour {

		public bool hasCollided;

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
}
