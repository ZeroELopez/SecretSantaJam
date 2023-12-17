using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Base.Events;
using UnityEngine.UI;

public class CreateSnapshot : MonoBehaviour, ISubscribable<onCreatureCaptured>
{
    [SerializeField]Camera mainCamera;
    [SerializeField] RawImage image;

    [SerializeField] int resWidth;
    [SerializeField] int resHeight;


    //[SerializeField] Texture2D screenShot;
    public void HandleEvent(onCreatureCaptured evt)
    {
        Debug.Log("Creating Snapshot");
        //    if (Input.GetKeyDown("k"))
        //    {
        mainCamera = Camera.main;
        //.backgroundColor = Color.clear;

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        mainCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight,
                                             TextureFormat.RGB24, false);



        mainCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();
        mainCamera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors 
        //Destroy(rt);

        evt.page.image = screenShot;
        image.texture = screenShot;

        GameManager.newPages.Add(evt.page);
        //binder.Add(screenShot);

        //byte[] bytes = screenShot.EncodeToPNG();
        //string filename = Application.dataPath + "/screenshots/screen"
        //                + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        //System.IO.File.WriteAllBytes(filename, bytes);
        //Debug.Log(string.Format("Took screenshot to: {0}", filename));
        //    }
    }

    public void Subscribe()
    {
        EventHub.Instance.Subscribe<onCreatureCaptured>(this);
    }

    public void Unsubscribe()
    {
        EventHub.Instance.Unsubscribe<onCreatureCaptured>(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}
