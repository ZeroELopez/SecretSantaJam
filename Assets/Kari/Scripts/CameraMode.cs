using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;

using Assets.Scripts.Base.Events;
using Kari.SoundManagement;

//so the original script was made by me, Kari, but
//Griffin edited it so that there is a point system
//to the game
public class CameraMode : MonoBehaviour, ISubscribable<onCameraToggle>
{
    public static Action<float> onFrameChange;

    [Header("Camera Movement")]
    [SerializeField] Vector2 borderSize;
    [SerializeField] float panSpeed;

    [Header("Taking Picture")]
    [SerializeField] float cameraReadyDistance = .5f;
    [SerializeField] float cameraCooldown;
    float cooldownTime;

    //p1 is normal platformer camera pos and 
    //p2 is the picture mode camera pos
    [Header("Camera Transition")]
    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;

    //speed of transition, have it be editable by designers
    [SerializeField] float transitionSpeed;

    //unused events that can be taken out and out into
    //event hub
    public UnityEvent onCameraOn;
    public UnityEvent onCameraOff;

    //event for taking a successful picture of a creature
    public UnityEvent onCreatureCaptured;
    public UnityEvent onTakeSnapshot;


    //ScoreTracker scoreTracker;
    Creature[] creatures;
    [SerializeField] Rect screenRect;

    private Controls playerControls;
    Vector3 movement;

    public void Subscribe()
    {
        EventHub.Instance.Subscribe<onCameraToggle>(this);
    }

    public void Unsubscribe()
    {
        EventHub.Instance.Unsubscribe<onCameraToggle>(this);
    }
    //so 
    // Start is called before the first frame update
    void Start()
    {
        target = pos1;
        Subscribe();

        creatures = GameObject.FindObjectsOfType<Creature>();

        playerControls = new Controls();
        playerControls.Enable();

        playerControls.Actions.Move.started += OnMove;
        playerControls.Actions.Move.performed += OnMove;
        playerControls.Actions.Move.canceled += OnMove;

        playerControls.Actions.TakeSnapshot.started += TakeSnapshot;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void OnMove(InputAction.CallbackContext context) =>        movement = context.ReadValue<Vector2>();

    public void HandleEvent(onCameraToggle evt)
    {
        target = evt.On ? pos2 : pos1;
        cameraOn = evt.On;

        StopCoroutine("cameraTransition");
        StartCoroutine("cameraTransition");

        if (evt.On)
            onCameraOn?.Invoke();
        else
            onCameraOff?.Invoke();
    }

    public bool cameraOn = false;
    Transform target;

    // Update is called once per frame
    void Update()
    {
        cooldownTime += Time.deltaTime;

        if (!cameraOn)
            return;

        pos2.localPosition += movement * panSpeed * Time.deltaTime;

        if (movement != Vector3.zero)
            AudioManager.PlaySound("CameraPan");


        pos2.localPosition = new Vector3(
                Mathf.Clamp(pos2.localPosition.x, -borderSize.x, borderSize.x),
                Mathf.Clamp(pos2.localPosition.y, -borderSize.y, borderSize.y),
                pos2.localPosition.z
                );
    }

    public float transitionTime { get; private set; }
    float fps = 0.016666f;
    IEnumerator cameraTransition()
    {
        AudioManager.PlaySound("CameraZoomIn");

        Vector3 origin = transform.position;
        transitionTime = 0;
        float time = 0;
        while (time < transitionSpeed || cameraOn)
        {
            time += fps;
            transitionTime = Mathf.InverseLerp(0, transitionSpeed, time);
            transform.position = Vector3.Lerp(origin, target.position, transitionTime);
            onFrameChange?.Invoke(transitionTime);
            yield return new WaitForSeconds(fps);
        }
    }

    //the logic for taking a snapshot
    public void TakeSnapshot(InputAction.CallbackContext context)
    {
        if (cooldownTime < cameraCooldown || !cameraOn)
            return;

        cooldownTime = 0;
        onTakeSnapshot?.Invoke();
        

        //if camera is not in position then return with no picture 
        if (Vector3.Distance(transform.position, pos2.position) > cameraReadyDistance)
            return;

        AudioManager.PlaySound("CameraFlash");

        int points = 0;
        foreach (Creature c in creatures)
            if (WithinCameraShot(Camera.main.WorldToViewportPoint(c.transform.position)))
            {
                if (c.creatureCaptured)
                    continue;

                float distance = Vector2.Distance(pos2.position, c.transform.position);
                if (distance > 3.2f)
                {
                    points += c.poorScore;
                }
                else if (distance > 1.6f)
                {
                    points += c.goodScore;
                }
                else
                {
                    points += c.greatScore;
                }
                EventHub.Instance.PostEvent(new onCreatureCaptured() { points = points });

                if (c.FocusCreature)
                    EventHub.Instance.PostEvent(new onSpecialCreatureCaptured() {});

                onCreatureCaptured?.Invoke();
                c.creatureCaptured = true;
                AudioManager.PlaySound("CameraSuccess");

                //scoreTracker.AddPoints(points);
            }
    }

    bool WithinCameraShot(Vector3 screenPos)
        => screenPos.x > screenRect.xMin && screenPos.x < screenRect.xMax &&
            screenPos.y > screenRect.yMin && screenPos.y < screenRect.yMax;





}
