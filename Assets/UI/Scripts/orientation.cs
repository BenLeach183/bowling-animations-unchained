using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class orientation : MonoBehaviour
{
    public Canvas landscape;
    public Canvas portrait;

    private void Start()
    {
        Input.gyro.enabled = true;
        landscape.gameObject.SetActive(false);
        portrait.gameObject.SetActive(true);
    }

    private void Update()
    {
        Debug.Log(Input.gyro.attitude.eulerAngles.x);
        if (Input.gyro.attitude.z < 0)
        {
            if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            {
                landscape.gameObject.SetActive(false);
                portrait.gameObject.SetActive(true);
            }
            else //use gyro because when phone was on its back, it was going to landscape, this negates the y axis of gyro as it never gets above 1
            {
                landscape.gameObject.SetActive(true);
                portrait.gameObject.SetActive(false);
            }
        }
    }
}
