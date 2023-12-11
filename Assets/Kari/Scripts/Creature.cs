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

    private void Start()
    {
        if (FocusCreature)
            focusCreature = this;
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
