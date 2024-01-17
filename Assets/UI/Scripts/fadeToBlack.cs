using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class fadeToBlack : MonoBehaviour
{
    private GameObject fadeImageObject;

    private Image fadeImage;

    public float fadeSpeed = 1.0f;

    private bool fading = false;

    public void FadeOut(GameObject m_fadeImageObject)
    {
        fadeImageObject = m_fadeImageObject;
        fadeImage = fadeImageObject.GetComponent<Image>();
        fading = true;
    }

    void Update()
    {
        if (fading)
        {
            Debug.Log(fadeImage.color.a);
            Color newFadeColor = fadeImage.color;
            float newFadeAlpha = newFadeColor.a + (fadeSpeed * Time.deltaTime);
            newFadeColor.a = newFadeAlpha;
            fadeImage.color = newFadeColor;

            if (fadeImage.color.a >= 1.0f)
            {
                Debug.Log("Open new scene");
                SceneManager.LoadScene("SampleScene");
                fading = false;
            }
        }
    }
}

