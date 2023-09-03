using UnityEngine;
using UnityEngine.Playables;

// https://blog.unity.com/technology/extending-timeline-a-practical-guide

public class LightControlBehaviour : PlayableBehaviour
{
    public Color color = Color.white;
    public float intensity = 1f;
}