using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	// All HUDElements must be added to the Script Execution Order to prevent
	// NullReferenceExeption Errors
	public class HUDElement : MonoBehaviour {

		public static HUDManager HUDManager;
	}
}