
using UnityEngine;
using UnityEngine.UI;

public class boss_slime : MonoBehaviour
{

    public GameObject chestPrefab;  // Assign your chest prefab in the inspector
    private GameObject dropContainer;

    public GameObject clear_panel;


    public GameObject hpbar_trigger;
    public UnityEngine.UI.Image hpbar;
    public float king_slime_HP = 500f;
    public static int king_slime_damage = 30;

    public float moveSpeed = 5f;
    public float minMoveDistance = 4.0f;
    public float maxMoveDistance = 8.0f;
    public float patrolRange = 12.0f;

    public float jumpForce = 10.0f;
    public float gravityScale = 2.0f;
    public float jumpInterval = 2f;

    public float chaseRange = 10.0f;

    private Vector3 initialPosition;
    private float moveDistance;
    private float currentMoveDistance;
    private bool movingRight = true;

    private bool isChasing = false;
    private Transform playerTransform;

    private Rigidbody2D rb;
    private bool isGrounded;

    public GameObject reward;

    private void Start()
    {
        hpbar_trigger.SetActive(false);
        initialPosition = transform.position;
        GenerateRandomMoveDistance();
        currentMoveDistance = 0f;
        playerTransform = GameObject.FindGameObjectWithTag("playerdetect").transform;

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        isGrounded = false;

        reward.SetActive(true);

        // Create a drop container as a child of the boss slime
        dropContainer = new GameObject("DropContainer");
        dropContainer.transform.parent = transform;
        dropContainer.transform.localPosition = Vector3.zero;
        clear_panel.SetActive(false);
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
            hpbar_trigger.SetActive(true);
            king_slime_HP -= DemoScene.player_damage;
            hpbar.fillAmount = king_slime_HP / 500f;

            if (king_slime_HP <= 0)
            {            
                Die();
                hpbar_trigger.SetActive(false);
            }
        }
    }

    private void Die()
    {
        clear_panel.SetActive(true);
        // Randomly determine the number of chests to spawn (between 1 and 2)
        int numberOfChests = Random.Range(1, 3);

        // Spawn the random number of chests at the drop container's position
        for (int i = 0; i < numberOfChests; i++)
        {
            // Randomize the position slightly to avoid overlapping chests
            Vector3 randomOffset = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
            Vector3 spawnPosition = dropContainer.transform.position + randomOffset;

            // Instantiate a chest at the randomized position
            Instantiate(chestPrefab, spawnPosition, Quaternion.identity);
        }
        Debug.Log("Enemy defeated!");
        reward.SetActive(false);
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
