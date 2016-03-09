using UnityEngine;
using System.Collections;

public class shootBullet : MonoBehaviour
{

    // how far the bullet goes
    public float range = 10f;
    public float damage = 5f;

    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    LineRenderer gunLine;

    // Use this for initialization
    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunLine = GetComponent<LineRenderer>();

        // ray start position
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;
        gunLine.SetPosition(0, transform.position);

        // if the bullet hit something
        if(Physics.Raycast(shootRay, out shootHit , range , shootableMask))
        {
            //hit an enemy goes here
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
