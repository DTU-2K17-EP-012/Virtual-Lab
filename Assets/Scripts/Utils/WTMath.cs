using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WTUtils
{
    public static class WTMath
    {
        public static bool IsBetween(this float value, float border1, float border2)
        {
            return (value > border1 && value < border2) || (value > border2 && value < border1);
        }
        public static bool IsBetweenOrEqual(this float value, float border1, float border2)
        {
            return (value >= border1 && value <= border2) || (value >= border2 && value <= border1);
        }
        public static bool IsBetween(this int value, int border1, int border2)
        {
            return (value > border1 && value < border2) || (value > border2 && value < border1);
        }
        public static bool IsBetweenOrEqual(this int value, int border1, int border2)
        {
            return (value >= border1 && value <= border2) || (value >= border2 && value <= border1);
        }
    }
}

