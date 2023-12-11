using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Base.Events;

public class LoseGameTrigger : TriggerScript
{
    public override void onStill(Component script)
    {
        if (GameManager.Instance.state != GameState.Escape)
            return;

        EventHub.Instance.PostEvent(new onGameLost());
    }
}
