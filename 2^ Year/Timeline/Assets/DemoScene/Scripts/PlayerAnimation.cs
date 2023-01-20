using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimation : MonoBehaviour
{
    public CharacterController cc;
    public float vel = 5;
    public float rotVel = 180;
    public float gravity = 9;
    public Animator animator;
    public UnityEvent onFoot;
    public UnityEvent onHit;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        cc.Move(transform.forward * vel * y * Time.deltaTime + Vector3.down * Time.deltaTime * gravity);
        transform.Rotate(Vector3.up * rotVel * Time.deltaTime * x);

        animator.SetFloat("velocity", y);
        animator.SetBool("moving", y != 0);

        if (Input.GetKeyDown(KeyCode.Space))
            animator.SetTrigger("attack");
    }

    public void FootR()
    {
        if (onFoot != null)
            onFoot.Invoke();
    }

    public void FootL()
    {
        if (onFoot != null)
            onFoot.Invoke();
    }

    public void Hit()
    {
        if (onHit != null)
            onHit.Invoke();
    }

    public void ForceIdle()
    {
        animator.SetBool("moving", false);
    }

}