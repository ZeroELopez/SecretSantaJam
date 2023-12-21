using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    [SerializeField] Vector3 moveBy;


    // Update is called once per frame
    void Update()
    {
        transform.position += moveBy * Time.deltaTime;
    }
}
