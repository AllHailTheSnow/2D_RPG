using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    //[SerializeField] private float knockbackForce = 15f;

    private int currentHealth;
    private Knockback knockback;
    private Flash flash;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    // This method is called when the enemy takes damage
    public void TakeDamage(int damage)
    {
        // Subtract the damage from the current health
        currentHealth -= damage;
        // Call the Knockback method on the knockback component with the player's transform and a knockback value of 15
        //knockback.GetKnockedBack(PlayerController.Instance.transform, knockbackForce);
        // Call the FlashRountine method on the flash component
        StartCoroutine(flash.FlashRountine());
        // Check if the enemy is dead
        StartCoroutine(CheckDeathRoutine());
    }

    public void GetKnockedBack(float knockedBack)
    {
        knockback.GetKnockedBack(PlayerController.Instance.transform, knockedBack);
    }

    private IEnumerator CheckDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }

    private void DetectDeath()
    {
        // If the current health is less than or equal to 0
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            GetComponent<PickupSpawner>().DropItems();
            Destroy(gameObject);
        }
    }
}
