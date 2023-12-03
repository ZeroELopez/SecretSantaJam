using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMode : MonoBehaviour
{
    [SerializeField]Transform pos1;
    [SerializeField] Transform pos2;

    [SerializeField] float transitionSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public static bool cameraOn = false;
    // Update is called once per frame
    void Update()
    {
        cameraOn = Input.GetKey(KeyCode.LeftControl);
        PlayerMovement.still = cameraOn;
        Vector3 target = cameraOn ? pos2.position : pos1.position;

        transform.position = Vector3.MoveTowards(transform.position, target, transitionSpeed * Time.deltaTime);
    }
}
