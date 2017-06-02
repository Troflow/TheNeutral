using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaText : MonoBehaviour, IHUDObserver<PlayerState> 
{
	[SerializeField]
	private HUDManager _HUDManager;

	/// <summary>
	/// The text mesh component of this instance
	/// </summary>
	private TextMesh _TextMesh;


	void OnEnable()
	{
		Debug.Log ("Text: Enabled and Subscribed");
		_TextMesh = GetComponent<TextMesh>();
		// when set to active, add this instance of IHUDObserver to the HUDManager's list of observers
		_HUDManager.Subscribe (this);
	}

	void OnDisable()
	{
		Debug.Log ("Text: Disabled and Disposed");
		// when set to active, add this instance of IHUDObserver to the HUDManager's list of observers
		_HUDManager.Dispose (this);
	}

	/// <summary>
	/// Update this instance of HUD observer
	/// change text value to reflect current amount of stamina
	/// </summary>
	/// <param name="updatedData">Updated info.</param>
	public void OnNext(PlayerState newState)
	{
		_TextMesh.text = "" + newState.stamina;
	}
}
