using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occlusion : MonoBehaviour
{
    public int OcclusionFrequency;
    public float FadeTime;

    GameObject player;

    AudioSource source;
    AudioLowPassFilter filter;

    Ray r;
    RaycastHit hit;

    bool isOccluded;

    float valueToAdd;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.playerInstance.gameObject;
        source = GetComponent<AudioSource>();

        filter = gameObject.AddComponent<AudioLowPassFilter>();

        valueToAdd = (22000 - OcclusionFrequency) / FadeTime;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rayDirection = (player.transform.position - transform.position) + new Vector3(0, 1, 0);
        r = new Ray(transform.position, rayDirection);

        if (Physics.Raycast(r, out hit))
        {
            if (hit.collider.tag == "Player")
            {
                DeOcclude();
            }
            else if (hit.collider.tag != "Player")
            {
                Occlude();
            }
        }
    }

    void Occlude()
    {
        if (filter.cutoffFrequency > OcclusionFrequency)
        {
            filter.cutoffFrequency -= valueToAdd * Time.deltaTime;
            if(filter.cutoffFrequency < OcclusionFrequency)
            {
                filter.cutoffFrequency = OcclusionFrequency;
            }
        }
    }

    void DeOcclude()
    {
        if (filter.cutoffFrequency < 22000)
        {
            filter.cutoffFrequency += valueToAdd * Time.deltaTime;
        }
    }
}
