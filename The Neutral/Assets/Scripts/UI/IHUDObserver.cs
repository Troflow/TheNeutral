using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHUDObserver<T> 
{

	/// <summary>
	/// Update this instance of HUD observer
	/// </summary>
	void OnNext(T updatedData);
}
