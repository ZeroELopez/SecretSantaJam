using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

using Assets.Scripts.Base.Events;

public enum GameState
{
    Investigation, Chase, Escape, Count
}

public class GameManager : Singleton<GameManager>, ISubscribable<onGameWon>, ISubscribable<onGameLost>, ISubscribable<onCreatureCaptured>
{
    BoxCollider2D homeBase;
    public void SetHome(BoxCollider2D newHome) => homeBase = newHome;

    [SerializeField] float timer;
    float t;

    private GameState state = GameState.Investigation;

    public TextMeshProUGUI textObj;
    [SerializeField] float investigationDistance;


    // Start is called before the first frame update
    void Awake()
    {
        t = timer;

        SetInstance(this);
        DontDestroyOnLoad(this);
    }

    private void Start()=>        Subscribe();

    private void OnDestroy()=>        Unsubscribe();
    

    public void Subscribe()
    {
        EventHub.Instance.Subscribe<onGameLost>(this);
        EventHub.Instance.Subscribe<onGameWon>(this);
        EventHub.Instance.Subscribe<onCreatureCaptured>(this);
    }

    public void Unsubscribe()
    {
        EventHub.Instance.Unsubscribe<onGameLost>(this);
        EventHub.Instance.Unsubscribe<onGameWon>(this);
        EventHub.Instance.Unsubscribe<onCreatureCaptured>(this);
    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////////////////////////////////////////////////////////////
        //Check State for Investation and Chase
        ///////////////////////////////////////////////////////////////////////////

        if (Creature.focusCreature == null)
            return;

        float speed = Creature.focusCreature.GetComponent<FollowPath>().speed;

        if (speed > investigationDistance)
            ChangeState(GameState.Chase);
        else
            ChangeState(GameState.Investigation);

        ////////////////////////////////////////////////////////////////////////////
        //End Game Home Trigger
        ///////////////////////////////////////////////////////////////////////////

        if (state != GameState.Escape)
            return;

        Collider2D[] allCollisions = new Collider2D[10];

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;

        if (homeBase.OverlapCollider(filter, allCollisions) == 0)
            return;

        foreach (BoxCollider2D t in allCollisions)
            if (t != null && t.gameObject.GetComponent<PlayerMovement>())
            {
                Debug.Log("GameWon");
                EventHub.Instance.PostEvent(new onGameWon());
            }

        ////////////////////////////////////////////////////////////////////////////
        //End Game Timer
        ////////////////////////////////////////////////////////////////////////////

        t -= Time.deltaTime;
        textObj.text = "Escape : " + t.ToString();

        if (t > 0)
            return;

        Debug.Log("GameLost");
        EventHub.Instance.PostEvent(new onGameLost());
    }

    public static void ChangeState(GameState newState)
    {
        if (Instance.state == newState || Instance.state == GameState.Escape)
            return;

        Instance.state = newState;

        switch (Instance.state)
        {
            case GameState.Investigation:
                Instance.textObj.text = "Investigation";
                MusicManager.SetTrack("Investigation");
                EventHub.Instance.PostEvent(new onInvestigationMode());
                return;
            case GameState.Chase:
                Instance.textObj.text = "Chase";
                MusicManager.SetTrack("Chase");
                EventHub.Instance.PostEvent(new onChaseMode());
                return;
            case GameState.Escape:
                MusicManager.SetTrack("Escape");
                EventHub.Instance.PostEvent(new onEscapeMode());
                return;
        }
    }

    public UnityEvent onGameWon;
    public void HandleEvent(onGameWon evt)
    {
        state = GameState.Investigation;
        onGameWon?.Invoke();
        t = timer;
    }

    public UnityEvent onGameLose;
    public void HandleEvent(onGameLost evt)
    {
        state = GameState.Investigation;
        onGameLose?.Invoke();
        t = timer;
    }

    public void HandleEvent(onCreatureCaptured evt)
    {
        ChangeState(GameState.Escape);
    }
}
