using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [Header("Following Stuff")]
    public Vector3 CameraOffset;
    [Tooltip("Set the value from 0 to 1")]
    public float InterpolationRatio;
    

    GameObject targetToFollow;
    GameObject player;

    //--------------------------------------MAIN METHODS-------------------------------------------

    void Start()
    {
        targetToFollow = GameManager.playerInstance.Head;
        player = GameManager.playerInstance.gameObject;
    }

    void Update()
    {
        //FOLLOW THE PLAYER
        transform.position = Vector3.Lerp(transform.position, targetToFollow.transform.position + CameraOffset, InterpolationRatio);
        
        CalculateRotation();
    }

    //--------------------------------------OTHER METHODS------------------------------------------

    void CalculateRotation()
    {
        //ROTATE CAMERA
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, player.transform.localEulerAngles.y, 0);
    }
}
