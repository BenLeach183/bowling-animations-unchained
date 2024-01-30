using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class PlayerSave
{
    public float highScoreSave = 0;
    public int controllerModes = 0; //0 = Tap, 1 = Joystick, 2 = Tilt
    public bool muteMusic = false;
    public bool muteSFX = false;
    public bool enablePostProcessing = true;
    public byte graphicsSetting = 0;
}

public class SaveManager : MonoBehaviour
{
    public PlayerSave playerSave = new PlayerSave();

    public bool loadedData = false;

    // Start is called before the first frame update
    void Start()
    {
        
        try
        {
            playerSave = LoadSaveData();
        }
        catch
        {
            Debug.Log("Error");
            SavePlayerData(playerSave);
            //save = new PlayerSave();
        }

        loadedData = true;
    }

    public void SavePlayerData(PlayerSave save)
    {
        PlayerSave newPlayerSave = save;

        string output = JsonConvert.SerializeObject(newPlayerSave);
        using (StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + "/PlayerSave.dat"))
        {
            streamWriter.Write(output);
        }
    }

    public PlayerSave LoadSaveData()
    {
        PlayerSave playerSave = new PlayerSave();
        try 
        {
            using (StreamReader streamReader = new StreamReader(Application.persistentDataPath + "/PlayerSave.dat"))
            {
                playerSave = JsonConvert.DeserializeObject<PlayerSave>(streamReader.ReadToEnd());
            }
            return playerSave;
        } 
        catch { return playerSave; }
        
    }

    public void Save()
    {
        SavePlayerData(playerSave);
    }
}
