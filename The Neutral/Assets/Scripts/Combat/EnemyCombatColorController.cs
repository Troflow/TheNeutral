using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Neutral
{

    //think about adding a base class for enemies with a type which will help in the following ways:
    // set their exposed colors
    // set their palette
    // easier to add new combat enemies

    // then we can leave this class for just purely the color logic

    public class EnemyCombatColorController : MonoBehaviour
    {
        [SerializeField]
        private Color exposedColor;
        private List<CombatColor> palette;
        private CombatColor currentEnemyCombatColor;
        public MeshRenderer colorBookRenderer;
        private List<CombatColor> colorBook;

        // Use this for initialization
        void Start()
        {
            palette = CombatColorHelper.IntializePalette(EnemyType.Mojo);
            currentEnemyCombatColor = palette[Random.Range(0, palette.Count-1)];
            exposedColor = Color.green;
            colorBook = new List<CombatColor>()
            {
                currentEnemyCombatColor
            };

            foreach (Transform child in transform)
            {
                if (child.CompareTag("Flag"))
                {
                    colorBookRenderer = child.GetComponent<MeshRenderer>();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            colorBookRenderer.material.color = currentEnemyCombatColor.color.Value;
        }


        private CombatColor computeColoringBookColor()
        {
            //if (colorBook.Count <= 0) return new CombatColorMixed(Color.black);
            var mixedColor = colorBook[0];
            for (int x = 1; x < colorBook.Count; x++)
            {
                mixedColor += colorBook[x];
            }

            return mixedColor;
        }

        public List<CombatColor> getColorBook()
        {
            return colorBook;
        }

        public void addColorToColorBook(CombatColor color)
        {
            //print("adding color to enemy colorBook: " + color);
            colorBook.Add(color);
            currentEnemyCombatColor = computeColoringBookColor();
            if (currentEnemyCombatColor.color.Value == exposedColor)
            {
                currentEnemyCombatColor.ExposedColorLogic();
            }
        }
    }

}