using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Base.Events;

public class CameraAnimator : MonoBehaviour, ISubscribable<onCameraToggle>
{
    [SerializeField] string OnAnimation;
    [SerializeField] string OffAnimation;

    Animator thisAnimator;
    string state;

    public void HandleEvent(onCameraToggle evt)
    {
        if (evt.On)
            state = OnAnimation;
        else
            state = OffAnimation;
    }

    public void Subscribe()
    {
        CameraMode.onFrameChange += Animate;
        EventHub.Instance.Subscribe<onCameraToggle>(this);
    }

    public void Unsubscribe()
    {
        CameraMode.onFrameChange -= Animate;
        EventHub.Instance.Unsubscribe<onCameraToggle>(this);
    }

    // Update is called once per frame
    void Start()
    {
        
        Subscribe();
        thisAnimator = GetComponent<Animator>();
        thisAnimator.speed = 0;
    }

    void Animate(float t)
    {
        thisAnimator.Play(state, 0, t);
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}
