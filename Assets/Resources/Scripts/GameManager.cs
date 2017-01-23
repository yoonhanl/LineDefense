/*
This Script manages the states of the game.
*/

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
//using Boomlagoon.JSON;

public class GameManager : MonoBehaviour {
    //정적 오브젝트
    static GameManager _instance;
    
    //싱글톤
    public static GameManager Instance
    {
        get
        {
            if(!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }
            return _instance;
        }
    }

    //쉴드 오브젝트
    private GameObject shield;

    //발사체 소환 클래스
    private ProjectileSpawner pSpawner;
    
    //화면 뿌옇게..쓰는거(안쓰는중..)
    private BlurControl bCtrl;

    //중앙 지구 오브젝트
    private GameObject earth;
    
    //베스트 스코어 컴포넌트
    private Text bestScore;
    

    //PAUSE 버튼 오브젝트들
    public Image PauseButtonIcon;
    public Sprite PauseIcon;
    public Sprite PlayIcon;

    //the explosion when the game ends
    //public string EndGameExplosion;

    //the audio that will play when the game ends
    public AudioClip EndGameAudio;

    //스코어 추가 이펙트용 오브젝트 풀
    private PoolManager poolManager;

    //그림 그리기 컴포넌트
    private DrawLine _drawManager;

    //audio variables to use
    //public AudioClip hitShieldAudio;

    
    //게임 상태 ENUM
    public enum GameStates
    {
        IDLE = 0,
        MAIN,
        STORE,
        PLAYING,
        PAUSE,
        GAMEOVER,
        RESUME,
    }

    //플레이어 업그레이드 ENUM
    public enum PlayerUpgrade
    {
        INCREASE_LINELENGTH=0,
        INCREASE_POWER_OF_FORCEFILED,
        INCREASE_SHIELD_SPEED,
        INCREASE_DRAW_LINE_COUNT,
    }

    //플레이어 업그레이드 정보 클래스
    public class UpgradeInfo
    {
        public string Name { get; set; }
        public int MaxLevel { get; set; }
        public int CurrentLevel { get; set; }
        public int InitValue { get; set; }
        public float DeltaValue { get; set; }
        public string Detail { get; set; }
        public UpgradeInfo(string name,int maxLevel, int currentLevel, int initValue, float deltaValue, string detail)
        {
            Name = name;
            MaxLevel = maxLevel;
            CurrentLevel = currentLevel;
            InitValue = initValue;
            DeltaValue = deltaValue;
            Detail = detail;
        }
    }

    //game state variables

    //게임상태가 변환될떄의 시간
    public float StateChangeTime; //time when it was last changed

    //게임 이전 상태
    public GameStates PrevState = GameStates.IDLE; 

    //게임 현재 상태
    public GameStates State = GameStates.IDLE; 

    //플레이어 업그레이드 정보 MAP(업그레이드 종류, 업그레이드 정보)
    public Dictionary<PlayerUpgrade, UpgradeInfo> _playerUpgradeInfo = new Dictionary<PlayerUpgrade, UpgradeInfo>();

    /// <summary>
    /// 해당 정보를 업그레이드한다.
    /// </summary>
    /// <param name="pu"></param>
    /// <returns></returns>
    public bool UpgradePlayer(PlayerUpgrade pu)
    {
        //현재 레벨이 맥스 레벨일 경우
        if(_playerUpgradeInfo[pu].CurrentLevel==_playerUpgradeInfo[pu].MaxLevel)
        {
            return false;
        }
        //업그레이드 가능한경우
        else
        {
            _playerUpgradeInfo[pu].CurrentLevel++;
            return true;
        }
    }
    /// <summary>
    /// 해당 플레이어 정보를 리턴
    /// </summary>
    /// <param name="pu">리턴할 플레이어 ENUM</param>
    /// <returns></returns>
    public float GetPlayerUpgradeInfo(PlayerUpgrade pu)
    {
        float val = 0.0f;
        switch(pu)
        {
            case PlayerUpgrade.INCREASE_SHIELD_SPEED:
            case PlayerUpgrade.INCREASE_DRAW_LINE_COUNT:
            case PlayerUpgrade.INCREASE_POWER_OF_FORCEFILED:
            case PlayerUpgrade.INCREASE_LINELENGTH:
                val = _playerUpgradeInfo[pu].InitValue +
                    ((_playerUpgradeInfo[pu].CurrentLevel - 1) * _playerUpgradeInfo[pu].DeltaValue);
                break;
            default:
                val = 0.0f;
                 break;
        }
        return val;
    }

    //로딩 오브젝트
    public GameObject _loadingObj;

    /// <summary>
    /// 스타트 함수 이전 및 프리팹의 인스턴스화 직후 호출(거의 처음 호출)
    /// Awake()->OnEnable()->Start()
    /// </summary>
    void Awake()
    {
        //maps the States to the methods
        shield = GameObject.FindObjectOfType<ShieldMovement>().gameObject;
        pSpawner = GameObject.FindObjectOfType<ProjectileSpawner>();
        bCtrl = GameObject.FindObjectOfType<BlurControl>();
        earth = GameObject.FindGameObjectWithTag("Cookie");
        poolManager = GameObject.FindObjectOfType<PoolManager>();
        _drawManager = GameObject.FindObjectOfType<DrawLine>();

        //처음상태는 MAIN
        SetState(GameStates.MAIN);

        //플레이어 업그레이드 정보를 .csv파일에서 로딩
        LoadPlayerUpgradeInfo();
    }

    /// <summary>
    /// 플레이어 업그레이드 로딩(나중엔 서버에서 받아야함)
    /// </summary>
    public void LoadPlayerUpgradeInfo()
    {
        //CSV파일 로딩
        _playerUpgradeInfo.Clear();

        List<Dictionary<string, object>> data = CSVReader.Read("Data/playerUpgrade");

        for (int i = 0; i < data.Count; i++)
        {
            UpgradeInfo info = new UpgradeInfo(data[i]["UPGRADE_NAME"].ToString() ,Convert.ToInt32(data[i]["MAX_LEVEL"]), Convert.ToInt32(data[i]["CURRENT_LEVEL"]),
                Convert.ToInt32(data[i]["INIT_VALUE"]), Convert.ToSingle(data[i]["DELTA_VALUE"]), data[i]["DETAIL"].ToString());

            _playerUpgradeInfo.Add((PlayerUpgrade)(Convert.ToInt32(data[i]["PLAYER_UPGRADE"])), info);
        }
    }

    /// <summary>
    /// 게임 상태를 설정한다
    /// </summary>
    /// <param name="NextState">바꿀 게임상태</param>
    public void SetState(GameStates NextState)
    {
        //바뀌는 시간을 저장
        StateChangeTime = Time.time;

        //이전상태를 저장
        PrevState = State;

        //새로운 상태로 변경
        State =  NextState;

        //메인 메뉴 상태일떄
        if (State == GameStates.MAIN)
        {
            //메인 메뉴를 SHOW
            InterfaceManager.Instance.ShowGameMenu();
        }

        if(State==GameStates.STORE)
        {
            //상점 메뉴를 SHOW
            InterfaceManager.Instance.ShowGameStore();
        }

        //플레잉 상태일때
        if (State == GameStates.PLAYING)
        {
            //쉴드를 Active
            shield.SetActive(true);

            //발사체 소환 Active
            pSpawner.enabled = true;
            //pSpawner.gameObject.SetActive(true);

            //발사체 카운트
            pSpawner.fireCount = 0;

            //게임 메뉴를 SHOW
            InterfaceManager.Instance.ShowGameStart();

            //중앙지구를 Active           
            earth.SetActive(true);

            //드로우 매니저 Active
            _drawManager.enabled = true;
        }
        else//게임 플레이 상태가 아닐경우
        {
            //쉴드 Deactive
            shield.SetActive(false);

            //발사체 소환 Deactive
            pSpawner.enabled = false;
            
            //지구를 Deactive
            earth.SetActive(false);

            //드로우 매니저 Deactive
            _drawManager.enabled = false;

        }

        //게임 오버상태일 경우
        if (State == GameStates.GAMEOVER)
        {
            //현재 발사체를 재활용한다.
            RecycleProjectiles();

            //모든 라인 초기화
            _drawManager.ClearAllLine();

            //스코어를 서버로 전송
            SendScore();
        }

        //IDLE 상태일경우(현재 사용안함)
        if (State == GameStates.IDLE)
        {
            earth.SetActive(true);
            pSpawner.enabled = false;
        }

        //일시정지 상태일 경우
        if (State == GameStates.PAUSE)
        {
            //일시정지 메뉴를 SHOW
            InterfaceManager.Instance.ShowGamePause();

            //StartTime 코루틴 중지
            StopCoroutine("StartTime");

            //타임 스케일을 0
            Time.timeScale = 0f;
        }

        //게임재개 상태일 경우
        if(State==GameStates.RESUME)
        {
            //게임메뉴를 SHOW
            InterfaceManager.Instance.ShowGameStart();

            //플레이 상태로 변경
            SetState(GameStates.PLAYING);

            //STARTTIME 코루틴을 중지
            StopCoroutine("StartTime");

            //STARTTIME 코루틴을 시작
            StartCoroutine("StartTime");
        }
        /*else
        {
            bCtrl.blur = false;
        }*/
    }


    /*
    //this method will be executed during Death
    private void Death()
    {
        if (Time.time - StateChangeTime >= 3f)
        {
            SetState(GameStates.IDLE);
        }

    }
    */

    /// <summary>
    /// PAUSE 버튼 클릭 이벤트
    /// </summary>
    public void PauseButton()
    {
        if (State == GameStates.PAUSE)
        {
            PauseButtonIcon.sprite = PauseIcon;
            SetState(GameStates.PLAYING);
            StopCoroutine("StartTime");
            StartCoroutine("StartTime");
        }
        else
        {
            PauseButtonIcon.sprite = PlayIcon;
            SetState(GameStates.PAUSE);
            StopCoroutine("StartTime");
            Time.timeScale = 0f;
        }
    }

    /// <summary>
    /// 모든 발사체 재활용
    /// </summary>
    private void RecycleProjectiles()
    {
        Projectile[] Projectiles = GameObject.FindObjectsOfType<Projectile>();

        for(int i = 0; i < Projectiles.Length; i++)
        {
            Projectiles[i].Recycle();
        }
    }

    /// <summary>
    /// 리스타트 시간을 느리게한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartTime()
    {
        while (Time.timeScale < 1f )
        {
            Time.timeScale += 0.01f;

            //다음 Update구문의 수행이 완료될떄까지 대기
            yield return null;
        }

        Time.timeScale = 1f;
    }

    public void Start()
    {
       
        
    }

    /// <summary>
    /// 서버로 스코어를 전송한다.
    /// </summary>
    public void SendScore()
    {
        Person person = new global::Person(/*SystemInfo.deviceUniqueIdentifier,"yhn","100"*/);
        person.uuid = SystemInfo.deviceUniqueIdentifier;
        person.name = "yhn";
        person.score = "100";

        string jsonData = LitJson.JsonMapper.ToJson(person);
        
        HTTPClient.Instance.POST("localhost:3000/new", jsonData, delegate (WWW www)
        {
            if (www.error != null)
            {
                DialogDataConfirm confirm = new DialogDataConfirm("접속 오류", "네트워크를 확인해 주세요.", delegate (bool yn)
                 {
                     //yes 버튼을 누른 경우
                     if(yn)
                     {
                         SendScore();
                     }
                     //no 버튼을 누른경우
                     else
                     {
                         Application.Quit();
                     }
                 });
                DialogManager.Instance.Push(confirm);
            }
            else //정상 연결
            {
                print("Received:" + www.text);
                InterfaceManager.Instance.ShowGameOver();
                InterfaceManager.Instance.ShowRankingItem(www.text);
            }
            
        });
    }

    /// <summary>
    /// 스코어가 추가되었을때 이펙트 실행
    /// </summary>
    /// <param name="position"></param>
    public void AddScore(Vector2 position)
    {
        /*
        float angle = Constants.GetAngleDirection(earth.transform.position, position);
        score.addScore();

        GameObject AddScore = poolManager.Spawn("AddScore");
        AddScore.transform.position = position;

        Vector2 V2 = Constants.GetXYDirection(angle * -1f + 180f + UnityEngine.Random.Range(-30f, 30f), UnityEngine.Random.Range(-100f, -150f));
        AddScore.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        AddScore.GetComponent<Rigidbody2D>().AddForce(V2);
        */
    }
}
