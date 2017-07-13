using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public interface IObserver<T> 
	{

		/// <summary>
		/// Update this instance of HUD observer
		/// </summary>
		void OnNext(T updatedData);
	}
}
