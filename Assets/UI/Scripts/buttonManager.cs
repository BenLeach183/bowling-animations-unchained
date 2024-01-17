using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class buttonManager : MonoBehaviour
{
    private bool fadeOut = false;

    private GameObject button;

    public void StartGame(GameObject m_button)
    {
        //Start the game
        button = m_button;
        fadeOut = true;
    }

    public void OpenSettings()
    {
        //open settings menu
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void FixedUpdate()
    {
        if (fadeOut)
        {
            Image buttonImage = button.GetComponent<Image>();
            Color newButtonColor = buttonImage.color;
            float newButtonAlpha = newButtonColor.a -= Time.deltaTime;
            newButtonColor.a = newButtonAlpha;
            buttonImage.color = newButtonColor;
            if (buttonImage.color.a == 0f)
            {
                fadeOut = false;
            }
        }
    }
}
