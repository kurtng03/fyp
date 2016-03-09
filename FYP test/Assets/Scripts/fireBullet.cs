using UnityEngine;
using System.Collections;

public class fireBullet : MonoBehaviour
{

    public float timeBetweenBullets = 0.15f;

    public GameObject projectile;

    float fire = 0;
    float nextBullet;

    // Use this for initialization
    void Awake()
    {
        nextBullet = 0f;
        fire = 0;
    }

    // Update is called once per frame
    void Update()
    {
        playerController myPlayer = transform.root.GetComponent<playerController>();

        //left mouse button
       if (fire > 0 && nextBullet < Time.time)
        {
            // when next bullet can shoot
            nextBullet = Time.time + timeBetweenBullets;

            Vector3 rot;
            if (myPlayer.getFacing() == -1f)
            {
                rot = new Vector3(0, -90, 0);
            }
            else
            {
                rot = new Vector3(0, 90, 0);
            }


            Instantiate(projectile, transform.position, Quaternion.Euler(rot));
        }
        
    }
    public void OpenFire()
    {
        fire =  1;
    }

    public void StopFire()
    {
        fire = 0;
    }


}
