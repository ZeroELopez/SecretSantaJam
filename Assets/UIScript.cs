using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    Controls playerControls;
    Button[] list;
    [SerializeField] SpriteRenderer[] visualSprites;
    [SerializeField] Image[] visualImages;
    Sprite[] originalVisuals;

    int selected;

    private static UIScript focus;

    // Start is called before the first frame update
    void Start()
    {

        list = GetComponentsInChildren<Button>();
        selected = 0;

        GrabDefaultValues();

        //Set up Input System callbacks;
        playerControls.Actions.Move.started += OnMove;

        playerControls.Actions.Jump.started += OnSelect;

        UpdateVisuals();
    }

    private void Update()
    {
        if (focus == null)
            focus = this;

        //Debug.Log(focus);
    }
    private void OnEnable()
    {
        playerControls = new Controls();
        playerControls.Enable();

        playerControls.Actions.Move.started += OnMove;
        playerControls.Actions.Jump.started += OnSelect;

        focus = this;

    }
    private void OnDisable()
    {
        playerControls.Actions.Move.started -= OnMove;

        playerControls.Actions.Jump.started -= OnSelect;

        if (focus == this)
            focus = null;

    }
    private void OnDestroy()
    {
        if (focus == this)
            focus = null;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (focus != this)
            return;

        selected -= Mathf.CeilToInt(context.ReadValue<Vector2>().y);
        selected = Mathf.Clamp(selected, 0, list.Length - 1);

        UpdateVisuals();
    }

    void GrabDefaultValues()
    {
    }

    void UpdateVisuals()
    {
        //turn off all Sprite renderers except for the selected Index
        for (int i = 0; i < list.Length && i < visualSprites.Length; i++)
        {
            //visualImages[i].enabled = i == selected; //Just in case
            visualSprites[i].enabled = i == selected;
        }

        for (int i = 0; i < list.Length && i < visualImages.Length; i++)
        {
            //visualImages[i].enabled = i == selected; //Just in case
            visualImages[i].enabled = i == selected;
        }
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        if (focus != this)
            return;

        list[selected].onClick?.Invoke();
    }


}
