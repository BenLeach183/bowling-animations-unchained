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


public class PlayerSave
{
    public float highScoreSave;
}

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

    public GameObject settingsMenuLandsape;
    public GameObject settingsMenuPortrait;
    public GameObject darkenBackground;

    private bool inSettingsMenu = false;

    public GameObject Joystick, JoystickPoint;

    private bool withinBounds = false;

    private PlayerSave save;

    public float ScoreMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerRb = player.GetComponent<Rigidbody>();
        floorTrackObjects = GetComponent<ProceduralGeneration>().pooledTrackScripts;

        try
        {
            save = LoadSaveData();
        }
        catch
        {
            Debug.Log("Error");
            SavePlayerData(0);
            save = new PlayerSave() { highScoreSave = 0 };
        }

        highScore = save.highScoreSave;
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

        withinBounds = false;
        for (int i = 0; i < floorTrackObjects.Length; i++)
        {
            if ((player.transform.position - floorTrackObjects[i].totalBoundingSphere.GetPosition()).magnitude < floorTrackObjects[i].totalBoundingSphere.GetRadius())
            {
                withinBounds = true;
                OOBDeathTimer = 0;
                break;
            }
        }

        if (!withinBounds && !playerController.onTrack)
        {
            OOBDeathTimer += Time.deltaTime;
        }
        else
        {
            OOBDeathTimer = 0;
        }

        if (OOBDeathTimer > 3f)
        {
            EndGame();
            Debug.Log("End game OOB");
        }

        scoreText.text = Mathf.Round(score).ToString() + " X " + Mathf.Ceil(ScoreMultiplier).ToString();
    }

    void EndGame()
    {
        SavePlayerData(highScore);
        SceneManager.LoadScene(0);
    }

    public void SavePlayerData(float highScorein)
    {
        PlayerSave newPlayerSave = new PlayerSave()
        {
            highScoreSave = highScorein
        };

        string output = JsonConvert.SerializeObject(newPlayerSave);
        using (StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + "/PlayerSave.dat"))
        {
            streamWriter.Write(output);
        }
    }

    public PlayerSave LoadSaveData()
    {
        PlayerSave playerSave = new PlayerSave();
        using (StreamReader streamReader = new StreamReader(Application.persistentDataPath + "/PlayerSave.dat"))
        {
            playerSave = JsonConvert.DeserializeObject<PlayerSave>(streamReader.ReadToEnd());
        }
        return playerSave;
    }

    public void CloseSettingsMenu()
    {
        DeactivateSettingsMenu();
        darkenBackground.SetActive(false);
        Time.timeScale = 1;
        inSettingsMenu = false;
    }

    public void PauseGame()
    {
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
