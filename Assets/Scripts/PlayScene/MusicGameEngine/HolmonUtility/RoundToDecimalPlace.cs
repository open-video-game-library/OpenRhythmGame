using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HolmonUtility
{
    public static class RoundToDecimalPlace
    {
        public static double Round(double value, int decimalPlace)
        {
            double pow = Mathf.Pow(10, decimalPlace);
            return Mathf.Round((float)(value * pow)) / pow;
        }
    }
}
