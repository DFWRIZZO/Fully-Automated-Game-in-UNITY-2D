using UnityEngine;
using System.Collections;

public class GoldBagSpawner : MonoBehaviour
{
    public GameObject goldBagPrefab; // Assign the gold bag prefab in the Inspector
    public float spawnInterval = 2f; // Time interval between spawns
    public float minSpawnDistance = 2f; // Minimum distance below the spawner to spawn the bags
    public float maxSpawnDistance = 4f; // Maximum distance below the spawner to spawn the bags
    public float lerpTime = 1f; // Time to lerp to the end position

    private void Start()
    {
        // Start the spawn coroutine
        StartCoroutine(SpawnGoldBags());
    }

    private IEnumerator SpawnGoldBags()
    {
        while (true)
        {
            // Wait for the specified spawn interval
            yield return new WaitForSeconds(spawnInterval);

            // Get a random spawn distance between min and max
            float randomSpawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

            // Get a random point within a unit circle and scale it to the random spawn distance
            Vector2 randomDirection = Random.insideUnitCircle * randomSpawnDistance;
            Vector3 endPosition = new Vector3(randomDirection.x, -randomSpawnDistance, randomDirection.y) + transform.position;

            // Start the lerping process
            StartCoroutine(LerpGoldBag(endPosition));
        }
    }

    private IEnumerator LerpGoldBag(Vector3 endPosition)
    {
        // Create the gold bag instance
        GameObject goldBag = Instantiate(goldBagPrefab, transform.position, Quaternion.identity);

        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < lerpTime && goldBag != null) // Check if goldBag still exists
        {
            // Lerp from the start position to the end position
            goldBag.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / lerpTime);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final position is set correctly if goldBag still exists
        if (goldBag != null)
        {
            goldBag.transform.position = endPosition;
        }
    }
}
