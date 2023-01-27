using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionDetecto : MonoBehaviour
{
    GameObject player;
    RaycastHit hit;

    AudioLowPassFilter LPF;

    public int TransitionSpeed;

    bool toOcclude;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<AudioListener>().gameObject;
        LPF = GetComponent<AudioLowPassFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Linecast(transform.position, player.transform.position, out hit);

        if(hit.collider.tag == "Player")
        {
            toOcclude = false;
        }
        else
        {
            toOcclude = true;
        }

        if (toOcclude)
        {
            Occlude();
        }
        else
        {
            DeOcclude();
        }
    }

    private void CheckForOcclusion()
    {

    }

    private void DeOcclude()
    {
        if(LPF.cutoffFrequency < 22000)
        {
            LPF.cutoffFrequency += 1 * TransitionSpeed;
        }
    }

    void Occlude()
    {
        if (LPF.cutoffFrequency > 700)
        {
            LPF.cutoffFrequency -= 1 * TransitionSpeed;
        }
    }
}
