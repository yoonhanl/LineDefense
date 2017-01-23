using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Multiplier : Boosts {

    // The multiplication coefficient
    public int MultiplyByNTime = 2;

    // Initial amount of points for each click
    private int InitialEachClick;

    void Start()
    {
        Init();
        
        InitialEachClick = Click.eachClick;
    }

    public override void BeforeAction()
    {
        Debug.Log("Started");
        InstantiateIndicator();
        Click.eachClick *= MultiplyByNTime;
    }

    public override void InstantiateIndicator()
    {
        FillAmount = IndicatorGO.transform.FindChild("FillAmount").GetComponent<Image>();
    }



    public override void InAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                Vector3 mousePos = hit.point;

                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Cookie")
                    {
                        //Gives 1 point for each click
                        InstantiateParticleInPos(hit.point);               
                    }
                }
            }
        }

        TimeBar();
        
    }

    public override void AfterAction()
    {
        gameObject.SetActive(true);
        Click.eachClick = InitialEachClick;
        FillAmount.fillAmount = 1;

        Debug.Log("Ended");
    }

    public void InstantiateParticleInPos(Vector3 pos)
    {
        GameObject temp = Instantiate(Particle, pos, Quaternion.identity) as GameObject;
        DestroyAfterTime(temp, 1f);
    }

    void DestroyAfterTime(GameObject go, float time) 
    {
        Destroy(go, time);
    }
    
}