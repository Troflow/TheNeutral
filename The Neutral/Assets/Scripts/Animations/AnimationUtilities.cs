using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class AnimationUtilities : MonoBehaviour
    {
        public bool AnimationFinished(AnimatorStateInfo animState)
        {
            Debug.Log("Normalized Time/Length: " + animState.normalizedTime + "/" + animState.length);
            Debug.Log("Normalized Time % length: " + animState.normalizedTime % animState.length);
            if (animState.normalizedTime % animState.length > 0 && animState.normalizedTime >= animState.length)
            {
                return true;
            }
            return false;
        }
		public bool AnimationFinished(AnimatorStateInfo animState, float length)
		{
			Debug.Log("Normalized Time/Length (MODIFIED): " + animState.normalizedTime + "/" + length);
			Debug.Log("Normalized Time % length (MODIFIED): " + animState.normalizedTime % length);
			if (animState.normalizedTime % length > 0 && animState.normalizedTime >= length)
			{
				return true;
			}
			return false;
		}
    }
}