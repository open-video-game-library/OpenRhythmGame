namespace HolmonUtility
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class Vector2Ext
    {
        /// <summary>
        /// Vector2.Lerp��t�͈̔͂��g����������
        /// </summary>
        /// <param name="a">�n�_</param>
        /// <param name="b">�I�_</param>
        /// <param name="t">����</param>
        /// <returns></returns>
        public static Vector2 OverLerp(Vector2 a, Vector2 b, float t)
        {
            float resultX = a.x + (b.x - a.x) * t;
            float resultY = a.y + (b.y - a.y) * t;
            return new Vector2(resultX, resultY);
        }
    }
}
