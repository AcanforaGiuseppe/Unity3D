using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MyDataObject : ScriptableObject
{
    //public bool b;
    //public int i;
    //[Range(0.0f, 1.0f)]
    //public float f;
    [System.Serializable]
    public struct MinMax
    {
        public int min;
        public int max;
    }

    public Vector2 widthHeight;
    public Vector3 widthHeightDepth;
    public MinMax range;
}