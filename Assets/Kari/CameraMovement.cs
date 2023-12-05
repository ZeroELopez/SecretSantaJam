using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//in charge of the controls when in picture mode
//Both movement and the button for taking a picture
//is here but not the logic for taking the picture
//or getting out of it. Should combine this script 
//with Camera mode?
public class CameraMovement : MonoBehaviour
{
    [SerializeField]Vector2 borderSize;
    [SerializeField] float panSpeed;

    [SerializeField] UnityEvent takeSnapshot;
    [SerializeField] float cameraCooldown;
    float time;

    BoxCollider2D thisCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        thisCollider2D = GetComponent<BoxCollider2D>();
    }



    // Update is called once per frame
    void Update()
    {
        if (!CameraMode.cameraOn)
            return;

        Vector3 force = new Vector3();

        if (Input.GetKey(KeyCode.LeftArrow))
            force.x -= 1;
        if (Input.GetKey(KeyCode.RightArrow))
            force.x += 1;
        if (Input.GetKey(KeyCode.UpArrow))
            force.y += 1;
        if (Input.GetKey(KeyCode.DownArrow))
            force.y -= 1;

        force *= panSpeed;

        transform.localPosition += force * Time.deltaTime;

        transform.localPosition = new Vector3(
//Limit the movement based on the variable border size
//this will keep the camera at a certain range of the player
            Mathf.Clamp(transform.localPosition.x,-borderSize.x,borderSize.x),
            Mathf.Clamp(transform.localPosition.y, -borderSize.y, borderSize.y),
            transform.localPosition.z
            );

//cooldown for camera

        time += Time.deltaTime;

        if (time < cameraCooldown)
            return;

        if (Input.GetKey(KeyCode.Space))
        {
//take camera shot and reset cooldown
            takeSnapshot?.Invoke();
            time = 0;
        }


    }
}
