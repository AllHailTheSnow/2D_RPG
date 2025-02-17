using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    // The bullet prefab that the enemy will shoot
    [SerializeField] private GameObject bulletPrefab;
    [Tooltip("Must be set to minimum of 0.1")]
    [SerializeField] private float bulletMoveSpeed;
    [Tooltip("Must be set to minimum of 1")]
    [SerializeField] private int burstCount;
    [Tooltip("Must be set to minimum of 1")]
    [SerializeField] private int projectilesPerBurstCount;
    [SerializeField] [Range(0, 359)] private float angleSpread;
    [Tooltip("Must be set to minimum of 0.1")]
    [SerializeField] private float startingDistance = 0.1f;
    [Tooltip("Must be set to minimum of 0.1")]
    [SerializeField] private float timeBetweenBursts;
    [Tooltip("Must be set to minimum of 0.1")]
    [SerializeField] private float restTime = 1f;
    [SerializeField] private bool stagger;
    [Tooltip("Stagger must be enabled for oscillate to work")]
    [SerializeField] private bool oscillate;

    private bool isShooting = false;

    //Validate the values of the variables to be used in editor
    private void OnValidate()
    {
        //Only allow oscillate to be true if stagger is true
        if (oscillate) { stagger = true; }
        if (!oscillate) { stagger = false; }
        //projectilesPerBurstCount must be at least 1
        if (projectilesPerBurstCount < 1) { projectilesPerBurstCount = 1; }
        //burstCount must be at least 1
        if (burstCount < 1) { burstCount = 1; }
        //timeBetweenBursts, restTime, and startingDistance must be at least 0.1
        if (timeBetweenBursts < 0.1f) { timeBetweenBursts = 0.1f; }
        if (restTime < 0.1f) { restTime = 0.1f; }
        if (startingDistance < 0.1f) { startingDistance = 0.1f; }
        //if angleSpread is 0, projectilesPerBurstCount must be 1
        if (angleSpread == 0) { projectilesPerBurstCount = 1; }
        //bulletMoveSpeed must be at least 0.1
        if (bulletMoveSpeed <= 0) { bulletMoveSpeed = 0.1f; }
    }

    public void Attack()
    {
        // if the enemy is not shooting, start the shooting routine
        if (!isShooting)
        {
            StartCoroutine(ShootRountine());
        }
    }

    private IEnumerator ShootRountine()
    {
        // Set isShooting to true so that the enemy will not start another shooting routine while this one is running
        isShooting = true;

        // Set the start angle, current angle, and angle step for the cone of influence
        float startAngle, currentAngle, angleStep, endAngle;

        // Set the time between projectiles to 0
        float timeBetweenProjectiles = 0f;

        // Get the start angle, current angle, and angle step for the cone of influence
        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

        if (stagger) { timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurstCount; }

        // Loop through the burst count
        for (int i = 0; i < burstCount; i++)
        {
            // If the enemy is oscillating, set the current angle to the end angle and the end angle to the start angle
            if (!oscillate) 
            { 
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle); 
            }

            // If the enemy is oscillating and the modulus of i divided by 2 is not 1, redefine the cone of influence
            if (oscillate && i % 2 != 1) 
            { 
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle); 
            }
            // If the enemy is oscillating and the modulus of i divided by 2 is 1, set the current angle to the end angle,
            // the end angle to the start angle, and the angle step to the negative angle step
            else if (oscillate)
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }

            // Loop through the projectiles per burst count
            for (int j = 0; j < projectilesPerBurstCount; j++)
            {
                // Find the position to spawn the bullet
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                // Instantiate the bullet prefab at the position and set the rotation to face the player
                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                // If the bullet has a Projectiles component, update the move speed
                if (newBullet.TryGetComponent(out Projectiles projectile))
                {
                    // Update the move speed of the bullet
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                // Increment the current angle by the angle step
                currentAngle += angleStep;

                // Wait for the time between projectiles if the enemy is staggering
                if (stagger) { yield return new WaitForSeconds(timeBetweenProjectiles); }
            }

            // Set the current angle back to the start angle
            currentAngle = startAngle;

            // Wait for the time between bursts if the enemy is not staggering
            if (!stagger) { yield return new WaitForSeconds(timeBetweenBursts); }
        }

        // Wait for the rest time
        yield return new WaitForSeconds(restTime);
        // Set isShooting to false so that the enemy can start another shooting routine
        isShooting = false;

    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        // Get the direction to the player from the enemy
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        // Get the angle in degrees from the enemy to the player
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        // Set the start angle, end angle, and current angle to the target angle
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        // Set the angle step and half angle spread to 0
        float halfAngleSpread = 0f;
        angleStep = 0;

        // If the angle spread is not 0, calculate the angle step, half angle spread, start angle, and end angle
        if (angleSpread != 0)
        {
            //Set the angle step to the angle spread divided by the projectiles per burst count minus 1
            angleStep = angleSpread / (projectilesPerBurstCount - 1);
            //Set the half angle spread to the angle spread divided by 2
            halfAngleSpread = angleSpread / 2f;
            //Set the start angle to the target angle minus the half angle spread
            startAngle = targetAngle - halfAngleSpread;
            //Set the end angle to the target angle plus the half angle spread
            endAngle = targetAngle + halfAngleSpread;
            //Set the current angle to the start angle
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPos(float currentAngle)
    {
        //Set x and y to the starting distance away from the enemy in the direction of the current angle in degrees
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        //Create a new Vector2 with the x and y values
        Vector2 pos = new Vector3(x, y);

        //Return the new Vector2
        return pos;

    }
}
