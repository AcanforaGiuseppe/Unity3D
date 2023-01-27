using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOcclusion : MonoBehaviour
{
    Ray r;
    RaycastHit hit;

    GameObject player;

    AudioLowPassFilter filter;
    public int OcclusionFrequency;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.playerInstance.gameObject;
        filter = gameObject.AddComponent<AudioLowPassFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rayDirection = (player.transform.position - transform.position) + new Vector3(0, 1, 0);

        r = new Ray(transform.position, rayDirection);
        Debug.DrawRay(transform.position, rayDirection, Color.yellow);

        if(Physics.Raycast(r, out hit))
        {
            if(hit.collider.tag == "Player")
            {
                filter.cutoffFrequency = 22000;
            }
            else if(hit.collider.tag != "Player")
            {
                filter.cutoffFrequency = OcclusionFrequency;
            }
        }
    }
}
