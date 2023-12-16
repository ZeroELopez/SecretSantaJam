using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBasedOnDir : MonoBehaviour
{
    [SerializeField]EmuContra soldier;
    [SerializeField] float turnSpeed = 1;

    public void StartShooting(Vector2 dir)
    {
        StartCoroutine("Shooting",dir);
    }

    IEnumerator Shooting(Vector2 dir)
    {
        Vector3 dirV3 = dir;
        while (transform.right != dirV3)
        {
            transform.right = Vector3.MoveTowards(transform.right, dirV3, Time.deltaTime * turnSpeed);
            yield return new WaitForSeconds(.0166f);
        }
    }
}
