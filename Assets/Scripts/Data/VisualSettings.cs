using System.Collections;
using System.Collections.Generic;
using VirtualLab.Gameplay;
using UnityEngine;

namespace VirtualLab.Data
{
    [CreateAssetMenu(menuName = "Data/VisualSettings")]
    public class VisualSettings : ScriptableObject
    {
        [Header("Colors")]
        public Color unselectedColor;
        [Tooltip("In order Blue, Red, Green, Yellow")]
        public Color[] colorsForTypes;

        public Color GetColorForColorType(ColorType colorType)
        {
            return colorsForTypes[(int)colorType];
        }
    }
}
