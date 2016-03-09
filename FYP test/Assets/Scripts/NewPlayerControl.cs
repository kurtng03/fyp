using UnityEngine;
using System.Collections;

public class NewPlayerControl : MonoBehaviour {

    public Transform m_transform;
    CharacterController m_ch;
    float m_movSpeed = 3.0f;
    float m_gravity = 2.0f;

    public int m_life = 5;


	// Use this for initialization
	void Start () {
        m_transform = this.transform;
        m_ch = this.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {

        //check 
        Vector3 playerPos = m_ch.transform.position;



      


        float xm=0, ym=0, zm = 0;
        ym -= m_gravity * Time.deltaTime;
      

        if (Input.GetKey(KeyCode.W))
        {
            ym += m_movSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            xm -= m_movSpeed * Time.deltaTime;
        }
         if (Input.GetKey(KeyCode.D))
        {
            xm += m_movSpeed * Time.deltaTime;
        }
       

        m_ch.Move(m_ch.transform.TransformDirection(new Vector3(xm, ym, 0)));
    }
}
