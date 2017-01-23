using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

// 일반 팝업 창과 확인 팝업 창을 관리하는 DialogController***(DialogControllerAlert, DialogControllerConfirm) 의 부모 클래스입니다.
public class DialogController : MonoBehaviour
{
    public Color backgroundColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);

    private GameObject background;

    // 팝업 창의 Transform입니다.
    public Transform window;
	
	// 팝업 창이 보이는지 조회하거나, 보이지 않게 설정하는 변수입니다.
	public bool Visible 
	{
		get
		{
			return window.gameObject.activeSelf;
		}

		private set
		{
            //if(!value)
            //{
            //    RemoveBackground();
            //}
            
            window.gameObject.SetActive(value);
		}
	}

	public virtual void Awake()
	{
	}

	public virtual void Start()
	{

	}
	// 팝업이 화면에 나타날 때 OnEnter() 열거형(IEnumerator) 함수로 애니메이션을 구현할 수 있습니다.
	IEnumerator OnEnter(Action callback)
	{
        Visible = true;
        AddBackground();
        if ( callback != null ) {
			callback();
		}
		yield break;
	}

	// 팝업이 화면에서 사라질 때 OnEnter() 열거형(IEnumerator) 함수로 애니메이션을 구현할 수 있습니다.
	IEnumerator OnExit(Action callback)
	{
        Debug.Log(window.gameObject);
        
        Animator animator = window.GetComponent<Animator>();
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            animator.Play("Close");
        RemoveBackground();
        yield return new WaitForSeconds(0.5f);

        Visible = false;

		if( callback != null ) {
			callback();
		}
        yield break;
	}
    private void AddBackground()
    {
        if (background != null)
        {
            background.SetActive(true);
            return;
        }
        print("백그라운드 생성");
        var bgTex = new Texture2D(1, 1);
        bgTex.SetPixel(0, 0, backgroundColor);
        bgTex.Apply();

        background = new GameObject("PopupBackground");
        var image = background.AddComponent<Image>();
        var rect = new Rect(0, 0, bgTex.width, bgTex.height);
        var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
        image.material.mainTexture = bgTex;
        image.sprite = sprite;
        var newColor = image.color;
        image.color = newColor;
        image.canvasRenderer.SetAlpha(0.0f);
        image.CrossFadeAlpha(1.0f, 0.4f, false);

        var canvas = GameObject.Find("Canvas");
        background.transform.localScale = new Vector3(1, 1, 1);
        background.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
        background.transform.SetParent(canvas.transform, false);
        background.transform.SetSiblingIndex(transform.GetSiblingIndex());
        
    }

    public virtual void Build(DialogData data)
    {
        
    }
    

    private void RemoveBackground()
    {
        var image = background.GetComponent<Image>();
        if (image != null)
        {
            image.CrossFadeAlpha(0.0f, 0.2f, false);
            background.SetActive(false);
        }
    }
	// 팝업이 화면에 나타날 때 OnEnter() 열거형(IEnumerator) 함수로 애니메이션을 구현할 수 있습니다.
    public void Show(Action callback)
    {
		StartCoroutine ( OnEnter( callback ) );
    }

	// 팝업이 화면에서 사라질 때 OnEnter() 열거형(IEnumerator) 함수로 애니메이션을 구현할 수 있습니다.
    public void Close(Action callback)
    {
		StartCoroutine ( OnExit( callback ) );
    }
}
