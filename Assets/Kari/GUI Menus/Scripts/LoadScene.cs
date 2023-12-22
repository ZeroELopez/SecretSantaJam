using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LoadSceneScript : MonoBehaviour
{
    public float delayStart = 10;

    [SerializeField] string[] scenes;
    public static int index;
    [SerializeField] int showIndex;
    // Start is called before the first frame update

    public UnityEvent onCompleted;

    void Start()
    {
        StartCoroutine("Load");
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(delayStart);

        showIndex = index;

        if (index >= scenes.Length)
            index = 0;

        var job = SceneManager.LoadSceneAsync(scenes[index], LoadSceneMode.Additive);

        job.completed += Completed;
    }

    void Completed(AsyncOperation job)=>        onCompleted?.Invoke();
}
