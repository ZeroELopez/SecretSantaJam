using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundBasedEffects : MonoBehaviour
{
    [SerializeField] UnityEvent onGrassWalk;
    [SerializeField] UnityEvent onSandWalk;
    [SerializeField] UnityEvent onGroundWalk;

    [SerializeField] UnityEvent onGrassDash;
    [SerializeField] UnityEvent onSandDash;
    [SerializeField] UnityEvent onGroundDash;

    [SerializeField] UnityEvent onGrassJump;
    [SerializeField] UnityEvent onSandJump;
    [SerializeField] UnityEvent onGroundJump;

    public void onWalk()
    {
        switch (LowerbodyScript.floorType)
        {
            case "Grass":
                onGrassWalk?.Invoke();
                break;
            case "Sand":
                onSandWalk?.Invoke();
                break;
            default:
                onGroundWalk?.Invoke();
                break;
        }
    }

    public void onDash()
    {
        switch (LowerbodyScript.floorType)
        {
            case "Grass":
                onGrassDash?.Invoke();
                break;
            case "Sand":
                onSandDash?.Invoke();
                break;
            default:
                onGroundDash?.Invoke();
                break;
        }
    }

    public void onJump()
    {
        switch (LowerbodyScript.floorType)
        {
            case "Grass":
                onGrassJump?.Invoke();
                break;
            case "Sand":
                onSandJump?.Invoke();
                break;
            default:
                onGroundJump?.Invoke();
                break;
        }
    }

}
