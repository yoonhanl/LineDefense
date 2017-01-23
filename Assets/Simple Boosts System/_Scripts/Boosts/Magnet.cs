using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Magnet : Boosts {

    //The boost's "Kill zone"
    public float distance = 25;
    public Text TimerText;
    //The speed of the magnetized objects (in our case it is coins)
    public float returnSpeed = 5.5f;

    void Start()
    {
        Init();
    }

    public override void BeforeAction()
    {
        if (Particle != null && ParicleTransform != null)
        {
            InstantiateParticle();
        }

        Debug.Log("Started");
    }

    public override void InAction()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");

        if (coins != null)
        {
            foreach (GameObject coinsTrans in coins)
            {
                if (Vector2.Distance(Player.transform.position, coinsTrans.transform.position) < 25)
                {
                    coinsTrans.transform.position = Vector3.Lerp(coinsTrans.transform.position, Player.transform.position, returnSpeed * Time.deltaTime);
                }
                
            }
        }
        else 
        {
            Debug.Log("Null");
        }

        TimeBar();
    }

    public override void AfterAction()
    {
        DestroyParticle();
        TimerText.text = "";
        Debug.Log("Ended");
    }

    public override void TimeBar()
    {
        TimerText.text = ((int)CurentTime).ToString();
    }

}