using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kari.SoundManagement;
using UnityEngine.SceneManagement;
using Assets.Scripts.Base.Events;
using System;
using UnityEngine.Events;


public class UnityAnimatorEvents : MonoBehaviour
{
    AudioSource source;
    public UnityEvent onLoadComplete;
    public string nextSceneName;

    private void Start()=>        source = GetComponent<AudioSource>();

    public void PlaySound(string soundName)=>        AudioManager.PlaySound(soundName, source);

    public void PlayMusic(string track) => MusicManager.SetTrack(track);

    public void CameraShake() => ObjectShake.Camera.Shake();


    public void Cutscene() => EventHub.Instance.PostEvent(new onCutsceneToggle() { On = true});
    public void EndCutscene() => EventHub.Instance.PostEvent(new onCutsceneToggle() { On = false });

    public void ReplayLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void Quit()=>        Application.Quit();
    public void LoadScene(string sceneName)=>        SceneManager.LoadScene(sceneName);

    public void NextScene() => SceneManager.LoadScene(nextSceneName);

    public void UnloadLevel() => SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);

    public void LoadSceneAsync(string sceneName)
    {
        var job = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
        job.completed += loadCompleted;
    }

    void loadCompleted(AsyncOperation job) => onLoadComplete?.Invoke();

    bool added = false;
    public void NextChapterSet()
    {
        if (added)
            return;

        LoadSceneScript.index++;
        added = true;

        LoadScene("Camp " + LoadSceneScript.index);
    }

    public void Footstep() => AudioManager.PlaySound("Runon" + LowerbodyScript.floorType, source, "RunonRock");

    public void AddPagesToEncyclopedia() => GameManager.AddPagesToEncyclopedia();

    public void PlayEffects()
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponentInChildren<ParticleSystem>().Play();
    }
}
