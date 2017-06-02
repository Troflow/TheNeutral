using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour, IHUDObservable<PlayerState> 
{

	/// <summary>
	/// List of all HUD elements that are observers of Remy's state.
	/// </summary>
	private List<IHUDObserver<PlayerState>> _observers;

	[SerializeField]
	private PlayerState _RemyState;


	[SerializeField]
	private Transform _player;
	private Vector3 _offset;
	private float smoothing;


	// Use this for initialization
	void Awake () {
		_observers = new List<IHUDObserver<PlayerState>> ();
	}

	void Start()
	{
		_offset = transform.position - _player.position;
		smoothing = 5f;
	}

	/// <summary>
	/// Subscribe the specified IHUDObserver to list of observers
	/// </summary>
	/// <param name="IHUDObserver">IHUD observer.</param>
	/// <param name="observer">Observer.</param>
	public void Subscribe(IHUDObserver<PlayerState> observer)
	{
		if (!_observers.Contains (observer)) 
		{
			_observers.Add (observer);
		}

		observer.OnNext (_RemyState);
	}

	public void NotifyAllObservers()
	{
		foreach (IHUDObserver<PlayerState> observer in _observers)
		{
			observer.OnNext (_RemyState);
		}
	}

	/// <summary>
	/// Dispose the specified observer from the list of observers, so it is no longer notified
	/// </summary>
	/// <param name="observer">Observer.</param>
	public void Dispose(IHUDObserver<PlayerState> observer)
	{
		if (_observers.Contains (observer)) 
		{
			_observers.Remove (observer);
		}
	}
		

	void FixedUpdate () {

		Vector3 targetCamPos = _player.position + _offset;

		//transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
