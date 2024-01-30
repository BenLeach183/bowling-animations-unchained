using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class consistentFOV : MonoBehaviour
{
    private float aspect;
    private float previousAspect = 0;

    public float landscapeBaseFOV = 31f;
    public float portraitBaseFOV = 27.2f;

    [SerializeField] GameObject highScorePortrait;

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

        if (previousAspect != aspect) UpdateScreen();

        previousAspect = aspect;
    }

    private void UpdateScreen()
    {
        // if it is wider than tall use landscape fov
        if (aspect > 1)
        {
            // adjusts the fov to keep scale correct
            AdjustFOV(landscapeBaseFOV, 16f / 9f);
            highScorePortrait.SetActive(false);
        }
        // if it is within aspect 0.7 - 1.0 use a set fov
        else if (aspect > 0.7f)
        {
            mainCamera.fieldOfView = 27.2f;
            highScorePortrait.SetActive(true);
        }
        // if it is portait and less then 0.7 update fov
        else
        {
            AdjustFOV(portraitBaseFOV, 9f / 16f);
            highScorePortrait.SetActive(true);
        }
    }
}
