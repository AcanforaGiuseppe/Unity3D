using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;

    Cinemachine3rdPersonFollow personFollow;

    void Start()
    {
        personFollow = vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
            vCam.m_Lens.FieldOfView += 1;

        if (Input.GetKey(KeyCode.Alpha2))
            vCam.m_Lens.FieldOfView -= 1;

        if (Input.GetKey(KeyCode.Alpha3))
            personFollow.CameraDistance = personFollow.CameraDistance + 0.1f * Time.deltaTime;

        if (Input.GetKey(KeyCode.Alpha4))
            personFollow.CameraDistance = personFollow.CameraDistance - 0.1f * Time.deltaTime;
    }

}