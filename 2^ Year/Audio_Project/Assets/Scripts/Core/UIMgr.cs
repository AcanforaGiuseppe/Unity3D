using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    public GameObject InGameUI;
    public GameObject PauseUI;
    public Slider AudioVolume;
    public AudioMgr AudioManager;

    // Start is called before the first frame update
    void Start()
    {
        InGameUI.SetActive(true);
        PauseUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVolume()
    {
        AudioManager.SetAudioVolume("MasterVol", Mathf.Log10(AudioVolume.value) * 20);
    }

    public void SetSFXVolume()
    {
        AudioManager.SetAudioVolume("SFXVolume", Mathf.Log10(AudioVolume.value) * 20);
    }

    public void PauseGame()
    {
        InGameUI.SetActive(false);
        PauseUI.SetActive(true);
    }

    public void ResumeGame()
    {
        InGameUI.SetActive(true);
        PauseUI.SetActive(false);
    }
}
