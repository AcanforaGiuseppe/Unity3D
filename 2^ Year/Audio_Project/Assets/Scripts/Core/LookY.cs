using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookY : MonoBehaviour
{
    [Header("Rotation")]
    public float XRotationSpeed;
    public bool InvertedAxis;

    float rotationInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //GET THE MOUSE INPUT
        rotationInput = Input.GetAxis("Mouse Y");
        if (InvertedAxis)
            rotationInput *= -1;
        Vector3 rotation = new Vector3(rotationInput, 0, 0);

        //ROTATE CAMERA
        transform.Rotate(rotation * XRotationSpeed * Time.deltaTime);
    }
}
