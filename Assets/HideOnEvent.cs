using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Base.Events;

public class HideOnEvent : SubscribeBehavior<onEscapeMode> {
    public override void HandleEventVirtual(onEscapeMode evt)
    {
        base.HandleEventVirtual(evt);

        gameObject.SetActive(false);
    }
}
