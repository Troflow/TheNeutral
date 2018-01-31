using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neutral
{
    /// <summary>
    /// Class containing helper methods to for In-Game/Navigation Menus function
    /// </summary>
	public class MenuManager : MonoBehaviour
	{
        private PlayerState playerState;
        private Transform colorChooseMenu;

        void Awake()
        {
            playerState = GameObject.Find("Player").GetComponent<PlayerState>();
            getAllMenus();
            disableAllMenus();
        }

        #region COLOR CHOOSE Menu
        void handleColorChooseMenuDisplay()
        {
            colorChooseMenu.gameObject.SetActive(Input.GetButton("Color_Switch"));
            if (Input.GetButtonDown("Color_Switch"))
            {
                colorChooseMenu.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            }
        }

        public void chooseColorRed()
        {
            playerState.setCurrentCombatColor(new CombatColorRed());
        }

        public void chooseColorGreen()
        {
            playerState.setCurrentCombatColor(new CombatColorGreen());
        }

        public void chooseColorBlue()
        {
            playerState.setCurrentCombatColor(new CombatColorBlue());
        }
        #endregion

        void getAllMenus()
        {
            colorChooseMenu = GameObject.Find("ColorChoose_Menu").GetComponent<RectTransform>();
        }

        void disableAllMenus()
        {
            colorChooseMenu.gameObject.SetActive(false);
        }

        void Update()
        {
            handleColorChooseMenuDisplay();
        }
	}
}
