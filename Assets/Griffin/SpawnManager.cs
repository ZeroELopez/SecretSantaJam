using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private SendPlayerBackToSpawn deathBar;

    // Start is called before the first frame update
    void Start()
    {
        deathBar = GetComponent<SendPlayerBackToSpawn>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (LowerbodyScript.state == PhysicsState.onGround) 
    //    {
    //        deathBar.SetSpawn();
    //    }
    //}
}
