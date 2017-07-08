using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class HUDManager : MonoBehaviour, IObservable<PlayerState> 
	{

		/// <summary>
		/// List of all HUD elements that are observers of player's state.
		/// </summary>
		private List<IObserver<PlayerState>> observers;

		private PlayerState playerState;
		private Transform player;
		private Vector3 offset;
		private float smoothing = 5f;
		private float lockPos = 0f;


		void Awake () {
			HUDElement.HUDManager = this;
			observers = new List<IObserver<PlayerState>> ();
		}

		void Start()
		{
			offset = transform.position - player.position;
		}

		public void setPlayerState(PlayerState pPlayerState)
		{
			playerState = pPlayerState;
		}

		public void setPlayerTransform(Transform pPlayerTransform)
		{
			player = pPlayerTransform;
		}

		#region Observor Methods
		/// <summary>
		/// Subscribe the specified IHUDObserver to list of observers.
		/// </summary>
		/// <param name="IHUDObserver">IHUD observer.</param>
		/// <param name="observer">Observer.</param>
		public void Subscribe(IObserver<PlayerState> observer)
		{
			if (!observers.Contains (observer)) 
			{
				observers.Add (observer);
			}

			observer.OnNext (playerState);
		}

		public void NotifyAllObservers()
		{
			foreach (IObserver<PlayerState> observer in observers)
			{
				observer.OnNext (playerState);
			}
		}

		/// <summary>
		/// Dispose the specified observer from the list of observers, so it is no longer notified.
		/// </summary>
		/// <param name="observer">Observer.</param>
		public void Dispose(IObserver<PlayerState> observer)
		{
			if (observers.Contains (observer)) 
			{
				observers.Remove (observer);
			}
		}
		#endregion

		void FixedUpdate () 
		{
			var targetPos = player.position + offset;
			transform.position = targetPos;
		}
	}
}
