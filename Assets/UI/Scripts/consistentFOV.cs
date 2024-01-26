using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class consistentFOV : MonoBehaviour
{
    public float baseFOV = 31f;

    Camera mainCamera;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        AdjustFOV();
    }

    private void AdjustFOV()
    {
        float targetAspect = 16f / 9f;
        float currentAspect = (float)Screen.width / Screen.height;

        float adjustedFOV = baseFOV * (targetAspect / currentAspect);

        mainCamera.fieldOfView = adjustedFOV;
    }

    private void Update()
    {
        AdjustFOV();    //This is on update for debug purposes REMOVE THIS after testing
    }
}
