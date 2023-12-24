using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Base.Events;

public class Creature : MonoBehaviour
{
    public static Creature focusCreature;
    
    public bool FocusCreature;
    public bool creatureCaptured;

    public int poorScore;
    public int goodScore;
    public int greatScore;

    public Page page;
    //public FollowPath path = null;

    private void Start()
    {
        if (FocusCreature) 
        {
            focusCreature = this;
            GameObject gameObject = this.gameObject;

            //while (path == null) 
            //{
            //    path = gameObject.GetComponent<FollowPath>();
            //    gameObject = gameObject.transform.parent.gameObject;
            //}
        }

        //Subscribe();
    }

    private void Update()
    {
        if (creatureCaptured)
            gameObject.SetActive(false);
    }

    //private void OnDisable()
    //{
    //    if (FocusCreature)
    //        focusCreature = null;

    //    //Unsubscribe();
    //}

    private void OnDestroy()
    {
        if (FocusCreature)
            focusCreature = null;

        //Unsubscribe();
    }

    //public void Subscribe()
    //{
    //    EventHub.Instance.Subscribe(this);
    //}

    //public void Unsubscribe()
    //{
    //    EventHub.Instance.Subscribe(this);
    //}

    //public void HandleEvent(onChaseMode evt)
    //{
    //}
}
