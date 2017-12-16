using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Neutral
{
    /// <summary>
    /// Class that handles the 'Clyde' mechanic, where the Player
    /// closes their eyes and reveals hidden gameplay elements
    /// </summary>
    public class Blink : MonoBehaviour
    {
        private Camera playerCamera;
        private Transform topEyelid;
        private Animation topEyelidAnim;

        private float closeSpeed = 0.8f;
        private float openSpeed = -0.65f;

        private Transform bottomEyelid;
        private Animation bottomEyelidAnim;

        void Start()
        {
            playerCamera = Camera.main;
            topEyelid = playerCamera.transform.GetChild(0);
            topEyelidAnim = topEyelid.gameObject.GetComponent<Animation>();

            bottomEyelid = playerCamera.transform.GetChild(1);
            bottomEyelidAnim = bottomEyelid.gameObject.GetComponent<Animation>();
        }

        public void openEyes()
        {
            // Set normalizedTime to 0.65, so it begins to rewind animation
            // from its end, rather than jumping to its beginning
            topEyelidAnim["Blink_Top"].normalizedTime = 0.65f;
            bottomEyelidAnim["Blink_Bottom"].normalizedTime = 0.65f;

            topEyelidAnim["Blink_Top"].speed = openSpeed;
            bottomEyelidAnim["Blink_Bottom"].speed = openSpeed;

            topEyelidAnim.Play("Blink_Top");
            bottomEyelidAnim.Play("Blink_Bottom");
        }

        public void closeEyes()
        {
            topEyelidAnim["Blink_Top"].speed = closeSpeed;
            bottomEyelidAnim["Blink_Bottom"].speed = closeSpeed;

            topEyelidAnim.Play("Blink_Top");
            bottomEyelidAnim.Play("Blink_Bottom");

            topTimeInAnim = topEyelidAnim["Blink_Top"].normalizedTime;
            bottomTimeInAnim = bottomEyelidAnim["Blink_Bottom"].normalizedTime;

            // TODO: Only when eyes are FULLY closed, we reveal what needs to be revealed
            // NOTE: Make the act of closing eyes less nauseating.
        }
    }
}