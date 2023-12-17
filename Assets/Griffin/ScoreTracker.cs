using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.Base.Events;

public class ScoreTracker : MonoBehaviour, ISubscribable<onCreatureCaptured>
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
    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPoints(int points) 
    {
        score += points;
        text.text = "Score: " + score.ToString();
    }

    public void Subscribe()
    {
        EventHub.Instance.Subscribe(this);
       }

    public void Unsubscribe()
    {
        EventHub.Instance.Unsubscribe(this);
    }

    public void HandleEvent(onCreatureCaptured evt)
    {
        AddPoints(evt.page.points);
    }
}
