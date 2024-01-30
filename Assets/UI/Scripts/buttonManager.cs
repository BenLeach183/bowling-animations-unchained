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

    private AudioManager audioScript;

    private bool zoomToScreen = false;
    private bool playButtonPressed = false;
    private bool settingsButtonPressed = false;

    private float startTime;
    private float zoomDistance;
    public float zoomSpeed;

    private Transform cameraStartingTrans;

    protected SaveManager saveScript;
    [SerializeField] private GameObject screenSpaceCanvas;
    [SerializeField] private Transform settingsScreen;
    [SerializeField] private Transform playScreen;
    [SerializeField] private TextMeshProUGUI tapToPlayTxt;
    [SerializeField] private TextMeshPro highscoreTxtTV;
    [SerializeField] private TextMeshProUGUI highscoreTxtScreen;
    private float playFontSize;

    private Transform targetPosition;

    private PlayerSave playerData;

    IEnumerator Start()
    {
        
        cameraStartingTrans = Camera.main.transform;
        playFontSize = tapToPlayTxt.fontSize;
        audioScript = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        //attempt to load data
        saveScript = GetComponent<SaveManager>();

        // wait until SaveManager has loaded saves
        yield return new WaitUntil(() => saveScript.loadedData);

        try
        {
            //Attempt to load save
            playerData = saveScript.LoadSaveData();
            // update the highscore
            highscoreTxtTV.text = "Highscore\n\n" + Mathf.Floor(playerData.highScoreSave).ToString();
            highscoreTxtScreen.text = Mathf.Floor(playerData.highScoreSave).ToString();
        }

        catch
        {
            //If no save loaded, use default
            Debug.Log("No save file detected");
            saveScript.SavePlayerData(playerData);
        }
    }

    public void StartGame()
    {
        screenSpaceCanvas.SetActive(false);

        playButtonPressed = true;
        audioScript.playOneSFX(0);

        targetPosition = playScreen;
        zoomDistance = Vector3.Distance(cameraStartingTrans.position, playScreen.position);
        startTime = Time.time;

        zoomToScreen = true;
    }

    public void OpenSettings()
    {
        screenSpaceCanvas.SetActive(false);
        audioScript.playOneSFX(0);
        settingsButtonPressed = true;

        targetPosition = settingsScreen;
        zoomDistance = Vector3.Distance(cameraStartingTrans.position, settingsScreen.position);
        startTime = Time.time;

        zoomToScreen = true;
    }

    public void QuitGame()
    {
        audioScript.playOneSFX(0);
        Application.Quit();
    }

    private void FixedUpdate()
    {
        if (zoomToScreen)
        {
            if ((cameraStartingTrans.position - targetPosition.position).sqrMagnitude <= 0.02f)
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

        playFontSize += Mathf.Sin(Time.timeSinceLevelLoad * 3.0f) * 0.1f;
        tapToPlayTxt.fontSize = playFontSize;
    }
}
