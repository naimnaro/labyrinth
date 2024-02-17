using UnityEngine;

public class Villager1_Patrol : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float minMoveDistance = 4.0f; // Minimum move distance.
    public float maxMoveDistance = 10.0f; // Maximum move distance.
    public float patrolRange = 20.0f; // The total range within which the NPC can move.

    private Vector3 initialPosition;
    private float moveDistance; // The random move distance.
    private float currentMoveDistance; // The distance moved in the current direction.
    private bool movingRight = true;

    private void Start()
    {
        initialPosition = transform.position;
        GenerateRandomMoveDistance();
        currentMoveDistance = 0f; // Initialize current move distance to zero.
    }

    private void Update()
    {
        // Calculate the new position based on the current movement direction
        Vector3 newPosition = transform.position + (movingRight ? Vector3.right : Vector3.left) * moveSpeed * Time.deltaTime;

        // Calculate the distance from the initial position
        float distanceFromInitial = Vector3.Distance(newPosition, initialPosition);

        // Check if the NPC has moved beyond the random move distance
        if (distanceFromInitial >= moveDistance)
        {
            // Clamp the NPC's position to stay within the patrol range
            newPosition.x = Mathf.Clamp(newPosition.x, initialPosition.x - patrolRange / 2, initialPosition.x + patrolRange / 2);

            // Calculate the remaining distance to the patrol range boundary
            float remainingDistance = Mathf.Abs(newPosition.x - (initialPosition.x + (movingRight ? patrolRange / 2 : -patrolRange / 2)));

            // If the remaining distance is less than the random move distance, change direction
            if (remainingDistance < moveDistance)
            {
                movingRight = !movingRight;

                // Generate a new random move distance
                GenerateRandomMoveDistance();

                // Reset the current move distance
                currentMoveDistance = 0f;
            }
        }

        // Move the NPC
        transform.position = newPosition;

        // Update the current move distance
        currentMoveDistance += moveSpeed * Time.deltaTime;

        // Flip the NPC's direction based on movement
        Flip();
    }

    private void GenerateRandomMoveDistance()
    {
        // Generate a random move distance between minMoveDistance and maxMoveDistance
        moveDistance = Random.Range(minMoveDistance, maxMoveDistance);
    }

    private void Flip()
    {
        // Change the NPC's direction (flip its sprite or model)
        Vector3 scale = transform.localScale;
        scale.x = movingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
