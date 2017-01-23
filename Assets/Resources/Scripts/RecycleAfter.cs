//similar to DestroyAfter.cs however this will recycle an object instead of Destorying it.

using UnityEngine;
using System.Collections;

public class RecycleAfter : MonoBehaviour 
{
    //a pool manager is used to make this game more mobile friendly 
    private PoolManager poolManager;

    //time the object started to exist
    private float StartTime;

    //time until it's recycled
    public float time = 3f;

    //on enable reset time
    void OnEnable()
    {
        StartTime = Time.time;
    }

    //set variables
    void Start () 
    {
        poolManager = GameObject.FindObjectOfType<PoolManager>();
        StartTime = Time.time;
    }

	// Update is called once per frame
	void FixedUpdate () 
    {
        if (Time.time - StartTime >= time)
        {
            poolManager.Recycle(gameObject.name.Replace("(Clone)",""),gameObject);
        }
	
	}
}
