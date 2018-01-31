using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
    /// <summary>
    /// Class containing helper methods to help Game Menus function
    /// </summary>
	public class MenuManager : MonoBehaviour
	{
        private Transform colorChooseMenu;

        void Awake()
        {
            getAllMenus();
            disableAllMenus();
        }

        void getAllMenus()
        {
            colorChooseMenu = GameObject.Find("ColorChoose_Menu").GetComponent<Transform>();
        }

        void disableAllMenus()
        {

        }

        void Update()
        {
            // Only display the menu while the Color_Choose button is being pressed
            // Be sure to reposition the Menu before displaying
            if (Input.GetButtonDown("Color_Switch"))
            {

            }

            // Make it so that the next color in the list is chosen as the currentCombatColor
            // And then hide the menu
            if (Input.GetButtonDown("Color_Switch"))
            {

            }
        }
	}
}
