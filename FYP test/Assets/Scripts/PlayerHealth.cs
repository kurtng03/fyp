using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    public float fullHealth;
    float currentHealth;

    // Use this for initialization
    void Start()
    {
        currentHealth = fullHealth;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            makeDead();
        }
    }

    public void makeDead()
    {
        Destroy(gameObject);
    }

}
