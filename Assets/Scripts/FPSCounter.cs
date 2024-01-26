using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    float t = 0;
    TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (t > 1)
        {
            t = 0;
            text.text = Mathf.Round((1.0f / Time.deltaTime)).ToString();
        }
        else
        {
            t += Time.deltaTime;
        }
    }
}
