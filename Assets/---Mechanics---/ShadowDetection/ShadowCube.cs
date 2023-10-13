using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ShadowCube : MonoBehaviour
{
    [SerializeField] RenderTexture shadowTexture;

    [Range(1,5)][Tooltip("Number Of Sample Points Within Shadow")]
    [SerializeField] int Threshhold = 5;

    public bool InShadow = false;

    Texture2D OutputTexture;
    Rect screenRect;
    Color[] Colors = new Color[5];

    [SerializeField] Color Average;

    void Start()
    {
        OutputTexture = new Texture2D(shadowTexture.width, shadowTexture.height, TextureFormat.RGB24, false);
        screenRect = new Rect(0, 0, shadowTexture.width, shadowTexture.height);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        RenderTexture.active = shadowTexture;
        OutputTexture.ReadPixels(screenRect,0,0);
        RenderTexture.active = null;


        ///1/
        //234
        ///5/

        int PointsInShadow = 0;
        int width = OutputTexture.width;
        int height = OutputTexture.height;


        //Reads Texture Colors at 5 Sample Points
        Colors[0] = OutputTexture.GetPixel(width/2, 0);
        Colors[1] = OutputTexture.GetPixel(0, height/2);
        Colors[2] = OutputTexture.GetPixel(width / 2, height / 2);
        Colors[3] = OutputTexture.GetPixel(width, height / 2);
        Colors[4] = OutputTexture.GetPixel(width / 2, height);

        //Count Amount of points in shadow
        foreach (var color in Colors)
        {
            if (color.r < .5f) PointsInShadow++;
        }

        InShadow = PointsInShadow >= Threshhold;
    }
}
