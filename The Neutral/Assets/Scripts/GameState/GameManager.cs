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
        #region DAYDREAM

        public static float pulseSpeed = 20f;
        public static float pulseDistanceEpsilon = 0.5f;

        #endregion

        #region COLOR WHEEL

        public static float colorTransferTimeStep = 2f;
        public static float colorWheelRotateSpeed = 25f;

        #endregion

        #region BLINKABLE

        public static float shaderFadeValue = 1f;
        public static BlinkState playerBlinkState = BlinkState.EyesOpen;

        #endregion
	}
}
