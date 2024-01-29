using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class consistentFOV : MonoBehaviour
{
    private float aspect;
    public float landscapeBaseFOV = 31f;
    public float portraitBaseFOV = 27.2f;

    Camera mainCamera;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        //AdjustFOV();
    }

    private void AdjustFOV(float baseFOV, float targetAspect)
    {
        float adjustedFOV = baseFOV * (targetAspect / aspect);

        mainCamera.fieldOfView = adjustedFOV;
    }

    private void Update()
    {
        // get the current aspect of the screen
        aspect = (float)Screen.width / Screen.height;

        // if it is wider than tall use landscape fov
        if(aspect > 1)
        {
            // adjusts the fov to keep scale correct
            AdjustFOV(landscapeBaseFOV, 16f/9f);
        } 
        // if it is within aspect 0.7 - 1.0 use a set fov
        else if (aspect > 0.7f)
        {
            mainCamera.fieldOfView = 27.2f;
        } 
        // if it is portait and less then 0.7 update fov
        else
        {
            AdjustFOV(portraitBaseFOV, 9f / 16f);
        }
    }
}
