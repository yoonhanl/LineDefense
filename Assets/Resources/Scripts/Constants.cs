//Variables and methods that can be used in other scripts.

using UnityEngine;
using System.Collections;

public static class Constants  {

    #region Variables


    #endregion


    #region Methods

    public static Vector2 GetXYDirection( float angle, float magnitude)
    {
        angle *= -1f;
        angle += 90f;
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * magnitude;
    }

    public static float GetAngleDirection( Vector2 point1, Vector2 point2)
    {
        Vector2 v = point1 - point2;
        
        return (float)Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg; 
    }
    #endregion
}
