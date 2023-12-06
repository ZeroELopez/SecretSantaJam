using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//this script is to define the safe zone and to
//call the game won event.
//Personally I think this should be in a game
//manager script. along with the points and timer
public class HomScript : MonoBehaviour
{
//the trigger for the safe zone used during the
//escape mode
    BoxCollider2D boxCollider2D;

//this shouldn't be a global variable at all.
    public static bool EndAvailable = false;
//this is used by the events onGameWon and onGameLost
//it's to reset the availability which shouldn't be global
//so it's best if we get rid of doing it this way.
    public void setAvailable(bool set) => EndAvailable = set;

//event called when game has won
    public UnityEvent onGameWon;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

//my logic for getting overlapping colliders.
//I think this is a good place to explain why I use 
//this logic. A previous game of mine had 
//difficulties with judging whether a player was 
//still on the ground or not. I found this way to be 
//more reliable than the traditional onTriggerEnter


    }
}
