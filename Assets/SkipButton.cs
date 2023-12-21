using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkipButton : MonoBehaviour
{
    private Button button;

    [SerializeField]
    private UnityAnimatorEvents events;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    public void ClickFunction()
    {
        events.LoadSceneAsync("LoadingScreen");

        button.enabled = false;
    }
}
