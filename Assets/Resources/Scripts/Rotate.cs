//rotates an object over time.

using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

    public float speed; //turn speed
    private float Rotataion; //current rotation
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        Rotataion += speed;
        gameObject.transform.rotation = Quaternion.Euler(0f,0f,Rotataion);
	}
}
