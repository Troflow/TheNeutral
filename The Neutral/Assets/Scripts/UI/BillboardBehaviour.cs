using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class BillboardBehaviour : MonoBehaviour {

		private Camera mainCamera;

		// Use this for initialization
		void Start () {
			mainCamera = Camera.main;
		}
		
		// Update is called once per frame
		void Update () {
			transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
				mainCamera.transform.rotation * Vector3.up);
		}
	}
}
