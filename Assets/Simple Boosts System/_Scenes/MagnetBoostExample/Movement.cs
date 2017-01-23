using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    // Player's initial movement speed
    public float speed = 5f;
	
	// Update is called once per frame
	void Update () 
    {
        gameObject.transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0f, 0f);
	}

}