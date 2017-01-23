using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class DrawLine : MonoBehaviour {
    public class LineStruct
    {
        public LineStruct(VectorLine _line)
        {
            line = _line;
        }
        public bool bUse;
        public VectorLine line;
    }

    public Texture lineTex;
    public int maxPoints = 5000;
    public float lineWidth = 4.0f;
    public int minPixelMove = 5;
    public bool useEndCap = false;
    public Texture2D capLineTex;
    public Texture2D capTex;
    public float capLineWidth = 20.0f;
    /*private VectorLine line;
    private List<LineStruct> lines;*/
    public int _lineMaxCount = 5;
    

    public Queue<LineStruct> lineQueue;
    public VectorLine currentLine;

    Vector2 previousPosition;
    int sqrMinPixelmove;
    bool canDraw = false;

    public int _maxLength;
    //a pool manager is used to make this game more mobile friendly 

    //a reference to the score script
    //private Score score;

    //audio variables to use
    private AudioSource audioSource;
    public AudioClip hitShieldAudio;

    //the angle we will be rotating to
    private float Angle;
    // Use this for initialization
    void Start () {
        LineSetting();
        sqrMinPixelmove = minPixelMove * minPixelMove;
    }
    public void LineSetting()
    {
        Texture tex;
        float useLineWidth;

        if (useEndCap)
        {
            VectorLine.SetEndCap("RoundCap", EndCap.Mirror, capLineTex, capTex);
            tex = capLineTex;
            useLineWidth = capLineWidth;
        }
        else
        {
            tex = lineTex;
            useLineWidth = lineWidth;
        }

        //라인풀 생성
        if(lineQueue!=null)
        {
            for (int i = 0; i < lineQueue.Count; i++)
            {
                LineStruct ls = lineQueue.Dequeue();
                VectorLine.Destroy(ref ls.line);
            }
            lineQueue.Clear();
        }
        
        int lineMaxCount = (int)GameManager.Instance.GetPlayerUpgradeInfo(GameManager.PlayerUpgrade.INCREASE_DRAW_LINE_COUNT);
        lineQueue = new Queue<LineStruct>(lineMaxCount);

        for (int i = 0; i < lineMaxCount; i++)
        {
            VectorLine localLine = new VectorLine("DrawnLine", new List<Vector2>(), tex, useLineWidth, LineType.Continuous, Joins.Weld);
            localLine.endPointsUpdate = 1;
            localLine.rectTransform.tag = "Line";
            localLine.rectTransform.name = "Line" + i.ToString();
            LineStruct ls = new LineStruct(localLine);
            
            ls.bUse = false;
            localLine.collider = true;
            lineQueue.Enqueue(ls);
        }

        //최대 선의 길이를 설정한다.
        _maxLength = (int)GameManager.Instance.GetPlayerUpgradeInfo(GameManager.PlayerUpgrade.INCREASE_LINELENGTH);
    }
    
    void OnEnable()
    {
        InputManager.OnPress += OnPress;
        InputManager.OnPressing += OnPressing;
    }
    void OnDisable()
    {
        InputManager.OnPress -= OnPress;
        InputManager.OnPressing -= OnPressing;
    }
    
    /// <summary>
    /// 모든 라인을 초기화한다.
    /// </summary>
    public void ClearAllLine()
    {
        foreach (LineStruct obj in lineQueue)
        {
            obj.bUse = false;
            obj.line.points2.Clear();
            obj.line.Draw();
        }
    }
    /// <summary>
    /// 안쓰는 라인을 반환
    /// </summary>
    /// <returns></returns>
    public VectorLine GetNotUseLine()
    {
        LineStruct ls = null;
        
        foreach(LineStruct obj in lineQueue)
        {
            if (obj.bUse == false)
            {
                ls = obj;
                ls.bUse = true;
                break;
            }
        }

        //모두 사용하고 있으면 처음그린 선을 뺀다.
        if (ls==null)
        {
            ls = lineQueue.Dequeue();
            lineQueue.Enqueue(ls);
            ls.bUse = true;
        }

        return ls.line;
    }

    /// <summary>
    /// 마우스 클릭 이벤트
    /// </summary>
    /// <param name="pos">마우스 위치</param>
    public void OnPress(Vector3 pos)
    {
        currentLine = GetNotUseLine();
        currentLine.points2.Clear();
        currentLine.Draw();
        previousPosition = Input.mousePosition;
        currentLine.points2.Add(pos);
        canDraw = true;
    }

    /// <summary>
    /// 마우스 클릭중 이벤트
    /// </summary>
    /// <param name="pos">마우스 위치</param>
    public void OnPressing(Vector3 pos)
    {
        if (((Vector2)pos - previousPosition).sqrMagnitude > sqrMinPixelmove && canDraw)
        {
            previousPosition = pos;
            currentLine.points2.Add(pos);

            if(GetLineLength(currentLine.points2) > _maxLength)
            {
                canDraw = false;
                return;
            }
            currentLine.Draw();
            
            int pointCount = currentLine.points2.Count;
            if (pointCount >= maxPoints)
            {
                canDraw = false;
            }
        }
    }
   
    public float GetLineLength(List<Vector2> list)
    {
        float totalLength = 0;

        for (int i = 1; i < list.Count; i++)
        {
            totalLength += Vector2.Distance(list[i], list[i - 1]);
        }

        return totalLength;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            LineSetting();
        }
    }

    private void AddLineCollider()
    {
        currentLine.Draw();
    }
    private Vector3 GetMousePos()
    {
        Vector3 p = Input.mousePosition;
        return p;
    }
}
