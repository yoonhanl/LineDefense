using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    public GameObject[] upgradeButton;
    public Text detailText;
    public Image currentSelectImage;

    private int _currentIndex;
    //public GameObject innerBackground;
    void Awake()
    {

    }


    // Use this for initialization
    void Start()
    {
        /*
        for (int i = 0; i < upgradeButton.Length; i++)
        {
            //토글 이벤트 설정
            Toggle toggle = upgradeButton[i].GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(delegate
            {
                if (toggle.isOn) 
                {
                    SelectItem(upgradeButton[i]);
                }
            });
        }
        */

        foreach (GameObject obj in upgradeButton)
        {
            //토글 이벤트 설정
            Toggle toggle = obj.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(delegate
            {
                if (toggle.isOn)
                {
                    SelectItem(obj);
                }
            });
        }
        SelectItem(upgradeButton[0]);

        SetUpgradeLevel();
    }

    void SetUpgradeLevel()
    {
        for (int i = 0; i < upgradeButton.Length; i++)
        {
            //키가 존재하면..
            if (GameManager.Instance._playerUpgradeInfo.ContainsKey((GameManager.PlayerUpgrade)i))
            {
                print(GameManager.Instance._playerUpgradeInfo[(GameManager.PlayerUpgrade)i].Name);
                upgradeButton[i].GetComponentInChildren<Text>().text = "(Level:" + GameManager.Instance._playerUpgradeInfo[(GameManager.PlayerUpgrade)i].CurrentLevel.ToString() + ")";
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 선택
    /// </summary>
    /// <param name="obj"></param>
    public void SelectItem(GameObject obj)
    {
        int index = 0;

        foreach (GameObject ob in upgradeButton)
        {
            ob.GetComponent<Outline>().enabled = false;

            if (ob.GetInstanceID() == obj.GetInstanceID())
            {
                _currentIndex = index;
            }
            index++;
        }

        obj.GetComponent<Outline>().enabled = true;

        currentSelectImage.sprite = obj.GetComponent<Image>().sprite;
        currentSelectImage.color = obj.GetComponent<Image>().color;

        detailText.text = GameManager.Instance._playerUpgradeInfo[(GameManager.PlayerUpgrade)_currentIndex].Detail;
    }

    public void OnUpgradeButton()
    {
        print("OnUpgrade");
        string name = GameManager.Instance._playerUpgradeInfo[(GameManager.PlayerUpgrade)_currentIndex].Name;
        string context = string.Format("{0}를 업그레이드 하시겠습니까?",name);
        DialogDataConfirm confirm = new DialogDataConfirm("업그레이드", context, delegate (bool yn)
        {
            //예스인경우
            if (yn)
            {
                if (GameManager.Instance.UpgradePlayer((GameManager.PlayerUpgrade)_currentIndex))
                {
                    SetUpgradeLevel();
                }
                //맥스 레벨일 경우 강화가 취소
                else
                {
                    DialogDataAlert alert = new DialogDataAlert("업그레이드", "이미 맥스레벨입니다.", delegate ()
                     {

                     });
                    DialogManager.Instance.Push(alert);
                }
            }
            else
            {

            }
        });
        DialogManager.Instance.Push(confirm);
    }
}
