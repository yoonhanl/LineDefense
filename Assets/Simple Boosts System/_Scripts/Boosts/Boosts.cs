using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class Boosts : MonoBehaviour {

    // A reference to the Player
    protected GameObject Player;

    // Indicators' characteristics ("GO" stands for GameObject)
    public GameObject GridLayout;
    public GameObject IndicatorGO;
    protected Image FillAmount;
    protected GameObject InstantiatedIndicator;

    // Properties of the Boosts
    public float Duration;
    protected float CurentTime;

    // A references to the Particle System
    public GameObject Particle;
    public Transform ParicleTransform;

    // Initialization of objects
    public void Init()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Get total duration of the effect
    public float GetDuration()
    {
        return Duration;
    }

    // Curent time of boost
    public void SetCurentTime(float _time)
    {
        CurentTime = _time;
    }

    // Implement your own logic before the Action: e.g. initialization, setTime etc.
    public abstract void BeforeAction();

    // Implement your own logic in time of Action: e.g. magnetization of coins, setIndicator etc.
    public abstract void InAction();

    // Implement your own logic after the Action: e.g. reset, destroy etc.
    public abstract void AfterAction();
    
    public virtual void InstantiateParticle() {
        GameObject particle = Instantiate(Particle, ParicleTransform.position, Quaternion.identity) as GameObject;
        Particle = particle;
        Particle.transform.position = Vector3.zero;
        Particle.transform.SetParent(ParicleTransform, false);
    }

    public virtual void InstantiateIndicator()
    {
        if (IndicatorGO != null && GridLayout != null)
        {
            InstantiatedIndicator = Instantiate(IndicatorGO);
            FillAmount = InstantiatedIndicator.transform.FindChild("FillAmount").GetComponent<Image>();
            InstantiatedIndicator.transform.parent = GridLayout.transform;
            InstantiatedIndicator.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public virtual void DestroyParticle() {
        if (Particle != null)
            Destroy(Particle);
    }

    public virtual void DestroyIndicator() {
        Destroy(InstantiatedIndicator);
    }

    public virtual void TimeBar()
    {
        FillAmount.fillAmount = (CurentTime / Duration);
    }

}