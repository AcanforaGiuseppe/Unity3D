using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoSomethingScript : MonoBehaviour
{
    [ContextMenu("Do Something")]
    public void DoSomething()
    {
        Debug.Log("Do Something");
    }

}