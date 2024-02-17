using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimespawner : MonoBehaviour
{
    
    public GameObject slimePrefab;
    public Transform spawnPoint;
    public int minSlimes = 2;
    public int maxSlimes = 7;
    public float spawnInterval = 3f;
    // Start is called before the first frame update
    void Start()
    {



        SpawnSlimes();


    }
    void SpawnSlimes()
    {
        int numberOfSlimes = Random.Range(minSlimes, maxSlimes + 1);

        for (int i = 0; i < numberOfSlimes; i++)
        {
            StartCoroutine(SpawnSlimeWithDelay(i * 0.5f)); // Introduce a delay between each spawn
            // Instantiate a slime at the spawn point with a random offset
            //Vector2 spawnPosition = GetValidSpawnPosition();
            //Vector2 spawnPosition = (Vector2)spawnPoint.position + Random.insideUnitCircle * 2f;
            //Instantiate(slimePrefab, spawnPosition, Quaternion.identity);
        }
    }

    IEnumerator SpawnSlimeWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector2 spawnPosition = GetValidSpawnPosition();
        Instantiate(slimePrefab, spawnPosition, Quaternion.identity);

    }


    Vector2 GetValidSpawnPosition()
    {
        Vector2 spawnPosition = (Vector2)spawnPoint.position + Random.insideUnitCircle * 2f;

        RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            spawnPosition.y = hit.point.y;
        }

        return spawnPosition;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
