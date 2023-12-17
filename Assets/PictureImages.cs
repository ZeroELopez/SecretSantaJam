using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PictureImages : MonoBehaviour
{
    public static Action<RawImage[]> setPhotos;
    public RawImage[] images;
    // Start is called before the first frame update
    void Start()=>        setPhotos?.Invoke(images);

}
