using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using LitJson;
using System;

[Serializable]
public class Person
{
    public string uuid;
    public string name;
    public string score;
    public string date;
    public string nRank;
    /*public Person(string uuid,string name,string score)
    {
        this.uuid = uuid;
        this.name = name;
        this.score = score;
    }*/
}

/// <summary>
/// JSON Array Helper 클래스
/// </summary>
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        json = "{\"Items\":" + json + "}";
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return UnityEngine.JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}



public class InterfaceManager : MonoBehaviour {
   
    //메뉴 인터페이스, 상점인터페이스, 게임인터페이스, 일시정지인터페이스, 게임종료 인터페이스
    public GameObject MenuInterface, StoreInterface, GameInterface, PauseInterface, GameEndInterface;

    //스코어 Object
    public Text _score;
    public string ScoreText
    {
        get
        {
            string val = null;
            if(_score)
            {
                val = _score.text;
            }
            return val;
        }
        set
        {
            if(_score)
                _score.text = value;
        }
    }

    /// <summary>
    /// 랭킹데이터
    /// </summary>
    public Person[] arrayPerson;

    //랭킹리스트 컴포넌트    
    public RankingList _rankingListScript;

    /// <summary>
    /// 싱글톤 인스턴스
    /// </summary>
    private static InterfaceManager _instance;
    public static InterfaceManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(InterfaceManager)) as InterfaceManager;
            }
            return _instance;
        }
    }

    void Awake()
    {
        
    }

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	   if(Input.GetKeyDown(KeyCode.Space))
        {
            
        }
	}

    #region Show Game Interface
    /// <summary>
    /// 게임메뉴를 SHOW
    /// </summary>
    public void ShowGameMenu()
    {
        MenuInterface.SetActive(true);
        StoreInterface.SetActive(false);
        GameInterface.SetActive(false);
        PauseInterface.SetActive(false);
        GameEndInterface.SetActive(false);
    }
    /// <summary>
    /// 상점메뉴를 SHOW
    /// </summary>
    public void ShowGameStore()
    {
        MenuInterface.SetActive(false);
        StoreInterface.SetActive(true);
        GameInterface.SetActive(false);
        PauseInterface.SetActive(false);
        GameEndInterface.SetActive(false);
    }

    /// <summary>
    /// 게임메뉴를 SHOW
    /// </summary>
    public void ShowGameStart()
    {
        MenuInterface.SetActive(false);
        StoreInterface.SetActive(false);
        GameInterface.SetActive(true);
        PauseInterface.SetActive(false);
        GameEndInterface.SetActive(false);
    }

    /// <summary>
    /// 일시정지메뉴를 SHOW
    /// </summary>
    public void ShowGamePause()
    {
        MenuInterface.SetActive(false);
        StoreInterface.SetActive(false);
        GameInterface.SetActive(false);
        PauseInterface.SetActive(true);
        GameEndInterface.SetActive(false);
    }

    /// <summary>
    /// 게임오버메뉴를 SHOW
    /// </summary>
    public void ShowGameOver()
    {
        MenuInterface.SetActive(false);
        StoreInterface.SetActive(false);
        GameInterface.SetActive(false);
        PauseInterface.SetActive(false);
        GameEndInterface.SetActive(true);
    }

    /// <summary>
    /// 랭킹오브젝트를 SHOW
    /// </summary>
    /// <param name="jsonData">서버에서받아온 jsonData</param>
    public void ShowRankingItem(string jsonData)
    {
        print(jsonData);

        //JSON형태로 파싱
        arrayPerson = JsonHelper.FromJson<Person>(jsonData);

        _rankingListScript.ListInitialize(arrayPerson.Length);
    }
    #endregion

    #region ButtonEvent

    /// <summary>
    /// 플레이버튼 이벤트
    /// </summary>
    public void ButtonPlay()
    {
        GameManager.Instance.SetState(GameManager.GameStates.STORE);
    }

    /// <summary>
    /// 게임시작 버튼 이벤트
    /// </summary>
    public void ButtonGameStart()
    {
        GameManager.Instance.SetState(GameManager.GameStates.PLAYING);
    }

    /// <summary>
    /// 일시정지 버튼 이벤트
    /// </summary>
    public void ButtonPauseGame()
    {
        GameManager.Instance.SetState(GameManager.GameStates.PAUSE);
    }

    /// <summary>
    /// 게임재개 버튼 이벤트
    /// </summary>
    public void ButtonResumeGame()
    {
        GameManager.Instance.SetState(GameManager.GameStates.RESUME);
    }
    #endregion
}
