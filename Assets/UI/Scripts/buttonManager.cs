using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security;
using UnityEditor.Rendering;
using TMPro;

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

    [SerializeField]
    private GameObject screenSpaceCanvas;
    [SerializeField]
    private Transform settingsScreen;
    [SerializeField]
    private Transform playScreen;
    [SerializeField]
    private TextMeshProUGUI tapToPlayTxt;
    private float playFontSize;

    private Transform targetPosition;

    private void Start()
    {
        cameraStartingTrans = Camera.main.transform;
        playFontSize = tapToPlayTxt.fontSize;
    }

    public void StartGame()
    {
        screenSpaceCanvas.SetActive(false);

        playButtonPressed = true;

        targetPosition = playScreen;
        zoomDistance = Vector3.Distance(cameraStartingTrans.position, playScreen.position);
        startTime = Time.time;

        zoomToScreen = true;
    }

    public void OpenSettings()
    {
        screenSpaceCanvas.SetActive(false);

        settingsButtonPressed = true;

        targetPosition = settingsScreen;
        zoomDistance = Vector3.Distance(cameraStartingTrans.position, settingsScreen.position);
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
            if (cameraStartingTrans.position == targetPosition.position)
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

                cameraStartingTrans.position = Vector3.Lerp(cameraStartingTrans.position, targetPosition.position, fractionOfDistance);
            }
        }

        playFontSize += Mathf.Sin(Time.timeSinceLevelLoad * 3.0f) * 0.03f;
        tapToPlayTxt.fontSize = playFontSize;
    }
}
