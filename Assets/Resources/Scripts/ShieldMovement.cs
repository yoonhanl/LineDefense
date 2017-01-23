//rotates the shield...to align to the touch point

using UnityEngine;
using System.Collections;

public class ShieldMovement : MonoBehaviour 
{
    #region Variables

    //the movment speed
    public float speed = 1f;

    //the touchpoint converted to worldpoint
    private Vector3 TouchWorldPoint;

    //the angle we will be rotating to
    private float Angle;

    //the vector3 we will be rotating to
    private Vector3 TargetRotation;

    //a pool manager is used to make this game more mobile friendly 
    private PoolManager poolManager;

    //a reference to the score script
    //private Score score;

    //audio variables to use
    private AudioSource audioSource;
    public AudioClip hitShieldAudio;

    //reference to the camera
    private Camera camera;

    private GameObject _cookie;

    private TrailRenderer _trail;
    #endregion


    #region Methods

   
    // Use this for initialization
    void Start()
    {
        //score = GameObject.FindObjectOfType<Score>();
        poolManager = GameObject.FindObjectOfType<PoolManager>();
        //audioSource =  GameObject.Find("Audio").GetComponent<AudioSource>();
        _cookie = GameObject.FindGameObjectWithTag("Cookie");
        _trail = GameObject.FindObjectOfType<TrailRenderer>();
        _trail.sortingOrder = -1;
        camera =  Camera.main;
    }
    private void OnEnable()
    {
        speed = GameManager.Instance.GetPlayerUpgradeInfo(GameManager.PlayerUpgrade.INCREASE_SHIELD_SPEED);
    }
    // Update is called once per frame
    void Update()
    {
        /*TouchWorldPoint = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,10f));
        Angle = 180 - Constants.GetAngleDirection(gameObject.transform.position,TouchWorldPoint);
        TargetRotation =  new Vector3(0f,0f, Angle);*/
        
        gameObject.transform.RotateAround(_cookie.transform.position, Vector3.forward, speed);
    }

    //Update is called once per render
    void FixedUpdate()
    {
        //gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(TargetRotation),Time.deltaTime * speed);
        

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Projectile")
        {
            /*
            //score.addScore();

            GameObject AddScore = poolManager.Spawn("AddScore");
            AddScore.transform.position = coll.contacts[0].point;

            Vector2 V2 = Constants.GetXYDirection(Angle * -1f + 180f  + Random.Range(-30f,30f), Random.Range(-100f,-150f));
            AddScore.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
            AddScore.GetComponent<Rigidbody2D>().AddForce(V2);

            audioSource.PlayOneShot(hitShieldAudio);
            */
        }
    }

    #endregion

}
