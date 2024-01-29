using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security;
using UnityEditor.Rendering;

public class buttonManager : MonoBehaviour
{
    private GameObject button;

    private bool zoomToScreen = false;
    private bool playButtonPressed = false;
    private bool settingsButtonPressed = false;

    private float startTime;
    private float zoomDistance;
    public float zoomSpeed;

    private Transform cameraStartingTrans;

    private void Start()
    {
        cameraStartingTrans = Camera.main.transform;
    }

    public void StartGame(GameObject m_button)
    {
        button = m_button;

        Button playButton = button.GetComponent<Button>();
        playButton.interactable = false;

        playButtonPressed = true;

        zoomDistance = Vector3.Distance(cameraStartingTrans.position, button.transform.position);
        startTime = Time.time;

        zoomToScreen = true;
    }

    public void OpenSettings(GameObject m_button)
    {
        button = m_button;

        Button settingsButton = button.GetComponent<Button>();
        settingsButton.interactable = false;

        settingsButtonPressed = true;

        zoomDistance = Vector3.Distance(cameraStartingTrans.position, button.transform.position);
        startTime = Time.time;

        zoomToScreen = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void FixedUpdate()
    {
        if (zoomToScreen)
        {
            if (cameraStartingTrans.position == button.transform.position)
            {
                zoomToScreen = false;
                if (playButtonPressed)
                {
                    SceneManager.LoadScene(1);
                    playButtonPressed = false;
                }
                else if (settingsButtonPressed)
                {
                    //open settings menu
                    settingsButtonPressed = false;
                }
            }
            else
            {
                float distanceCovered = (Time.time - startTime) * zoomSpeed;
                float fractionOfDistance = distanceCovered / zoomDistance;

                cameraStartingTrans.position = Vector3.Lerp(cameraStartingTrans.position, button.transform.position, fractionOfDistance);
            }
        }
    }
}
