using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Neutral
{
	public class PlayerState : MonoBehaviour 
	{

		[SerializeField]
		private HUDManager HUD;

		#region Player Attributes
		public int stamina;
		public bool isExhausted;
		public Lite heldColor;
        public MeshRenderer flag;
        #endregion

        #region CombatColors

        [SerializeField]
        public List<CombatColor> colorBook;

        private CombatColor currentCombatColor;

        private Color randomColor;
        #endregion


        public List<Lite> appliedStacks;
		public List<Lite> colorSchema;


		public Dictionary<Lite, int> completedPuzzles;
		public Dictionary<Lite, int> defeatedEnemies;
        public Dictionary<Lite, Color> flagColor;
        public List<Memory> collectedMemories;

        private bool isFlagPulsing;
        private const int dashSpeed = 25;

        void Awake()
		{
			stamina = 100;
			populateCompletedPuzzles ();
            initializeFlagColors();
			HUD.setPlayerState (this);
			HUD.setPlayerTransform (this.transform);
            flag = GameObject.FindGameObjectWithTag("PlayerFlag").GetComponent<MeshRenderer>();
            isFlagPulsing = false;

            currentCombatColor = new CombatColorGreen();
            colorBook = new List<CombatColor>();
            colorBook.Add(currentCombatColor.TestSubtractColor(currentCombatColor));
            randomColor = colorBook[0].color.Value;

            print(randomColor);

        }

        public static int getDashSpeed()
        {
            return dashSpeed;
    }

        public CombatColor getCurrentCombatColor()
        {
            return currentCombatColor;
        }

        public void setCurrentCombatColor(CombatColor newCombatColor)
        {
            currentCombatColor = newCombatColor;
        }

		private void populateCompletedPuzzles()
		{
			completedPuzzles = new Dictionary<Lite, int> ();

			completedPuzzles.Add (Lite.GREEN, 0);
			completedPuzzles.Add (Lite.BLUE, 0);
			completedPuzzles.Add (Lite.YELLOW, 0);
			completedPuzzles.Add (Lite.RED, 0);
			completedPuzzles.Add (Lite.GRAY, 0);
		}

        private void initializeFlagColors()
        {
            flagColor = new Dictionary<Lite, Color>
            {
                { Lite.BLACK, Color.black },
                { Lite.BLUE, Color.blue },
                { Lite.BROWN, new Color(102,51,0) },
                { Lite.GOLD, new Color(255,223,0) },
                { Lite.GRAY, Color.gray },
                { Lite.GREEN, Color.green },
                { Lite.RED, Color.red },
                { Lite.WHITE, Color.white },
                { Lite.YELLOW, Color.yellow }
            };
        }

		public void onStateChange()
		{
			HUD.NotifyAllObservers ();
		}

        private IEnumerator PulseFlag(Lite newColor, float pulseTime)
        {
            isFlagPulsing = true;
            float startPulseTime = Time.time;
            List<int> speedFactors = new List<int> { 2, 3, 4, 5 };
            int inverseSpeedFactorIndex = speedFactors.Count - 1;
            int currSpeedFactorIndex = 0;
            float defaultWaitTime = 0.5f;
            while (isFlagPulsing)
            {
                float currentPulseTime = Time.time - startPulseTime;

                Color color = flag.material.color;
                flag.material.color = flagColor[newColor];

                if (currentPulseTime > pulseTime / speedFactors[inverseSpeedFactorIndex])
                {
                    if (inverseSpeedFactorIndex > 0) inverseSpeedFactorIndex -= 1;
                    if (currSpeedFactorIndex < speedFactors.Count-1) currSpeedFactorIndex += 1;
                }

                yield return new WaitForSeconds(defaultWaitTime / speedFactors[currSpeedFactorIndex]);

                color.a = 1f;
                flag.material.color = flagColor[heldColor];
                if (currentPulseTime > pulseTime / speedFactors[inverseSpeedFactorIndex])
                {
                    if (inverseSpeedFactorIndex > 0) inverseSpeedFactorIndex -= 1;
                    if (currSpeedFactorIndex < speedFactors.Count-1) currSpeedFactorIndex += 1;
                }

                yield return new WaitForSeconds(defaultWaitTime / speedFactors[currSpeedFactorIndex]);
            }
            
        }

        public void pulseFlag(Lite newColor, float colorTransferTime)
        {
            if (!isFlagPulsing)
            {
                StartCoroutine(PulseFlag(newColor, colorTransferTime));
            }
        }

        public void stopPulseFlag()
        {
            StopAllCoroutines();
            isFlagPulsing = false;
        }

        // For Debugging purposes.
        private void handleInput()
		{
			if (Input.GetKey (KeyCode.DownArrow)) 
			{
				stamina -= 1;
				stamina = Mathf.Clamp (stamina, 0, 100);
				onStateChange ();
			}

			if (Input.GetKey (KeyCode.UpArrow)) 
			{
				stamina += 1;
				stamina = Mathf.Clamp (stamina, 0, 100);
				onStateChange ();
			}

		}
		
		// Update is called once per frame
		void Update () {
			handleInput ();
            if (!isFlagPulsing)
            {
                #region TEST ADD COLOR WITH RANDOM COLOR
                //if (Input.GetKeyDown(KeyCode.Y))
                //{
                //    colorBook.Add(currentCombatColor.TestSubtractColor(currentCombatColor));
                //    randomColor = colorBook[colorBook.Count - 1].color.Value;
                //}
                //flag.material.color = randomColor;
                #endregion

                #region ColorWheel puzzle
                //flag.material.color = flagColor[heldColor];
                #endregion


                #region TEST COMBAT COLOR
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    List<Color> keys = CombatColor.colorLookupTable.Keys.ToList();
                    int randInd = UnityEngine.Random.Range(0, CombatColor.colorLookupTable.Count);
                    CombatColor randomColorFromDict = CombatColor.colorLookupTable[keys[randInd]];
                    currentCombatColor = randomColorFromDict;
                }

                flag.material.color = currentCombatColor.color.Value;
                #endregion

            }
            
        }
    }
}
