using Assets.Scripts.Base.Events;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreCardDisplay : MonoBehaviour, ISubscribable<RequestScoreResponse>
{
    TextMeshProUGUI scoreDisplay;

    public void HandleEvent(RequestScoreResponse evt)
    {
        scoreDisplay.text = evt.Score.ToString();
    }

    public void Subscribe()
    {
        EventHub.Instance.Subscribe(this);
    }

    public void Unsubscribe()
    {
        EventHub.Instance.Unsubscribe(this);
    }

    //Retrieve Text Mesh UGUI
    private void Start()
    {
        scoreDisplay = GetComponent<TextMeshProUGUI>();
        Subscribe();

        EventHub.Instance.PostEvent(new RequestScore());
    }

    void OnDestroy()
    {
        Unsubscribe();
    }
}
