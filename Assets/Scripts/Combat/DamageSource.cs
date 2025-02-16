using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    private int damageAmount;
    private float knockbackAmount;

    private void Start()
    {
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        damageAmount = (currentActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage;
        knockbackAmount = (currentActiveWeapon as IWeapon).GetWeaponInfo().weaponKnockback;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyHealth>())
        {
            // Get the EnemyHealth component from the object we collided with
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            // Call the TakeDamage method on the EnemyHealth component
            enemyHealth.TakeDamage(damageAmount);
            // Call the GetKnockedBack method on the EnemyHealth component
            enemyHealth.GetKnockedBack(knockbackAmount);
        }
    }
}
