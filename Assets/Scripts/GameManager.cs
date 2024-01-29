using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Data;
using RDG;

public class GameManager : MonoBehaviour
{
    private float score;
    private float highScore;
    private float OOBDeathTimer;
    private float stuckDeathTimer;

    private float aspectRatio = 1.0f;
    private byte orientation = 0;
    private byte oldOrientation;

    private GameObject player;
    private PlayerController playerController;
    private Rigidbody playerRb;
    private FloorTrackObject[] floorTrackObjects;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI multiplierText;
    public GameObject multiplierObject;

    public GameObject settingsMenuLandsape;
    public GameObject settingsMenuPortrait;
    public GameObject darkenBackground;

    private bool inSettingsMenu = false;

    public GameObject Joystick, JoystickPoint;

    private bool withinBounds = false;

    public float ScoreMultiplier = 1;

    private ProceduralGeneration proceduralGenerationScript;

    private SaveManager saveManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerRb = player.GetComponent<Rigidbody>();
        floorTrackObjects = GetComponent<ProceduralGeneration>().pooledTrackScripts;
        proceduralGenerationScript = GetComponent<ProceduralGeneration>();
        saveManager = GetComponent<SaveManager>();

        highScore = saveManager.playerSave.highScoreSave;
        highScoreText.text = "High Score: " + Mathf.Round(highScore).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        aspectRatio = (float)Screen.width / (float)Screen.height;
        oldOrientation = orientation;
        
        if(aspectRatio > 1)
        {
            orientation = 1;
        } else { orientation = 0; }

        // if the orientation has changed
        if(orientation != oldOrientation)
        {
            // update settings menu
            if (inSettingsMenu) { UpdateSettingsOrientation(); }
        }
        

        if(playerController.JoystickPivot != Vector2.zero){
            Joystick.SetActive(true);
            Joystick.transform.position = playerController.JoystickPivot * 100;
            JoystickPoint.transform.position = (playerController.InputVector * Joystick.transform.localScale.x * 30) + (playerController.JoystickPivot * 100);
        }
        else{
            Joystick.SetActive(false);
        }

        if(ScoreMultiplier > 1){
            ScoreMultiplier -= (Time.deltaTime/5);
        }
        else{
            ScoreMultiplier = 1;
        }

        floorTrackObjects = GetComponent<ProceduralGeneration>().pooledTrackScripts;

        if (playerRb.velocity.magnitude > 0.1f)
        {
            score += Vector3.Scale(playerRb.velocity, playerController.CurrentMoveDirection).magnitude * ScoreMultiplier * Time.deltaTime;
            stuckDeathTimer = 0;
        }
        else
        {
            if (stuckDeathTimer > 5.0f)
            {
                EndGame();
            }
            else
            {
                stuckDeathTimer += Time.deltaTime;
            }
        }

        if (score > highScore)
        {
            highScore = score;
        }

        // check if the player has fallen
        withinBounds = false;

        // get the track object player was last on
        FloorTrackObject currentTrack = proceduralGenerationScript.pooledTrackScripts[proceduralGenerationScript.onTrackID];

        // get the distance from current track bounding sphere
        Vector3 offsetFromTrack = player.transform.position - currentTrack.totalBoundingSphere.GetPosition();
        float distanceFromTrackSqrd = offsetFromTrack.sqrMagnitude;

        // if the player has left the current tracks bounding sphere they have lost
        if (distanceFromTrackSqrd < currentTrack.totalBoundingSphere.GetRadiusSqr())
        {
            withinBounds = true;
            OOBDeathTimer = 0;
        }

        if (!withinBounds && !playerController.onTrack)
        {
            OOBDeathTimer += Time.deltaTime;
        }
        else
        {
            OOBDeathTimer = 0;
        }

        if (OOBDeathTimer > 0.5f)
        {
            EndGame();
            Debug.Log("End game OOB");
        }

        scoreText.text = Mathf.Round(score).ToString();

        int multipler = (int)Mathf.Ceil(ScoreMultiplier);
        multiplierText.text = "x" + multipler.ToString();

        if (multipler > 1)
        {
            multiplierObject.SetActive(true);
        } else
        {
            multiplierObject.SetActive(false);
        }
    }

    void EndGame()
    {
        if(score > saveManager.playerSave.highScoreSave)
        {
            saveManager.playerSave.highScoreSave = score;
        }

        Vibration.Vibrate(1000);

        saveManager.Save();
        SceneManager.LoadScene(0);
    }

    public void CloseSettingsMenu()
    {
        DeactivateSettingsMenu();
        Vibration.Vibrate(100);
        darkenBackground.SetActive(false);
        Time.timeScale = 1;
        inSettingsMenu = false;
    }

    public void PauseGame()
    {
        Vibration.Vibrate(100);
        Time.timeScale = 0;

        darkenBackground.SetActive(true);

        inSettingsMenu = true;
        UpdateSettingsOrientation();
    }

    private void DeactivateSettingsMenu()
    {
        settingsMenuLandsape.SetActive(false);
        settingsMenuPortrait.SetActive(false);
    }

    private void UpdateSettingsOrientation()
    {
        if (orientation >= 1)
        {
            settingsMenuLandsape.SetActive(true);
            settingsMenuPortrait.SetActive(false);
        }
        else
        {
            settingsMenuLandsape.SetActive(false);
            settingsMenuPortrait.SetActive(true);
        }
    }

}
