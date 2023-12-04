using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
            Mathf.Clamp(transform.localPosition.x,-borderSize.x,borderSize.x),
            Mathf.Clamp(transform.localPosition.y, -borderSize.y, borderSize.y),
            transform.localPosition.z
            );

        if (Input.GetKey(KeyCode.DownArrow))
            force.y -= 1;

        time += Time.deltaTime;

        if (time < cameraCooldown)
            return;

        if (Input.GetKey(KeyCode.Space))
        {
            takeSnapshot?.Invoke();
            time = 0;
        }


    }
}
