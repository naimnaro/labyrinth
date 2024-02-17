using UnityEngine;

public class enemy_patrol1 : MonoBehaviour
{

    public GameObject chestPrefab;  // Assign your chest prefab in the inspector
    private GameObject dropContainer; // Container for dropped items
    public int enemyHP = 60;

    public float moveSpeed = 3f;
    public float minMoveDistance = 4.0f;
    public float maxMoveDistance = 8.0f;
    public float patrolRange = 12.0f;

    public float jumpForce = 100.0f;
    public float gravityScale = 1.0f;
    public float jumpInterval = 2f;

    public float chaseRange = 5.0f;

    private Vector3 initialPosition;
    private float moveDistance;
    private float currentMoveDistance;
    private bool movingRight = true;

    private bool isChasing = false;
    private Transform playerTransform;

    private Rigidbody2D rb;
    private bool isGrounded;

    private void Start()
    {
        initialPosition = transform.position;
        GenerateRandomMoveDistance();
        currentMoveDistance = 0f;
        playerTransform = GameObject.FindGameObjectWithTag("playerdetect").transform;

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        isGrounded = false;

        dropContainer = new GameObject("DropContainer");
        dropContainer.transform.parent = transform;
        dropContainer.transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        jumpInterval -= Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < chaseRange)
        {
            isChasing = true;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("playerweapon"))
        {
            enemyHP -= DemoScene.player_damage;

            if (enemyHP <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Debug.Log("Enemy defeated!");
        float randomValue = Random.value;

        // Set the drop probability percentages
        float chestDropProbability = 0.3f; // 30% chance to drop a chest

        // Check if a chest should be dropped
        if (randomValue <= chestDropProbability)
        {
            // Instantiate a chest at the randomized position
            Instantiate(chestPrefab, dropContainer.transform.position, Quaternion.identity);

            // Log that a chest was dropped
            Debug.Log("Chest dropped!");
        }
        else
        {
            // Log that nothing was dropped
            Debug.Log("No drop.");
        }
        Destroy(gameObject);
    }

    private void ChasePlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        Flip();
        Jump();
    }

    void Patrol()
    {
        Vector3 newPosition = transform.position + (movingRight ? Vector3.right : Vector3.left) * moveSpeed * Time.deltaTime;

        float distanceFromInitial = Vector3.Distance(newPosition, initialPosition);

        if (distanceFromInitial >= moveDistance)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, initialPosition.x - patrolRange / 2, initialPosition.x + patrolRange / 2);

            float remainingDistance = Mathf.Abs(newPosition.x - (initialPosition.x + (movingRight ? patrolRange / 2 : -patrolRange / 2)));

            if (remainingDistance < moveDistance)
            {
                movingRight = !movingRight;
                GenerateRandomMoveDistance();
                currentMoveDistance = 0f;
            }
        }

        transform.position = newPosition;
        currentMoveDistance += moveSpeed * Time.deltaTime;
        Flip();
        Jump();
    }

    private void Jump()
    {
        if (jumpInterval <= 0 && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpInterval = 2f; // Reset jump interval
        }
    }

    private void GenerateRandomMoveDistance()
    {
        moveDistance = Random.Range(minMoveDistance, maxMoveDistance);
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = movingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            isGrounded = false;
        }
    }
}
