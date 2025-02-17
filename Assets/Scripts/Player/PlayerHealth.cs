using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockbackAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;

    protected override void Awake()
    {
        base.Awake();

        //Sets the flash to the flash component on the object
        flash = GetComponent<Flash>();
        //Sets the knockback to the knockback component on the object
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        //Sets the current health to the max health
        currentHealth = maxHealth;

        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Creates an enemy ai object for the enemy
        EnemyAI enemy = collision.collider.GetComponent<EnemyAI>();

        //If the enemy exists
        if (enemy)
        {
            //Calls the take damage function
            TakeDamage(1, collision.transform);
        }
    }

    //Creates a function to take damage
    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        //If the player can't take damage, return
        if (!canTakeDamage) { return; }

        //Sets the can take damage to false
        canTakeDamage = false;
        //Subtracts the damage amount from the current health
        currentHealth -= damageAmount;
        //Calls the screen shake function
        ScreenShakeManager.Instance.ShakeScreen();
        //Calls the knockback function
        knockback.GetKnockedBack(hitTransform, knockbackAmount);
        //Starts the flash routine
        StartCoroutine(flash.FlashRountine());
        //starts the damage recovery routine
        StartCoroutine(DamageRecoveryRoutine());
        //Call the update health slider function
        UpdateHealthSlider();
    }

    public void HealPlayer()
    {
        //If the current health is less than the max health
        if (currentHealth < maxHealth)
        {
            //Add 1 to the current health
            currentHealth += 1;
            //Call the update health slider function
            UpdateHealthSlider();
        }
        
    }

    private void CheckPlayerDeath()
    {
        //If the current health is less than or equal to 0
        if (currentHealth <= 0)
        {
            //Set the current health to 0
            currentHealth = 0;
            Debug.Log("Player is dead");
        }
    }

    private void UpdateHealthSlider()
    {
        //If the health slider is null
        if (healthSlider == null)
        {
            //Find the health slider and set it to the health slider
            healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        }

        //Set the max value of the health slider to the max health
        healthSlider.maxValue = maxHealth;
        //Set the value of the health slider to the current health
        healthSlider.value = currentHealth;
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
