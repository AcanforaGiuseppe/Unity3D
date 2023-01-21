#define PIPPO

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScript : MonoBehaviour
{
    void Start()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.DisplayDialog("", "", "");
#endif

#if PIPPO
        Debug.Log("Ciao");
#endif
    }

    void Update()
    {

    }

}