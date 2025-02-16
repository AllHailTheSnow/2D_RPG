using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDir = 2f;

    // The state of the enemy
    private enum State
    {
        Stop,
        Roaming
    }

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
        // Start the roaming routine
        StartCoroutine(StateRoutine());
    }

    // The roaming routine of the enemy AI
    private IEnumerator StateRoutine()
    {
        // While the enemy is roaming
        while (state == State.Roaming)
        {
            // Get a random position to roam to
            Vector2 roamPosition = GetRoamingPosition();
            // Move to the random position
            enemyPathfinding.MoveTo(roamPosition);
            // Wait for 2 seconds before moving to the next position
            yield return new WaitForSeconds(roamChangeDir);
        }

        while(state == State.Stop)
        {
            // Stop moving
            enemyPathfinding.MoveTo(Vector2.zero);
            // Wait for 2 seconds before moving to the next position
            yield return new WaitForSeconds(1f);
        }
    }

    private Vector2 GetRoamingPosition()
    {
        // Return a random position within a 10 unit radius
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}

