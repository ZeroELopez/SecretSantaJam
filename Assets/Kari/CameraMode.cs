using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//so the original script was made by me, Kari, but
//Griffin edited it so that there is a point system
//to the game
public class CameraMode : MonoBehaviour
{
//p1 is normal platformer camera pos and 
//p2 is the picture mode camera pos
    [SerializeField]Transform pos1;
    [SerializeField] Transform pos2;

//speed of transition, have it be editable by designers
    [SerializeField] float transitionSpeed;

//unused events that can be taken out and out into
//event hub
    public UnityEvent onCameraOn;
    public UnityEvent onCameraOff;

//event for taking a successful picture of a creature
    public UnityEvent snapTaken;


    Creature[] creatures;
    ScoreTracker scoreTracker;
    [SerializeField] float cameraReadyDistance = .5f;

    [SerializeField] Rect screenRect;

//so 
    // Start is called before the first frame update
    void Start()
    {
        creatures = GameObject.FindObjectsOfType<Creature>();
        scoreTracker = GameObject.FindObjectOfType<ScoreTracker>();
    }
    public static bool cameraOn = false;
    // Update is called once per frame
    void Update()
    {
//Check input from player. move camera if player is
//in camera mode.
        cameraOn = Input.GetKey(KeyCode.LeftControl);
        PlayerMovement.still = cameraOn;

        Vector3 target = cameraOn ? pos2.position : pos1.position;

        transform.position = Vector3.MoveTowards(transform.position, target, transitionSpeed * Time.deltaTime);


        Collider2D[] allCollisions = new Collider2D[10];

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.useDepth = false;
    }


//the logic for taking a snapshot
    public void TakeSnapshot()
    {
//if camera is not in position then return with no picture 
        if (Vector3.Distance(transform.position, pos2.position) > cameraReadyDistance)
            return;


        int points = 0;
        foreach (Creature c in creatures)
            if (WithinCameraShot(Camera.main.WorldToViewportPoint(c.transform.position))) 
            {
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
                scoreTracker.AddPoints(points);

                snapTaken?.Invoke();
            }
    }

    bool WithinCameraShot(Vector3 screenPos)
        => screenPos.x > screenRect.xMin && screenPos.x < screenRect.xMax &&
            screenPos.y > screenRect.yMin && screenPos.y < screenRect.yMax;

}
