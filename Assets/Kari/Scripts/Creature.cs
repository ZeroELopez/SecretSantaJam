using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void OnDisable()
    {
        if (FocusCreature)
            focusCreature = null;
    }

    private void OnDestroy()
    {
        if (FocusCreature)
            focusCreature = null;
    }
}
