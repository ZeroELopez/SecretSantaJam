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

public class GameManager : Singleton<GameManager>, ISubscribable<onGameStart>,ISubscribable<onGameWon>, ISubscribable<onGameLost>, ISubscribable<onSpecialCreatureCaptured>
{
    public BoxCollider2D homeBase;
    public void SetHome(BoxCollider2D newHome) => homeBase = newHome;
    [SerializeField] bool TimerOn = true;
    [SerializeField] float LevelTimer = 120;
    float time;

    public GameState state { get; private set; }

    public TextMeshProUGUI textObj;
    [SerializeField] float investigationDistance;

    public Encyclopedia encyclopedia;
    public static List<Page> newPages = new List<Page>();

    public float musicMaxDistance = 100;
    public float musicMinDistance = 5;
    // Start is called before the first frame update
    void Awake()
    {
        time = LevelTimer;

        SetInstance(this);
        DontDestroyOnLoad(this);
        ChangeState(GameState.Count);
        MusicManager.SetTrack("Forest");

    }

    void ShowPages(RawImage[] images)
    {
        for(int i = 0; i < images.Length && i < newPages.Count; i++)
        {
            images[i].texture = newPages[i].image;
            images[i].color= Color.white;
        }
    }

    private void Start()=>        Subscribe();

    private void OnDestroy()=>        Unsubscribe();
    
    public static void AddPagesToEncyclopedia()
    {
        foreach (Page p in newPages)
            Instance.encyclopedia.pages.Add(p);

        newPages.Clear();
    }

    public void Subscribe()
    {
        PictureImages.setPhotos += ShowPages;

        EventHub.Instance.Subscribe<onGameStart>(this);
        EventHub.Instance.Subscribe<onGameLost>(this);
        EventHub.Instance.Subscribe<onGameWon>(this);
        EventHub.Instance.Subscribe<onSpecialCreatureCaptured>(this);
    }

    public void Unsubscribe()
    {
        PictureImages.setPhotos -= ShowPages;

        EventHub.Instance.Unsubscribe<onGameStart>(this);
        EventHub.Instance.Unsubscribe<onGameLost>(this);
        EventHub.Instance.Unsubscribe<onGameWon>(this);
        EventHub.Instance.Unsubscribe<onSpecialCreatureCaptured>(this);
    }
    int oldT = -1000;
    // Update is called once per frame
    void Update()
    {
        ////////////////////////////////////////////////////////////////////////////
        //End Game Timer
        ////////////////////////////////////////////////////////////////////////////

        time -= TimerOn? Time.deltaTime: 0;
        int showT = Mathf.CeilToInt(time);
        if (oldT != showT)
            textObj.text = state.ToString() + " : " + (oldT = showT).ToString();

        if (time <= 0)
            EventHub.Instance.PostEvent(new onGameLost());

        ////////////////////////////////////////////////////////////////////////////
        //Check State for Investation and Chase
        ///////////////////////////////////////////////////////////////////////////


        if (Creature.focusCreature == null)
            return;

        float distance = Vector3.Distance(Creature.focusCreature.transform.position,PlayerMovement.position);


        MusicManager.layerFill = Mathf.InverseLerp(musicMaxDistance,musicMinDistance, distance); ;

        if (MusicManager.layerFill > investigationDistance)
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

        if (!homeBase || homeBase.OverlapCollider(filter, allCollisions) == 0)
            return;

        foreach (BoxCollider2D t in allCollisions) 
            if (t != null && t.gameObject.GetComponent<PlayerMovement>())
            {
                Debug.Log("GameWon");
                EventHub.Instance.PostEvent(new onGameWon());
            }

    }

    public void SetTimer(float newValue) 
    {
        time = newValue;
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
                EventHub.Instance.PostEvent(new onInvestigationMode());
                return;
            case GameState.Chase:
                Instance.textObj.text = "Chase";
                EventHub.Instance.PostEvent(new onChaseMode());
                return;
            case GameState.Escape:
                EventHub.Instance.PostEvent(new onEscapeMode());
                return;
        }
    }

    public UnityEvent onGameWon;
    public void HandleEvent(onGameWon evt)
    {
        state = GameState.Investigation;
        TimerOn = false;
        onGameWon?.Invoke();
        time = LevelTimer;
    }

    public UnityEvent onGameLose;
    public void HandleEvent(onGameLost evt)
    {
        state = GameState.Investigation;
        TimerOn = false;
        onGameLose?.Invoke();
        time = LevelTimer;
    }

    public void HandleEvent(onSpecialCreatureCaptured evt)
    {
        ChangeState(GameState.Escape);
    }

    public UnityEvent onGameStart;
    public void HandleEvent(onGameStart evt)
    {
        time = LevelTimer;
        TimerOn = true;
        onGameStart?.Invoke();
    }
}


[System.Serializable]
public class Encyclopedia
{
    public List<Page> pages = new List<Page>();
}

[System.Serializable]
public class Page
{
    public Texture2D image;

    public int points;

    public string name;
    public string description;
}