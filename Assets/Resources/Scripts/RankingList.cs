using OneP.InfinityScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingList : MonoBehaviour {
    public InfinityScrollView verticalScroll;

    public List<Person> list = new List<Person>();
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ListInitialize(int nCount)
    {
        verticalScroll.Setup(nCount);

        if(nCount>0)
            verticalScroll.InternalReload();
    }
}
