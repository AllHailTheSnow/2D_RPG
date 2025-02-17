using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleVFX;
    [SerializeField] private bool enemyProjectile = false;
    [SerializeField] private float projectileRange = 10f;

    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }

    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    private void MoveProjectile()
    {
        //Quaternion rotation = Quaternion.Euler(0, 0, 45);
        //Vector3 movementDir = rotation * Vector3.right;

        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }

    private void DetectFireDistance()
    {
        //if the distance between the start position and the current position is greater than the weapon range, destroy the projectile
        if (Vector3.Distance(transform.position, startPos) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //get the enemy health script from the enemy object
        EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
        //get the indestructable script from the indestructable object
        Indestructable indestructable = collision.GetComponent<Indestructable>();
        //get the player health script from the player object
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

        //if the projectile collides with an enemy, player or indestructable object
        if (!collision.isTrigger && (enemyHealth || indestructable || playerHealth))
        {
            //if the projectile collides with the player and is an enemy projectile or collides with an enemy and is a player projectile
            if ((playerHealth && enemyProjectile) || (enemyHealth && !enemyProjectile))
            {
                if(playerHealth)
                {
                    //deal damage to the player
                    playerHealth?.TakeDamage(1, transform);
                }
                
                //destroy the projectile and spawn the particle VFX
                Instantiate(particleVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            //if the projectile collides with an indestructable object, spawn the particle VFX and destroy the projectile
            else if (!collision.isTrigger && indestructable)
            {
                Instantiate(particleVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
