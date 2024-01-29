using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private SaveManager saveScript;
    private PlayerSave playerData;

    private bool isLoaded = false;

    [SerializeField] private Button Btn_muted_Music;
    [SerializeField] private Button Btn_muted_SFX;
    [SerializeField] private Button Btn_Unmuted_Music;
    [SerializeField] private Button Btn_Unmuted_SFX;
    [SerializeField] private Button Btn_Post_Processing;
    [SerializeField] private Button Btn_Tap;
    [SerializeField] private Button Btn_Tilt;
    [SerializeField] private Button Btn_Joystick;
    [SerializeField] private Button Btn_Home;
    [SerializeField] private TextMeshProUGUI Txt_Post_Processing;
    [SerializeField] private TextMeshProUGUI Txt_Loading;
    [SerializeField] private TextMeshProUGUI Txt_Highscore;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        //attempt to load data
        SaveManager saveScript = GetComponent<SaveManager>();

        // wait until SaveManager has loaded saves
        yield return new WaitUntil(() => saveScript.loadedData);

        try
        {
            //Attempt to load save
            playerData = saveScript.LoadSaveData();
            isLoaded = true;
            enableButtons(playerData);
        }

        catch
        {
            //If no save loaded, use default
            Debug.Log("No save file detected");
            saveScript.SavePlayerData(playerData);
            enableButtons(playerData);
        }
    }

    private void enableButtons(PlayerSave playerData)
    {
        //Remove Loading Text
        Txt_Loading.gameObject.SetActive(false);

        //These will always be enabled
        Btn_Home.gameObject.SetActive(true);
        Btn_Post_Processing.gameObject.SetActive(true);

        //Setting Highscore
        string highscore = "Highscore! \r\n" + playerData.highScoreSave.ToString();
        Txt_Highscore.text = highscore;
        Txt_Highscore.gameObject.SetActive(true);


        //Check if music was muted
        if (playerData.muteMusic)
        {
            Btn_muted_Music.gameObject.SetActive(true);
        } 
        else
        {
            Btn_Unmuted_Music.gameObject.SetActive(true);
        }

        //Check if SFX was muted
        if (playerData.muteSFX)
        {
            Btn_muted_SFX.gameObject.SetActive(true);
        } 
        else
        {
            Btn_Unmuted_SFX.gameObject.SetActive(true);
        }

        //Controller Mode: 0 = Tap, 1 = Joystick, 2 = Tilt
        switch (playerData.controllerModes)
        {
            case 0:
                Btn_Tap.gameObject.SetActive(true);
                break;

            case 1:
                Btn_Joystick.gameObject.SetActive(true); 
                break;

            case 2:
                Btn_Tilt.gameObject.SetActive(true); 
                break;
        }

        //Switch text between enable or 
        if (playerData.enablePostProcessing)
        {
            Txt_Post_Processing.text = "Enabled Post \nProcessing";
        } else
        {
            Txt_Post_Processing.text = "Disabled Post \nProcessing";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
