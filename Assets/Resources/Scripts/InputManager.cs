using UnityEngine;
using System.Collections;


/// <summary>
/// Class in charge of the input on different platforms.
/// </summary>
public class InputManager : MonoBehaviour
{
    public delegate void InputAction(Vector3 pos);
    public static event InputAction OnPress;
    public static event InputAction OnPressing;

    public delegate void InputAction2();
    public static event InputAction2 OnUnPress;
    void Update()
    {
        //Press
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            if(GameManager.Instance.State == GameManager.GameStates.PLAYING && 
                LayerMask.LayerToName(hit.collider.gameObject.layer)=="Background")
            {
                if (OnPress != null)
                    OnPress(Input.mousePosition);
            }
        }

        //UnPress
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            if (GameManager.Instance.State == GameManager.GameStates.PLAYING && 
                LayerMask.LayerToName(hit.collider.gameObject.layer) == "Background")
            {
                if (OnUnPress != null)
                    OnUnPress();
            }
        }
        else if(Input.GetMouseButton(0))
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            if (GameManager.Instance.State==GameManager.GameStates.PLAYING && 
                LayerMask.LayerToName(hit.collider.gameObject.layer) == "Background")
            {
                if (OnPressing != null)
                    OnPressing(Input.mousePosition);
            }
        }
        //Escaping
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //GameManager.Instance.Escape();
        }

    }
}
