using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject projectilePrefab;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Attack()
    {
        // Set the attack trigger in the animator
        animator.SetTrigger("Attack");

        // Flip the sprite based on the player's position
        if (transform.position.x - PlayerController.Instance.transform.position.x < 0)
        {
            spriteRenderer.flipX = false;
            //Debug.Log("FlipX is false");
        }
        else
        {
            spriteRenderer.flipX = true;
            //Debug.Log("FlipX is true");
        }
    }

    public void SpawnProjectile()
    {
        // Instantiate the projectile prefab
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }
}
