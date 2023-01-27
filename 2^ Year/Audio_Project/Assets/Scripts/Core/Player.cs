using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Generic Stuff")]
    public float HP;
    [Header("Movement Stuff")]
    public float WalkingSpeed;
    public float RunningSpeed;
    public float MinSpeedTreshold;
    public float Acceleration;
    public float Deceleration;
    public float YRotationSpeed;
    [Header("Gravity Stuff")]
    public float Gravity;
    public float GravityRecovery;
    [Header("Jump Stuff")]
    public float JumpForce;
    [Header("Body Parts")]
    public GameObject Head;
    [Header("Animation Stuff")]
    public Animator PlayerAnimator;
    [Header("Gear")]
    public GameObject Torch;
    
    public bool isMoving { get; protected set; }
    public bool isRunning { get; protected set; }
    public bool isJumping { get; protected set; }

    CharacterController controller;
    float horizontalInput;
    float verticalInput;
    float rotationInput;
    float movementSpeed;
    float actualGravity;
    bool isInAir;
    bool isTorchActive;

    //--------------------------------------MAIN METHODS-------------------------------------------

    private void Awake()
    {
        GameManager.playerInstance = this;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        actualGravity = Gravity;
    }

    void Update()
    {
        CheckFallingState();

        CalculateMovement();
        CalculateRotation();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            TorchOnOff();
        }


        CheckingGravity();

        AnimatorUpdate();
    }

    //--------------------------------------OTHER METHODS------------------------------------------

    void CalculateMovement()
    {
        //GET THE INPUT VALUES
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //CHECK IF IS MOVING
        if(horizontalInput != 0 || verticalInput != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //CREATE THE DIRECTION VECTOR AND NORMALIZE
        Vector3 direction;
        direction = new Vector3(horizontalInput, 0, verticalInput);

        if (horizontalInput != 0 && verticalInput != 0)
        {
            direction.Normalize();
        }

        direction = transform.transform.TransformDirection(direction);

        //SET THE RIGHT MOVEMENT SPEED
        if (isMoving)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;

                if (movementSpeed < RunningSpeed)
                {
                    movementSpeed += Acceleration * Time.deltaTime;
                }
                else if (movementSpeed > RunningSpeed)
                {
                    movementSpeed -= Deceleration * Time.deltaTime;
                }
            }
            else if (!Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = false;

                if (movementSpeed < MinSpeedTreshold)
                {
                    movementSpeed = MinSpeedTreshold;
                }
                else if (movementSpeed < WalkingSpeed)
                {
                    movementSpeed += Acceleration * Time.deltaTime;
                }
                else if (movementSpeed > WalkingSpeed)
                {
                    movementSpeed -= Deceleration * Time.deltaTime;
                }
            }
        }
        else
        {
            if(movementSpeed > 0)
            {
                movementSpeed -= Deceleration * Time.deltaTime;
            }
            else if(movementSpeed < 0)
            {
                movementSpeed = 0;
            }
        }

        //MOVE
        controller.Move(direction * movementSpeed * Time.deltaTime);

        //APPLY GRAVITY
        controller.Move(Vector3.down * actualGravity * Time.deltaTime);
    }

    void CalculateRotation()
    {
        //GET INPUT
        rotationInput = Input.GetAxis("Mouse X");
        Vector3 rotation = new Vector3(0, rotationInput * YRotationSpeed * Time.deltaTime, 0);

        //ROTATE PLAYER
        transform.Rotate(rotation);
    }

    void CheckingGravity()
    {
        if(actualGravity < Gravity)
        {
            actualGravity += GravityRecovery * Time.deltaTime;
        }
    }

    void CheckFallingState()
    {
        if (!controller.isGrounded && !isInAir && !isJumping)
        {
            isInAir = true;
            actualGravity = 0;
        }
        else if (!controller.isGrounded && !isInAir && isJumping)
        {
            isInAir = true;
        }

        if (controller.isGrounded && isInAir)
        {
            isInAir = false;

            if (isJumping)
            {
                isJumping = false;
            }
        }
    }

    void Jump()
    {
        if (controller.isGrounded)
        {
            actualGravity = -JumpForce;
            isJumping = true;
        }
    }
    
    void AnimatorUpdate()
    {
        if (isMoving)
        {
            PlayerAnimator.SetBool("MovingForward", true);
        }
        else
        {
            PlayerAnimator.SetBool("MovingForward", false);
        }

        if (isRunning)
        {
            PlayerAnimator.SetBool("Running", true);
        }
        else
        {
            PlayerAnimator.SetBool("Running", false);
        }
    }

    void TorchOnOff()
    {
        if (isTorchActive)
        {
            Torch.SetActive(false);
            isTorchActive = false;
        }
        else
        {
            Torch.SetActive(true);
            isTorchActive = true;
        }
    }
}
