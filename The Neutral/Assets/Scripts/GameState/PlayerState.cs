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
        public CombatColor exposedColor;
        private CombatColor currentCombatColor;
        private CombatColor incomingTransferColor;
        private float colorTransferValue;
        private bool isBeingGrantedNewColor;
        private Coroutine grantingColorCoroutine;

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
            flag = GameObject.FindGameObjectWithTag("PlayerFlag").GetComponent<MeshRenderer>();
            isFlagPulsing = false;

            colorTransferValue = 0f;
            incomingTransferColor = CombatColor.colorLookupTable[Color.black];
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
            onStateChange();
        }

        public float getColorTransferValue()
        {
            return colorTransferValue;
        }

        public void setColorTransferValue(float newValue)
        {
            colorTransferValue = newValue;
            onStateChange();
        }

        public CombatColor getIncomingTransferColor()
        {
            return incomingTransferColor;
        }

        public void setIncomingTransferColor(CombatColor incomingColor)
        {
            incomingTransferColor = incomingColor;
            onStateChange();
        }


        public bool getIsBeingGrantedNewColor()
        {
            return isBeingGrantedNewColor;
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


        private IEnumerator grantColor(CombatColor newColor)
		{
            isBeingGrantedNewColor = true;

            // If PlayerState's color already matches incoming color, don't try transferring
            if (currentCombatColor.color.Key == newColor.color.Key)
            {
                isBeingGrantedNewColor = false;
                yield return null;
            }
            else
            {
                // Update incomingTransferColor so UI knows which icon to display
                setIncomingTransferColor(newColor);

                // Once colorTransferValue reaches above 1f, set PlayerState's color to the incoming color
                for (float transferVal = 0f; transferVal <= 1.1f; transferVal += Time.deltaTime/GameManager.colorTransferTimeStep)
                {
                    setColorTransferValue(transferVal);

                    if (transferVal > 1f)
                    {
                        // Do this in the for-loop to prevent the delay after yield return null
                        setCurrentCombatColor(newColor);
                        isBeingGrantedNewColor = false;
                    }

                    yield return null;
                }
            }

		}

        public void startGrantingColor(CombatColor newColor)
        {
            if (!isBeingGrantedNewColor)
            {
                grantingColorCoroutine = StartCoroutine(grantColor(newColor));
            }
        }

        public void stopGrantingColor()
        {
            setColorTransferValue(0f);
            isBeingGrantedNewColor = false;

            if (grantingColorCoroutine != null)
            {
                StopCoroutine(grantingColorCoroutine);
            }
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
                onStateChange();
                #endregion

            }

        }
    }
}
