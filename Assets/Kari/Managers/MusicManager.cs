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
    AudioSource oldSource, newSource;
    string[] lines;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float length;
    float time;
    // Start is called before the first frame update

    private void Awake()
    {
        SetInstance(this);
    }

    public static void NewSong()
    {
        Instance.onSong++;

        Instance.PlaySong();
    }

    public static void SetTrack(int index)
    {
        Instance.onSong = index;
        Instance.PlaySong();
    }

    public static void SetTrack(string trackName)
    {
        for (int i = 0; i < Instance.SongList.Count;i++)
            if (Instance.SongList[i].name == trackName)
            {
                Instance.onSong = i;
                Instance.PlaySong();
            }
    }

    public void PlaySong()
    {
        SwitchSource();

        if (newSource == null)
            return;

        newSource.clip = SongList[onSong].clip;
        newSource.Play();
        time = 0;
    }



    private void Update()
    {
        if (time > length)
            return;

        time += Time.deltaTime;

        if (oldSource == null)
            return;

        oldSource.volume =  1 - curve.Evaluate(time / length);
        newSource.volume = curve.Evaluate(time / length);
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
}

[System.Serializable]
public class Track
{
    public string name;
    public AudioClip clip;
}
