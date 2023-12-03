using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kari.SoundManagement;

public class UnityAnimatorEvents : MonoBehaviour
{
    AudioSource source;

    public void PlaySound(string name)
    {
        AudioManager.PlaySound(name, source);
    }
}
