using UnityEngine;

public class GoldBag : MonoBehaviour
{
    public float stopRadius = 1f; // Radius within which the gold bag will stop
    private bool isStopped = false;

    private void Start()
    {
        // Check if the Rigidbody2D and Collider2D are set up properly
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("GoldBag: Rigidbody2D component missing!");
        }
        else
        {
            Debug.Log("GoldBag: Rigidbody2D found and configured.");
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogError("GoldBag: Collider2D component missing!");
        }
        else
        {
            Debug.Log("GoldBag: Collider2D found and configured.");
        }
    }

    private void Update()
    {
        if (!isStopped)
        {
            // Check if the gold bag is within the stopping radius
            if (transform.position.y <= stopRadius)
            {
                isStopped = true;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop the gold bag
                GetComponent<Collider2D>().isTrigger = false; // Ensure it is not a trigger
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"GoldBag: Triggered by {collision.name}");
        // Check if the gold bag collides with a goblin
        if (collision.CompareTag("Goblin"))
        {
            Debug.Log("GoldBag: Collided with Goblin, destroying gold bag.");
            Destroy(gameObject); // Destroy the gold bag
        }
    }
}
