using UnityEngine;
using System.Collections;

public class enemyDamage : MonoBehaviour {


    public float damage;
    // damge freq
    public float damageRate;
    public float pushBackForce;

    float nextDamage;

    // if player is in the range, add damage to player
    bool playerInRange = false;

    GameObject thePlayer;
    PlayerHealth thePlayerHealth;

	// Use this for initialization
	void Start () {
        nextDamage = Time.time;
        thePlayer = GameObject.FindGameObjectWithTag("Player");
        thePlayerHealth = thePlayer.GetComponent<PlayerHealth>();
	
	}
	
	// Update is called once per frame
	void Update () {
        if (playerInRange) Attack();
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
        }
    }

    void Attack()
    {
        if (nextDamage <= Time.time)
        {
            thePlayerHealth.addDamage(damage);
            nextDamage = Time.time + damageRate;

            pushBack(thePlayer.transform);
        }
    }

    void pushBack(Transform pushedObject)
    {
        Vector3 pushDirection = new Vector3(0, (pushedObject.position.y - transform.position.y), 0).normalized;
        pushDirection *= pushBackForce;

        Rigidbody pushedRB = pushedObject.GetComponent<Rigidbody>();
        pushedRB.velocity = Vector3.zero;
        pushedRB.AddForce(pushDirection, ForceMode.Impulse);
    }

}
