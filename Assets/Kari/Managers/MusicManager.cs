using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] List<Track> SongList;

    int onSong = -1;
    [SerializeField] AudioSource[] audioSources;
    [SerializeField] AudioSource[] layerSources;

    AudioSource oldSource, newSource;
    string[] lines;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float length;
    float time;

    [Range(0,1)]
    [SerializeField]float maxAudio;
    // Start is called before the first frame update

    private void Awake()
    {
        SetInstance(this);
    }

    public static void NewSong()
    {
        Instance.onSong++;

        Instance.PlaySong();
        Debug.Log("newSong");
    }

    public static float layerFill;

    public static void SetTrack(int index)
    {
        Instance.onSong = index;
        Instance.PlaySong();
        Debug.Log("newSong");

    }

    public static void SetTrack(string trackName)
    {
        for (int i = 0; i < Instance.SongList.Count;i++)
            if (Instance.SongList[i].name == trackName)
            {
                Instance.onSong = i;
                Instance.PlaySong();
                Debug.Log("newSong");
            }
    }

    public void PlaySong()
    {
        SwitchSource();

        if (newSource == null)
            return;

        newSource.clip = SongList[onSong].clip;
        newSource.Play();

        for (int i = 0; i < SongList[onSong].layers.Length && i < layerSources.Length;i++)
        {
            layerSources[i].clip = SongList[onSong].layers[i];
            layerSources[i].Play();
            layerSources[i].volume = 0;
        }

        time = 0;
    }



    private void Update()
    {
        UpdateLayers(layerFill);

        if (time > length)
            return;

        time += Time.deltaTime;

        if (oldSource == null)
            return;


        oldSource.volume = Mathf.Lerp(0,maxAudio, 1 - curve.Evaluate(time / length));
        newSource.volume = Mathf.Lerp(0, maxAudio, curve.Evaluate(time / length));
    }

    void SwitchSource()
    {
        if (audioSources[1] == null)
            return;

        if (audioSources[1].volume > audioSources[0].volume)
        {
            oldSource = audioSources[1];
            newSource = audioSources[0];
        }
        else
        {
            oldSource = audioSources[0];
            newSource = audioSources[1];
        }
    }

    void UpdateLayers(float fill)
    {
        float layers = SongList[onSong].layers.Length;
        float amountPerLayer = 1.0f / layers;

        for (int i = 0; i < layerSources.Length; i++)
            layerSources[i].volume = Mathf.Clamp01(Mathf.InverseLerp(amountPerLayer * i, amountPerLayer * (i+1),fill));
    }
}

[System.Serializable]
public class Track
{
    public string name;
    public AudioClip clip;

    public AudioClip[] layers;

    
}
