using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.Base.Events;

public class ScoreTracker : MonoBehaviour, ISubscribable<onCreatureCaptured>, ISubscribable<RequestScore>
{
    private int score;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        Subscribe();
        score = 0;
        text = GetComponent<TextMeshProUGUI>();
        text.text = "Score: " + score.ToString();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    public void AddPoints(int points) 
    {
        score += points;
        text.text = score.ToString();
    }

    public void Subscribe()
    {
        EventHub.Instance.Subscribe<onCreatureCaptured>(this);
        EventHub.Instance.Subscribe<RequestScore>(this);
    }

    public void Unsubscribe()
    {
        EventHub.Instance.Unsubscribe<onCreatureCaptured>(this);
        EventHub.Instance.Unsubscribe<RequestScore>(this);
    }

    public void HandleEvent(onCreatureCaptured evt)
    {
        AddPoints(evt.page.points);
    }

    public void HandleEvent(RequestScore evt)
    {
        EventHub.Instance.PostEvent(new RequestScoreResponse { Score = score });
    }
}
