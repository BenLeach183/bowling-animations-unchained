using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class lightRotation : MonoBehaviour
{
    public float rotationSpeed = 1.0f;

    float t = 0;
    public float colorSwitchTime = 1.0f;

    int colorIndex = 0;

    Light currentLight;

    public Color[] colorArray;

    public bool fade = false;

    private void Start()
    {
        currentLight = GetComponent<Light>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        t += Time.deltaTime;

        if (t >= colorSwitchTime) 
        {
            t = 0;

            if (colorIndex >= colorArray.Length - 1)
            {
                colorIndex = 0;
            }
            else
            {
                colorIndex++;
            }

            if (fade)
            {
                StartCoroutine(FadeColor(currentLight, colorArray[colorIndex], colorSwitchTime));
            }
            else
            {
                currentLight.color = colorArray[colorIndex];
            }
        }
    }

    IEnumerator FadeColor(Light light, Color targetColor, float duration)
    {
        float elapsedTime = 0;
        Color startColor = light.color;

        while (elapsedTime < duration)
        {
            light.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        light.color = targetColor;
    }


}
