using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using FMODUnity;
public class AudioMgr : MonoBehaviour
{
    public AudioMixer MasterMixer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSnapshot(string snapshotName, float timeToReach)
    {
        MasterMixer.FindSnapshot(snapshotName).TransitionTo(timeToReach);
    }

    public void SetAudioVolume(string parameterName, float valueToChange)
    {
        MasterMixer.SetFloat(parameterName, valueToChange);
    }
}
