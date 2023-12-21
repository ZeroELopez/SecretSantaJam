using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    Controls playerControls;
    Button[] list;
    int selected;

    // Start is called before the first frame update
    void Start()
    {
        list = GetComponentsInChildren<Button>();
        selected = 0;
        playerControls = new Controls();
        playerControls.Enable();

        //Set up Input System callbacks;
        playerControls.Actions.Move.started += OnMove;

        playerControls.Actions.Jump.started += OnSelect;
        playerControls.Actions.TakeSnapshot.started += OnSelect;

    }

    private void OnMove(InputAction.CallbackContext context)
    {
        selected -= Mathf.CeilToInt(context.ReadValue<Vector2>().y);
        Mathf.Clamp(selected, 0, list.Length - 1);

    }

    private void OnSelect(InputAction.CallbackContext context)=>        list[selected].onClick?.Invoke();


}
