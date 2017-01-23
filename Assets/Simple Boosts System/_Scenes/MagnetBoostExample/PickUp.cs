using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coin")
        {
            getCoin(other.gameObject);
        }
        else if (other.tag == "Boost")
        {
            if (other.GetComponent<Boosts>() != null)
            {
                Boosts boost = other.GetComponent<Boosts>();
                Destroy(other.gameObject);
                StartCoroutine(GetBoost(boost));
            }
            else 
            {
                Debug.Log("Error! You've forgotten to add Boost Component!");
            }
        }
        else if (other.tag == "Obstacle")
        {
            // Implement your own logic: e.g. diminish health, take off score, Game Over etc.
        }
        else
        {
            Debug.Log("Didn't handled by a trigger!");
        }
    }

    void getCoin(GameObject _coin)
    {
        Destroy(_coin);
        // Implement your own logic: e.g. increase score, get money, play sound etc.
    }

    public IEnumerator GetBoost(Boosts _boost)
    {
        Debug.Log("Started: " + _boost.GetDuration());
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