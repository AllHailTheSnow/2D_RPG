using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockbackAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;

    private void Awake()
    {
        //Sets the flash to the flash component on the object
        flash = GetComponent<Flash>();
        //Sets the knockback to the knockback component on the object
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        //Sets the current health to the max health
        currentHealth = maxHealth;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Creates an enemy ai object for the enemy
        EnemyAI enemy = collision.collider.GetComponent<EnemyAI>();

        //If the enemy exists and the player can take damage
        if (enemy && canTakeDamage)
        {
            //Calls the take damage function
            TakeDamage(1);
            //Calls the knockback function
            knockback.GetKnockedBack(collision.gameObject.transform, knockbackAmount);
            //Starts the flash routine
            StartCoroutine(flash.FlashRountine());
        }
    }

    //Creates a function to take damage
    private void TakeDamage(int damageAmount)
    {
        //Sets the can take damage to false
        canTakeDamage = false;
        //Subtracts the damage amount from the current health
        currentHealth -= damageAmount;
        //starts the damage recovery routine
        StartCoroutine(DamageRecoveryRoutine());
    }

    //Creates a function to start the damage recovery routine
    private IEnumerator DamageRecoveryRoutine()
    {
        //Waits for the damage recovery time
        yield return new WaitForSeconds(damageRecoveryTime);
        //Sets the can take damage to true
        canTakeDamage = true;
    }
}
