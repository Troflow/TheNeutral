using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHUDObservable<T> 
{
	/// <summary>
	/// Subscribe the specified IHUDObserver to list of observers
	/// </summary>
	/// <param name="IHUDObserver">IHUD observer.</param>
	void Subscribe(IHUDObserver<T> IHUDObserver);
}
