// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioManager : MonoBehaviour
{
    public float aspectRatio = 16f / 9f;

    void Start()
    {
        AdjustAspectRatio();
    }

    void AdjustAspectRatio()
    {
        float screenAspectRatio = (float)Screen.width / Screen.height;
        float scale = screenAspectRatio / aspectRatio;

        Camera camera = GetComponent<Camera>();

        if (scale < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scale;
            rect.x = 0;
            rect.y = (1.0f - scale) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            float scalewidth = 1.0f / scale;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
