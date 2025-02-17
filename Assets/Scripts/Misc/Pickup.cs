using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private enum PickupType
    {
        Coin,
        Health,
        Stamina
    }

    [SerializeField] private PickupType pickupType;
    [SerializeField] private float pickupDistance = 5f;
    [SerializeField] private float accelerationRate = .2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;

    private Vector3 moveDir;
    private Rigidbody2D rb;

    private void Start()
    {
        // Start the animation curve spawn routine
        StartCoroutine(AnimCurveSpawnRoutine());
    }
    private void Awake()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Get the player's position
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if(Vector3.Distance(transform.position, playerPos) < pickupDistance)
        {
            // Calculate the direction to move towards the player
            moveDir = (playerPos - transform.position).normalized;
            moveSpeed += accelerationRate;
        }
        else
        {
            // Reset the direction
            moveDir = Vector3.zero;
            moveSpeed = 0f;
        }
    }

    private void FixedUpdate()
    {
        // Move the pickup towards the player
        rb.velocity = moveSpeed * Time.deltaTime * moveDir;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If the pickup collides with the player, destroy the pickup
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            DetectPickupType();
            Destroy(gameObject);
        }
    }

    private void DetectPickupType()
    {
        switch(pickupType)
        {
            case PickupType.Coin:
                //Add coins
                Debug.Log("Coin Collected");
                break;

            case PickupType.Health:
                //Heal player
                PlayerHealth.Instance.HealPlayer();
                Debug.Log("Health Collected");
                break;

            case PickupType.Stamina:
                //Regen stamina
                Debug.Log("Stamina Collected");
                break;
        }
    }

    private IEnumerator AnimCurveSpawnRoutine()
    {
        // get the starting position of the pickup
        Vector2 startPoint = transform.position;
        // get a random position near the pickup on the x and y axis
        float randomX = transform.position.x + Random.Range(-2f, 2f);
        float randomY = transform.position.y + Random.Range(-1f, 1f);

        // set the end point of the pickup
        Vector2 endPoint = new Vector2(randomX, randomY);

        // set the time passed to 0
        float timePassed = 0f;

        // while the time passed is less than the pop duration
        while (timePassed < popDuration)
        {
            // increment the time passed by the time since the last frame
            timePassed += Time.deltaTime;
            // calculate the linear time and height time
            float linearT = timePassed / popDuration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            // set the position of the pickup to the lerp between the start and end points
            transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0f, height);
            yield return null;
        }
    }
}
