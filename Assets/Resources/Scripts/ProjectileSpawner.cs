//This script spawns the projectiles and launches them

using UnityEngine;
using System.Collections;

public class ProjectileSpawner : MonoBehaviour {

    //the name of the projectile prefab
    public string ProjectileName = "Projectile";

    //a pool manager is used to make this game more mobile friendly 
    private PoolManager poolManager;

    [Header("Fire Rates")]
    //how often a projectile will be fired
    public float FireRate = 1f;

    //the FireRate will update based on the FireCount
    public float FireRate1 = 1.5f;
    public float FireRate2 = 1f;
    public float FireRate3 = 0.75f;

    //Last time a projectile was fired
    private float LastFireTime = 0f;

    //the distance the projectile should start at
    public float StartDist = 1f;

    //max and min speed of the projectile
    public float maxSpeed;
    public float minSpeed;

    //number of projectiles fired
    public int fireCount = 0;



	// Use this for initialization
	void Start () 
    {
        poolManager = GameObject.FindObjectOfType<PoolManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        //Fire!...if it's time to
        if (Time.time - LastFireTime >= FireRate)
        {
            float degree = Random.Range(0f,360f);

            Vector3 Pos = Constants.GetXYDirection(degree,StartDist);

            GameObject NewProjectile = poolManager.Spawn(ProjectileName);
            NewProjectile.transform.position = Pos;
            NewProjectile.GetComponent<Projectile>().Launch(Random.Range(minSpeed,maxSpeed));
            NewProjectile.GetComponent<Projectile>().BeginPathCoroutine();

            LastFireTime = Time.time;

            fireCount += 1; 

        }

        //update the fire rate based on the number of the projectiles fired
        if (fireCount < 10)
        {
            FireRate = FireRate1;
        }
        else if (fireCount >= 10)
        {
            FireRate = FireRate2;
        }
        else if (fireCount >= 20)
        {
            FireRate = FireRate3;
        }
	}





}
