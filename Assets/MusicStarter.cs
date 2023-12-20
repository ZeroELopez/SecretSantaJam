using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    [SerializeField] string trackName;

    // Start is called before the first frame update
    void Start()
    {
        MusicManager.SetTrack(trackName);
    }
}
