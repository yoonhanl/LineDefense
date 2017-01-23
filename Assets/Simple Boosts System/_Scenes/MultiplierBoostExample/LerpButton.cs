using UnityEngine;
using System.Collections;

public class LerpButton : MonoBehaviour {

    Vector3 lerpOldPos;

    void Start() 
    {
        lerpOldPos = transform.localScale;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.localScale = lerpOldPos / 1.2f;
        }
        if (Input.GetMouseButtonUp(0))
        {
            transform.localScale = lerpOldPos;
        }
    }

    void OnMouseExit() 
    {
        transform.localScale = lerpOldPos;
    }

}