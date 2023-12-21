using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform objectiveTransform;
        if (GameManager.Instance.state == GameState.Investigation || GameManager.Instance.state == GameState.Chase) 
        {
            objectiveTransform = Creature.focusCreature.transform;
        } 
        else 
        {
            objectiveTransform = GameManager.Instance.homeBase.transform;
        }

        Vector2 directionToObjective = objectiveTransform.position - transform.position;
        float angleInRadians = Mathf.Atan2(directionToObjective.y, directionToObjective.x);
        float angleInDegrees = Mathf.Rad2Deg * angleInRadians;
        transform.rotation = Quaternion.Euler(0f, 0f, angleInDegrees);
    }
}
