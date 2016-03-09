using UnityEngine;
using System.Collections;


public class playerController : MonoBehaviour
{

    public float runSpeed;
    public int boost_value;
    public int boost_setLimit;


    Rigidbody myRB;
    Animator myAnim;
    public VirtualJoyStick joyStick;


    bool facingRight;
    bool IsJump;
    int boost_limit;
    float move;

    int JumpNum = 0;

    // Use this for initialization
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
        facingRight = true;
    }


    void FixedUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }

        Vector3 joyStickInput = JoyStickInput();

        Debug.Log("boost_limit :" + boost_limit);
        Debug.Log("boost_value :" + boost_value);

         move = Input.GetAxis("Horizontal");
        Debug.Log("move = " + move);
        if (joyStickInput.x < 0) move = -1;
        if (joyStickInput.x > 0) move = 1;
        myAnim.SetFloat("Speed", Mathf.Abs(move));

        // Jump
       
        IsJump = Input.GetKey(KeyCode.W);
        IsJump = checkJump();
        
        myAnim.SetBool("IsJump", IsJump);

        if (IsJump == true && boost_limit <= boost_setLimit && boost_limit > 0)
        {

            myRB.velocity = new Vector3(move * runSpeed, myRB.velocity.y + boost_value, 0);
            boost_limit = boost_limit - 1;
        }


        if (IsJump == false)
        {
            myRB.velocity = new Vector3(move * runSpeed, myRB.velocity.y, 0);
            if (boost_limit != boost_setLimit)
            {
                boost_limit = boost_limit + 1;
            }
        }




        if (move > 0 && facingRight != true)
        {
            Flip();
        }
        else if (move < 0 && facingRight) { Flip(); }
    }

    void Flip()
    {
        Debug.Log("running flip function");
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.z *= -1;
        transform.localScale = theScale;
    }

    public float getFacing()
    {
        if (facingRight) return 1;
        else return -1;
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

    public void setIsJump(bool check)
    {
        this.IsJump = true;
    }

    public void Jump()
    {
        JumpNum = 1;

        Debug.Log("calling jump");
    }

private bool checkJump()
    {
        if (JumpNum == 1)
        {
            JumpNum = 0;
            return true;
        }
        else return false;
    }   

}
