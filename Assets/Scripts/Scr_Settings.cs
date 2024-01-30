using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    protected SaveManager saveScript;
    protected AudioManager audioScript;
    private PlayerSave playerData;

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
    [SerializeField] private TextMeshProUGUI Txt_Controls;
    [SerializeField] private TextMeshProUGUI Txt_Music;
    [SerializeField] private TextMeshProUGUI Txt_SFX;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        //attempt to load data
        saveScript = GetComponent<SaveManager>();
        audioScript = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        // wait until SaveManager has loaded saves
        yield return new WaitUntil(() => saveScript.loadedData);

        try
        {
            //Attempt to load save
            playerData = saveScript.LoadSaveData();
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
        Txt_Controls.gameObject.SetActive(true);
        Txt_Music.gameObject.SetActive(true);
        Txt_SFX.gameObject.SetActive(true);

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
                Txt_Controls.text = "Controls: Tap";
                break;

            case 1:
                Btn_Joystick.gameObject.SetActive(true);
                Txt_Controls.text = "Controls: Joystick";
                break;

            case 2:
                Btn_Tilt.gameObject.SetActive(true);
                Txt_Controls.text = "Controls: Tilt";
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

    //SFX button Pressed
    public void sfxClick()
    {
        //Click noise
        audioScript.playOneSFX(0);

        if (Btn_muted_SFX.IsActive())
        {
            //Switch Button to unmuted
            Btn_muted_SFX.gameObject.SetActive(false);
            Btn_Unmuted_SFX.gameObject.SetActive(true);

            //Update New Setting
            playerData.muteSFX = false;

            //unmute SFX
            audioScript.toggleSFX();
        }
        else if (Btn_Unmuted_SFX.IsActive())
        {
            //Switch Button to muted
            Btn_Unmuted_SFX.gameObject.SetActive(false);
            Btn_muted_SFX.gameObject.SetActive(true);

            //Update New Setting
            playerData.muteSFX = true;

            //mute sfx
            audioScript.toggleSFX();
        }

        //save settings
        saveScript.SavePlayerData(playerData);
    }

    //Music button Pressed
    public void musicClick()
    {
        //Click noise
        audioScript.playOneSFX(0);

        if (Btn_muted_Music.IsActive())
        {
            //Switch Button Icon
            Btn_muted_Music.gameObject.SetActive(false);
            Btn_Unmuted_Music.gameObject.SetActive(true);

            //Update New Setting
            playerData.muteMusic = false;

            //unmute Music
            audioScript.toggleMusic();
        }
        else if (Btn_Unmuted_Music.IsActive())
        {
            //Switch Button Icon
            Btn_Unmuted_Music.gameObject.SetActive(false);
            Btn_muted_Music.gameObject.SetActive(true);

            //Update New Setting
            playerData.muteMusic = true;

            //mute Music
            audioScript.toggleMusic();
        }

        //save settings
        saveScript.SavePlayerData(playerData);
    }

    //Post Processing Pressed
    public void postProcessingClick()
    {
        //Click noise
        audioScript.playOneSFX(0);

        //Toggles if post processing is enabled or not
        if (playerData.enablePostProcessing)
        {
            playerData.enablePostProcessing = false;
            Txt_Post_Processing.text = "Disabled Post \nProcessing";
        } else
        {
            playerData.enablePostProcessing = true;
            Txt_Post_Processing.text = "Enabled Post \nProcessing";
        }

        //save settings
        saveScript.SavePlayerData(playerData);
    }

    //Controls Pressed
    public void controlsClick()
    //0 = Tap, 1 = Joystick, 2 = Tilt
    {
        //Click noise
        audioScript.playOneSFX(0);

        if (Btn_Tap.IsActive())
        {
            //Switch to Tilt
            Btn_Tap.gameObject.SetActive(false);
            Btn_Tilt.gameObject.SetActive(true);

            //Update Text
            Txt_Controls.text = "Controls: Tilt";

            //Update new Setting
            playerData.controllerModes = 2;
        } 
        else if (Btn_Tilt.IsActive())
        {
            //Switch to Joystick
            Btn_Tilt.gameObject.SetActive(false);
            Btn_Joystick.gameObject.SetActive(true);

            //Update Text
            Txt_Controls.text = "Controls: Joystick";

            //Update new Setting
            playerData.controllerModes = 1;
        }
        else if (Btn_Joystick.IsActive())
        {
            //Switch to Tap
            Btn_Joystick.gameObject.SetActive(false);
            Btn_Tap.gameObject.SetActive(true);

            //Update Text
            Txt_Controls.text = "Controls: Tap";

            //Save new Setting
            playerData.controllerModes = 0;
        }

        //Save new option
        saveScript.SavePlayerData(playerData);
    }


    //Home Pressed
    public void homeClick()
    {
        //Click noise
        audioScript.playOneSFX(0);

        //Returns to main menu
        SceneManager.LoadSceneAsync("MainMenuAdjusted");
    }
    
}
