using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlaySound : MonoBehaviour
{
    public AudioMixer Mixer;
    float sliderValue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Mixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
    }
}
