using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neutral
{
    /// <summary>
    /// Class containing static attributes all throughout the game
    /// </summary>
	public static class GameManager
	{
        #region COLOR WHEEL

        public static float colorTransferTimeStep = 2f;
        public static float colorWheelRotateSpeed = 25f;

        #endregion

        #region BLINKABLE

        public static float shaderFadeValue = 1f;
        public static bool playerEyesOpen = true;

        #endregion
	}
}
