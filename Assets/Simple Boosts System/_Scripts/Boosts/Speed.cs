using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Speed : Boosts {
    
    // A new speed value that could be given by the boost
    public float newSpeed;

    // Player's initial movement speed
    private float InitialSpeed;

    // Component of Movement.cs script that changes speed
    private Movement movement;              

    void Start()
    {
        Init();

        movement = Player.GetComponent<Movement>();

        InitialSpeed = movement.speed;
    }

    public override void BeforeAction()
    {

        if (Particle != null && ParicleTransform != null)
        {
            InstantiateParticle();
        }
        InstantiateIndicator();

        movement.speed = newSpeed;
    }

    public override void InAction()
    {
        TimeBar();
    }

    public override void AfterAction()
    {
        DestroyParticle();
        DestroyIndicator();
        movement.speed = InitialSpeed;
    }

}