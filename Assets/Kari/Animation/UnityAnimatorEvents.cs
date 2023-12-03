using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kari.SoundManagement;
using UnityEngine.SceneManagement;

public class UnityAnimatorEvents : MonoBehaviour
{
    AudioSource source;

    public void PlaySound(string name)
    {
        AudioManager.PlaySound(name, source);
    }

    public void ReplayLevel(string name)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
