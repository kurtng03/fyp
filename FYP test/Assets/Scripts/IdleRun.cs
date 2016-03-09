using UnityEngine;
using System.Collections;

public class IdleRun : MonoBehaviour
{

    public VirtualJoyStick joyStick;
    public Animator animator;
    public AnimatorStateInfo BS;
    static int Run = Animator.StringToHash("New Layer.walk");
    static int Idle = Animator.StringToHash("New Layer.Idle");
    static int Jump = Animator.StringToHash("New Layer.Jump");
    void Update()
    {

        Vector3 joyStickInput = JoyStickInput();

        animator.SetBool("Run", false);
        animator.SetBool("Jump", false);
        if (Input.GetKey("left")|| Input.GetKey("right") || joyStickInput.x < 0 || joyStickInput.x > 0)
        {
            animator.SetBool("Run", true);
        }
        if (Input.GetKey("up") || joyStickInput.y > 0)
        {
            animator.SetBool("Jump", true);
        }




    }

    private Vector3 JoyStickInput()
    {
        Vector3 direction = Vector3.zero;
        direction.x = joyStick.Horizontal();
        direction.y = joyStick.Vertical();

        if (direction.magnitude > 1)
            direction.Normalize();
        return direction;
    }

}

