using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaRing : MonoBehaviour, IHUDObserver<PlayerState> 
{
	[SerializeField]
	private HUDManager _HUDManager;
	private Image _StaminaRing;


	void OnEnable ()
	{
		_StaminaRing = GetComponent<Image> ();
		_HUDManager.Subscribe (this);
	}

	void Start()
	{
		_StaminaRing = GetComponent<Image> ();
		_HUDManager.Subscribe (this);
	}
	void OnDisable () 
	{
		_HUDManager.Dispose (this);
	}
		
	/// <summary>
	/// Update this instance of HUD observer
	/// </summary>
	/// <param name="newState">New state.</param>
	public void OnNext(PlayerState newState)
	{
		_StaminaRing.fillAmount = 0.01f * newState.stamina;
	}
		

}
