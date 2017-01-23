//This Script manages the projectiles


using UnityEngine;
using System.Collections;
using Vectrosity;
using System.Collections.Generic;

public class Projectile : MonoBehaviour {

    //this ridgidbody
    private Rigidbody2D RB; 

    //the explosion it makes
    public string explosion;

    //a pool manager is used to make this game more mobile friendly 
    private PoolManager poolManager;

    //발사체 궤적
    VectorLine _projectileLine;

    //패스의 텍스쳐
    public Texture _lineTex;

    //패스draw 여부
    public bool _continuousUpdate;

    //set variables
    void Start()
    {
        poolManager = GameObject.FindObjectOfType<PoolManager>();
        
    }

    /// <summary>
    /// 라인을 초기화하고 코루틴을 호출한다.
    /// </summary>
    public void BeginPathCoroutine()
    {
        if(_projectileLine==null)
        {
            _projectileLine = new VectorLine("Path", new List<Vector2>(), _lineTex, 18.0f, LineType.Continuous);
            _projectileLine.textureScale = 4f;
            _projectileLine.color = Color.black;
        }
        StartCoroutine(DrawPath(this.transform));
    }

    /// <summary>
    /// 발사체를 발사한다.
    /// </summary>
    /// <param name="speed">발사체 스피드</param>
    public void Launch(float speed = 1f)
    {
        GameObject earth = GameObject.Find("Cookie");
        float angle = Constants.GetAngleDirection(gameObject.transform.position,earth.transform.position) * -1f;
        gameObject.transform.rotation = Quaternion.Euler(0f,0f, angle);

        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponent<Rigidbody2D>().velocity  = transform.up * speed * -1f;
    }

    /// <summary>
    /// 발사체의 궤적을 그린다.
    /// </summary>
    /// <param name="thisTransform">궤적을 그릴 transform</param>
    /// <returns></returns>
    public IEnumerator DrawPath(Transform thisTransform)
    {
        bool running = true;
        while(running)
        {
            _projectileLine.points2.Add(Camera.main.WorldToScreenPoint(thisTransform.position));
            yield return new WaitForSeconds(0.1f);

            if (_continuousUpdate)
            {
                _projectileLine.Draw();
            }
        }
    }

    void Update()
    {
    }
    //recycle the projectile ...this is easier on mobile devices
    public void Recycle()
    {
        StopAllCoroutines();
        _projectileLine.points2.Clear();
        _projectileLine.Draw();
        poolManager.Recycle(gameObject.name.Replace("(Clone)",""),gameObject);
    }

    //on collision
    void OnCollisionEnter2D(Collision2D coll)
    {
        
        //print(LayerMask.LayerToName(coll.collider.gameObject.layer) + " : " + LayerMask.LayerToName(gameObject.layer));
        //make explosion
        GameObject thisExplosion = poolManager.Spawn(explosion);
        thisExplosion.transform.position = gameObject.transform.position;

        //패스 삭제
        
        
        //StartCoroutine(DrawPath(this.transform));

        //make shockwave
        //ShockWave.Get().StartIt(gameObject.transform.position,0.5f,7.5f,0.125f);

        //recycle
        Recycle();
        //poolManager.Recycle(gameObject.name.Replace("(Clone)",""),gameObject);

       //if earth ...end game
        if (coll.gameObject.tag == "Cookie")
        {
            GameManager.Instance.SetState(GameManager.GameStates.GAMEOVER);
        }
        else if(coll.gameObject.tag=="Line")
        {
            GameManager.Instance.AddScore(coll.contacts[0].point);
        }
    }
}
