using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;

public class MinorClick : MonoBehaviour {

	public float speed;
	public float turningSpeed;
	public float pickNextWaypointDist = 2;
	public float forwardLook = 1;
	public float endReachedDistance = 0.2F;

	/** Do a closest point on path check when receiving path callback.
	 * Usually the AI has moved a bit between requesting the path, and getting it back, and there is usually a small gap between the AI
	 * and the closest node.
	 * If this option is enabled, it will simulate, when the path callback is received, movement between the closest node and the current
	 * AI position. This helps to reduce the moments when the AI just get a new path back, and thinks it ought to move backwards to the start of the new path
	 * even though it really should just proceed forward.
	 */
	public bool closestOnPathCheck = true;

	/** Current index in the path which is current target */
	protected int currentWaypointIndex = 0;
	/** Current path which is followed */
	protected Path path;
	
	/** Cached CharacterController component */
	protected CharacterController controller;




	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
