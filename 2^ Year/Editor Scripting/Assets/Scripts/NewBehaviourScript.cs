using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Start()
    {

    }

#if UNITY_EDITOR
    void OnGUI()
    {
        GUILayout.Box(Time.time.ToString("0.00"));
        //GUI.Box(new Rect(0, 0, 300, 100), "Ciao");
    }
#endif

    void Update()
    {

    }

}