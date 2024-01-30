using System.Collections;
using UnityEngine;

public class GraphicsManager : MonoBehaviour
{
    protected SaveManager saveScript;
    private PlayerSave playerData;

    [SerializeField] GameObject postProcessing;

    IEnumerator Start()
    {
        //attempt to load data
        saveScript = GetComponent<SaveManager>();

        // wait until SaveManager has loaded saves
        yield return new WaitUntil(() => saveScript.loadedData);

        try
        {
            //Attempt to load save
            playerData = saveScript.LoadSaveData();

            // update the graphics
            UpdateGraphics();
        }

        catch
        {
            //If no save loaded, use default
            Debug.Log("No save file detected");
            saveScript.SavePlayerData(playerData);
        }
    }

    private void UpdateGraphics()
    {
        // activate/deactivate post processing effects
        postProcessing.SetActive(playerData.enablePostProcessing);

        // update graphics quality
        switch (playerData.graphicsSetting)
        {
            case 0:
                QualitySettings.SetQualityLevel(0);
                break;
            case 1:
                QualitySettings.SetQualityLevel(1);
                break;
            case 2:
                QualitySettings.SetQualityLevel(2);
                break;
        }
    }
}
