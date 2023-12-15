using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Events;

public class PlayDialogue : MonoBehaviour
{
    [SerializeField] float delayStart = 10;

    [SerializeField] string[] dialogue;
    [SerializeField] float pauseBetween;

    [SerializeField] GameObject dialogueBox;
    [SerializeField] Transform dialogueParent;

    Action onNewDialogue;

    public UnityEvent onCompleted;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Play");
    }

    IEnumerator Play()
    {
        yield return new WaitForSeconds(delayStart);


        foreach (string line in dialogue)
        {
            GameObject newBox = Instantiate(dialogueBox,dialogueParent);
            newBox.transform.localPosition = Vector3.zero;

            onNewDialogue += newBox.GetComponent<MakeRoomScript>().StartAdjust;
            newBox.GetComponentInChildren<TextMeshPro>().text = line;

            onNewDialogue?.Invoke();
            yield return new WaitForSeconds(pauseBetween);
        }

        onCompleted?.Invoke();
        onNewDialogue = null;
    }
}
