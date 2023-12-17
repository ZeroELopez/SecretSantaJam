
namespace Assets.Scripts.Base.Events
{
    public class onTakePhoto : DispatchableEvent { }

    public class onCreatureCaptured : DispatchableEvent
    {
        public Page page;
    }

    public class onCameraToggle : DispatchableEvent
    {
        public bool On;
    }

    public class onSpecialCreatureCaptured : DispatchableEvent { }
    public class onInvestigationMode : DispatchableEvent { }
    public class onChaseMode : DispatchableEvent { }
    public class onEscapeMode : DispatchableEvent { }

    public class onCutsceneToggle : DispatchableEvent { public bool On; }

    public class onPlayerRespawn : DispatchableEvent { }

    public class onGameStart : DispatchableEvent { }
    public class onGameWon : DispatchableEvent { }
    public class onGameLost : DispatchableEvent { }

    public class TryAgain : DispatchableEvent { }
    public class ExitToMainMenu : DispatchableEvent { }

    public class TogglePause : DispatchableEvent
    {

    }
}

