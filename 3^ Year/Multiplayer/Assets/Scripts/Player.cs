using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float angleSpeed = 150f;
    public float moveSpeed = 8f;

    void FixedUpdate()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * angleSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }
}