using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Click : MonoBehaviour {

	// Use this for initialization
    public int TotalClicks;
    public static int eachClick = 1;

    // A UI Text: used to display a total amount of clicks
    public Text totalClicksText;

	// Update is called once per frame
	void Update () 
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            Vector3 mousePos = hit.point;

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Cookie") 
                {
                    TotalClicks += eachClick; 
                }

                if (hit.collider.tag == "Boost")
                {
                    Boosts boost = hit.collider.GetComponent<Boosts>();
                  
                    hit.collider.gameObject.SetActive(false);

                    StartCoroutine(GetBoost(boost));
                }
            }
        }

        totalClicksText.text = "Total cookies : " + TotalClicks.ToString();
	}

    public IEnumerator GetBoost(Boosts _boost)
    {
        float waited = 0;
        _boost.BeforeAction();

        while (waited <= _boost.GetDuration())
        {
            waited += Time.deltaTime;
            _boost.SetCurentTime(_boost.GetDuration() - waited);
            _boost.InAction();

            yield return 0;
        }

        _boost.AfterAction();
    }

}