using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryScript : MonoBehaviour
{
    [SerializeField] private List<Sprite> Gallery;
    [SerializeField] private Image GalleryObj;
    private int CurrentImage = 0;

    public void RightArrowClick()
    {
        if (CurrentImage + 1 < Gallery.Count)
        {
            CurrentImage++;
        }
        else
        {
            CurrentImage = 0;
        }
        GalleryObj.sprite = Gallery[CurrentImage];
    }

    public void LeftArrowClick()
    {
        if (CurrentImage - 1 >= 0)
        {
            CurrentImage--; 
        }
        else
        {
            CurrentImage = Gallery.Count-1;
        }
        GalleryObj.sprite = Gallery[CurrentImage];

    }
}
