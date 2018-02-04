using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neutral
{
	// All HUDElements must be added to the Script Execution Order to prevent
	// NullReferenceExeption Errors
	public class ColorTransferBar : HUDElement, IObserver<PlayerState>
	{
		private Transform transferSlider;
        private Transform transferColorIcon;

		void OnEnable ()
		{
            transferSlider = transform.Find("ColorTransferSlider");
            transferColorIcon = transform.Find("TransferColorIcon");

            transferSlider.gameObject.SetActive(false);
            disableAllChildren(transferColorIcon);

            HUDManager.Subscribe (this);
		}

		void OnDisable ()
		{
			HUDManager.Dispose (this);
		}

        void updateIncomingColorIcon(CombatColor incomingTransferColor, bool displayCondition)
        {
            if (!displayCondition)
            {
                disableAllChildren(transferColorIcon);
                return;
            }

            var transferColor = incomingTransferColor.color.Value;

            transferColorIcon.Find("R").gameObject.SetActive(transferColor.r > 0.5f);
            transferColorIcon.Find("G").gameObject.SetActive(transferColor.g > 0.5f);
            transferColorIcon.Find("B").gameObject.SetActive(transferColor.b > 0.5f);
        }

		void updateColorTransferBar(float colorTransferValue, CombatColor incomingTransferColor)
		{
            var slider = transferSlider.GetComponent<Slider>();
            var displayCondition = colorTransferValue > 0f && colorTransferValue < 1f;
            transferSlider.gameObject.SetActive(displayCondition);
            updateIncomingColorIcon(incomingTransferColor, displayCondition);
            slider.value = colorTransferValue;
		}

        void disableAllChildren(Transform parent)
		{
			foreach (Transform child in parent)
			{
				child.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Update this instance of HUD observer
		/// </summary>
		/// <param name="newState">New state.</param>
		public void OnNext(PlayerState newState)
		{
			var colorTransferValue = newState.getColorTransferValue();
            var incomingTransferColor = newState.getIncomingTransferColor();
			updateColorTransferBar(colorTransferValue, incomingTransferColor);
		}
	}
}