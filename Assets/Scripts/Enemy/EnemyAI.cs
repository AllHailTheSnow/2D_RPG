using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDir = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private bool canAttack = true;

    // The state of the enemy
    private enum State
    {
        Roaming,
        Attacking
    }

    private Vector2 roamPosition;
    private float timeRoaming = 0f;

    // The current state of the enemy
    private State state;
    // The enemy pathfinding component
    private EnemyPathfinding enemyPathfinding;


    private void Awake()
    {
        // Get the enemy pathfinding component
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        // Set the state to roaming
        state = State.Roaming;
    }

    private void Start()
    {
        // set the roaming position to a random position
        roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        MovementStateControl();
    }

    // Control the movement state of the enemy
    private void MovementStateControl()
    {
        switch(state)
        {
            default:

            // If the enemy is roaming call the Roaming method
            case State.Roaming:
                Roaming();
                break;

            // If the enemy is attacking call the Attacking method
            case State.Attacking:
                Attacking();
                break;
        }
    }

    private void Roaming()
    {
        // Increase the time the enemy has been roaming
        timeRoaming += Time.deltaTime;

        // Move the enemy to the roaming position
        enemyPathfinding.MoveTo(roamPosition);

        // If the player is within the attack range, set the state
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
        }

        // If the enemy has been roaming for longer than the roamChangeDir time, get a new roaming position
        if (timeRoaming > roamChangeDir)
        {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Attacking()
    {
        // If the player is outside of the attack range, set the state to roaming
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
        }

        // If the player is in attack range and the enemy can attack, call the Attack method
        if (attackRange != 0 && canAttack)
        {
            // Set canAttack to false and call the Attack method on the enemyType
            canAttack = false;
            (enemyType as IEnemy).Attack();

            // If stopMovingWhileAttacking is true, stop the enemy from moving
            if (stopMovingWhileAttacking)
            {
                enemyPathfinding.StopMoving();
            }
            // If stopMovingWhileAttacking is false, set the enemy to move
            else
            {
                enemyPathfinding.MoveTo(roamPosition);
            }

            // Start the attack cooldown routine
            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private Vector2 GetRoamingPosition()
    {
        // Reset the timeRoaming
        timeRoaming = 0f;
        // Return a random position within a 10 unit radius
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private IEnumerator AttackCooldownRoutine()
    {
        // Wait for the attack cooldown time
        yield return new WaitForSeconds(attackCooldown);
        // Set canAttack to true
        canAttack = true;
    }
}

