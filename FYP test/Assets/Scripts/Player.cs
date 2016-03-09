
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{

    float moveSpeed = 6;
    float gravity = -20;
    float moveCheck;
    public VirtualJoyStick joyStick;
    //e03 start
    float jumpVelocity;
    public float jumpHeight = 4; // 跳到幾高
    public float timeToJumpApex = 0.4f; // 需要幾長時間到最高點

    // e03 end

    Vector3 velocity;

    Controller controller;

    bool IslookLeft = false;


    public float animSpeed = 1.5f;
    // 前進速度
    public float forwardSpeed = 7.0f;
    // 後退速度
    public float backwardSpeed = 2.0f;

    private Animator anim;
    private AnimatorStateInfo currentBaseState;

    void Start()
    {
        
        controller = GetComponent<Controller>();

        //e03 start

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("gravity: " + gravity + " Jump velocity: " + jumpVelocity);


        // e03 end
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        /*
        // lock z axis
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;

        //quit game

        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }


        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //e03 start

        Vector3 joyStickInput = JoyStickInput();


        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {

            velocity.y = jumpVelocity;
        }

        if (joyStickInput.y >0  && controller.collisions.below)
        {

            velocity.y = jumpVelocity;
        }

        if (Input.GetKey("left")  || joyStickInput.x <0)
        {
            if (IslookLeft == false)
            {
                //  transform.rotation = Quaternion.Euler(0f, -180f, 0);

                transform.localRotation = Quaternion.Euler(new Vector3(0, 180f, 0));
                IslookLeft = true;
             
                
            }
            moveCheck = 1;

        }




        if (Input.GetKey("right") || joyStickInput.x > 0)
        {
            if (IslookLeft == true)
            {
                //  transform.rotation = Quaternion.Euler(0f, 0f, 0);
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

                IslookLeft = false;
                
            }
            moveCheck = 1;
        }
        //e03 end

        
        velocity.x = moveCheck * moveSpeed;
        moveCheck = 0;
        
      //  print("velocity.x =" + velocity.x);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
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
    */

        float h = Input.GetAxis("Horizontal");              // 入力デバイスの水平軸をhで定義
        float v = Input.GetAxis("Vertical");                // 入力デバイスの垂直軸をvで定義
        anim.SetFloat("Speed", v);                          // Animator側で設定している"Speed"パラメタにvを渡す
        anim.SetFloat("Direction", h);                      // Animator側で設定している"Direction"パラメタにhを渡す
        anim.speed = animSpeed;                             // Animatorのモーション再生速度に animSpeedを設定する
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);


        // 以下、キャラクターの移動処理
        velocity = new Vector3(0, 0, v);        // 上下のキー入力からZ軸方向の移動量を取得
                                                // キャラクターのローカル空間での方向に変換
        velocity = transform.TransformDirection(velocity);
        //以下のvの閾値は、Mecanim側のトランジションと一緒に調整する
        if (v > 0.1)
        {
            velocity *= forwardSpeed;       // 移動速度を掛ける
        }
        else if (v < -0.1)
        {
            velocity *= backwardSpeed;  // 移動速度を掛ける
        }
    }
}