using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Script References
    private PlayerSave playerData;
    private SaveManager saveScript;

    //Arrays to be filled with music/audio
    protected GameObject[] arr_music;
    protected GameObject[] arr_SFX;

    //Used to cycle through and mute/unmute
    private AudioSource currentAudio;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        //Get Player Settings
        saveScript = GetComponent<SaveManager>();

        // wait until SaveManager has loaded saves
        yield return new WaitUntil(() => saveScript.loadedData);

        playerData = saveScript.LoadSaveData();

        //Mute Audio based on current settings
        if (playerData.muteMusic)
        {
            //Put all Music Audios into an array
            arr_music = GameObject.FindGameObjectsWithTag("Music");
            for (int i = 0; i < arr_music.Length; i++)
            {
                //Cycle through array and mute each one
                currentAudio = arr_music[i].GetComponent<AudioSource>();
                currentAudio.mute = true;
            }
        }

        //Mute SFX based on current settings
        if (playerData.muteSFX)
        {
            //Put all Music Audios into an array
            arr_SFX = GameObject.FindGameObjectsWithTag("SFX");
            for (int i = 0; i < arr_SFX.Length; i++)
            {
                //Cycle through array and mute each one
                currentAudio = arr_SFX[i].GetComponent<AudioSource>();
                currentAudio.mute = true;
            }
        }
    }

    //Switches state of music
    public void toggleMusic()
    {
        //Put all Music Audios into an array
        arr_music = GameObject.FindGameObjectsWithTag("Music");
        if (arr_music != null)
        {
            for (int i = 0; i < arr_music.Length; i++)
            {
                //Cycle through array and flip each one
                currentAudio = arr_music[i].GetComponent<AudioSource>();

                //Is it muted? Yes, unmute it. No, mute it. 
                currentAudio.mute = currentAudio.mute ? false : true;
            }
        }
        
    }

    //Switches state of SFX
    public void toggleSFX()
    {
        //Put all SFX Audios into an array
        arr_SFX = GameObject.FindGameObjectsWithTag("SFX");
        if (arr_SFX != null)
        {
            for (int i = 0; i < arr_SFX.Length; i++)
            {
                //Cycle through array and flip each one
                currentAudio = arr_SFX[i].GetComponent<AudioSource>();

                //Is it muted? Yes, unmute it. No, mute it. 
                currentAudio.mute = currentAudio.mute ? false : true;
            }
        }
    }

    //Used to play one SFX
    public void playOneSFX(int sourceIndex)
    {
        //Put all SFX Audios into an array
        arr_SFX = GameObject.FindGameObjectsWithTag("SFX");

        //Get Audio Source at sourceIndex
        currentAudio = arr_SFX[sourceIndex].GetComponent<AudioSource>();
        //Plays it once, can overlap
        currentAudio.PlayOneShot(currentAudio.clip);
    }

    public void playSFX(int sourceIndex)
    {
        //Put all SFX Audios into an array
        arr_SFX = GameObject.FindGameObjectsWithTag("SFX");

        //Get Audio Source at sourceIndex
        currentAudio = arr_SFX[sourceIndex].GetComponent<AudioSource>();
        //Plays it as normal
        currentAudio.Play();
    }

    public void stopSFX(int sourceIndex)
    {
        //Put all SFX Audios into an array
        arr_SFX = GameObject.FindGameObjectsWithTag("SFX");

        //Get Audio Source at sourceIndex
        currentAudio = arr_SFX[sourceIndex].GetComponent<AudioSource>();
        //Stops the audio
        currentAudio.Stop();
    }

}
