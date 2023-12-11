using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

//script that counts down to 0 and once it does it 
//calls the game lose event. it is disabled by 
//default and enabled when a successful picture of 
//the creature was taken.
//Personally think this should be a part of a game manager script
public class Timer : MonoBehaviour
{
    public UnityEvent onLose;

    [SerializeField]float timer;
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        text.text = timer.ToString();

        if (timer > 0)
            return;

        onLose?.Invoke();
        timer = float.MaxValue;
    }
}
