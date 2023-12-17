using Assets.Scripts.Base.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour, ISubscribable<onEscapeMode>
{
    public float shakeTime = 1.0f;
    public float shakeIntensity = 1.0f;

    private Vector3 initialPosition;
    private float currentShakeTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (currentShakeTime > 0)
        {
            //random position offset
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            transform.localPosition = initialPosition + randomOffset;
            currentShakeTime -= Time.deltaTime;
        }
        else
        {
            //return to initial position
            transform.localPosition = initialPosition;
        }
    }

    public void ShakeIt()
    {
        currentShakeTime = shakeTime;

    }

    public void HandleEvent(onEscapeMode evt)
    {
        ShakeIt();
    }

    public void Subscribe()
    {
        EventHub.Instance.Subscribe<onEscapeMode>(this);
    }

    public void Unsubscribe()
    {
        EventHub.Instance.Unsubscribe<onEscapeMode>(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        Subscribe();
        initialPosition = transform.localPosition;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

}
