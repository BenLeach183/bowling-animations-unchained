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
    private GameObject mainCamera;

    private bool zoomToScreen = false;
    private bool playButtonPressed = false;
    private bool settingsButtonPressed = false;

    private float startTime;
    private float zoomDistance;
    public float zoomSpeed;

    private Transform cameraStartingTrans;

    public void StartGame(GameObject m_button)
    {
        button = m_button;

        Button playButton = button.GetComponent<Button>();
        playButton.interactable = false;

        playButtonPressed = true;

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraStartingTrans = mainCamera.transform;
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

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraStartingTrans = mainCamera.transform;
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
            if (mainCamera.transform.position == button.transform.position)
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
                    SceneManager.LoadSceneAsync("SettingsMenu");
                }
            }
            else
            {
                float distanceCovered = (Time.time - startTime) * zoomSpeed;
                float fractionOfDistance = distanceCovered / zoomDistance;

                mainCamera.transform.position = Vector3.Lerp(cameraStartingTrans.position, button.transform.position, fractionOfDistance);
            }
        }
    }
}
