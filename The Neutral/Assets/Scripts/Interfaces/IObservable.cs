using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public interface IObservable<T> 
	{
		/// <summary>
		/// Subscribe the specified IHUDObserver to list of observers
		/// </summary>
		/// <param name="IHUDObserver">IHUD observer.</param>
		void Subscribe(IObserver<T> IHUDObserver);
	}
}
