/*
This Script blurs the screen while on Pause, and removes the blur during play. 
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlurControl : MonoBehaviour {

    #region Variables

    //the maxBlur
    public float maxBlur = 5f;
    //the speed of the blur
    public float blurSpeed = 0.5f;

    //material used to blur the screen
    private Material blurMaterial;

    //the amount of blur
    private float _blurSize = 0f;
    public float blurSize
    {
        get{return _blurSize;}
        set{
            _blurSize = value;
            blurMaterial.SetFloat("_Size",_blurSize);
        }
    }

    //whether or not the screen is blured
    private bool _blur = false;
    public bool blur
    {
        get{return _blur;}
        set{
            _blur = value;

            if (_blur)
            {
                StartCoroutine("OnBlur");
            }
            else
            {
                StartCoroutine("OffBlur");
            }
        }
    }

    #endregion


    #region Methods

    // Use this before initialization
    void Awake()
    {
        blurMaterial = GetComponent<Image>().material;
    }


    //using a Coroutine allows us to blur and unblur without having to worry about the Time.timeScale.
    private IEnumerator OnBlur()
    {
        while (blurSize < maxBlur )
        {
            blurSize += blurSpeed;

            yield return null;
        }

        blurSize = maxBlur;
    }

    //using a Coroutine allows us to blur and unblur without having to worry about the Time.timeScale.
    private IEnumerator OffBlur()
    {
        while (blurSize > 0f )
        {
            blurSize -= blurSpeed * 4f; //unblur 4x faster

            yield return null;
        }

        blurSize = 0f;
    }

    #endregion
}
