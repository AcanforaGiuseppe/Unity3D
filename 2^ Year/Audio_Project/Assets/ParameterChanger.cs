using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ParameterChanger : MonoBehaviour
{
    public StudioEventEmitter Emitter;

    public EventReference Event;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Emitter.SetParameter("Area", 50);
        }

    }
}
