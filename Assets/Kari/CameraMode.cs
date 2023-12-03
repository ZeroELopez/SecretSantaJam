using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraMode : MonoBehaviour
{
    [SerializeField]Transform pos1;
    [SerializeField] Transform pos2;

    [SerializeField] float transitionSpeed;

    public UnityEvent onCameraOn;
    public UnityEvent onCameraOff;

    public UnityEvent snapTaken;


    Creature[] creatures;
    [SerializeField] float cameraReadyDistance = .5f;

    [SerializeField] Rect screenRect;
    // Start is called before the first frame update
    void Start()
    {
        creatures = GameObject.FindObjectsOfType<Creature>();
    }
    public static bool cameraOn = false;
    // Update is called once per frame
    void Update()
    {

        cameraOn = Input.GetKey(KeyCode.LeftControl);
        PlayerMovement.still = cameraOn;

        Vector3 target = cameraOn ? pos2.position : pos1.position;

        transform.position = Vector3.MoveTowards(transform.position, target, transitionSpeed * Time.deltaTime);


        Collider2D[] allCollisions = new Collider2D[10];

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.useDepth = false;
    }

    public void TakeSnapshot()
    {
        if (Vector3.Distance(transform.position, pos2.position) > cameraReadyDistance)
            return;


        foreach (Creature c in creatures)
            if (WithinCameraShot(Camera.main.WorldToViewportPoint(c.transform.position)))
                snapTaken?.Invoke();
    }

    bool WithinCameraShot(Vector3 screenPos)
        => screenPos.x > screenRect.xMin && screenPos.x < screenRect.xMax &&
            screenPos.y > screenRect.yMin && screenPos.y < screenRect.yMax;

}
