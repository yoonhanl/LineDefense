using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneP.InfinityScrollView;
using UnityEngine.UI;

public class RankingItem : InfinityBaseItem
{
    public Text textScore;
    public Text textName;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Reload(InfinityScrollView infinity, int _index)
    {
        base.Reload(infinity, _index);

        
        if(InterfaceManager.Instance.arrayPerson!=null)
        {
            if(InterfaceManager.Instance.arrayPerson.Length>_index)
            {
                textScore.text = InterfaceManager.Instance.arrayPerson[_index].score;
                textName.text = InterfaceManager.Instance.arrayPerson[_index].uuid;
            }
        }

    }

}