using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goblin")) // Check if the collided object has the "Goblin" tag
        {
            // Check if the Goblin has the GoblinFollowGold component
            GoblinFollowGold goblin = collision.GetComponent<GoblinFollowGold>();

            if (goblin != null && goblin.HasGold()) // Ensure the Goblin has the script and is carrying gold
            {
                goblin.DropGoldOnHit(); // Call the method to drop gold
                Debug.Log("Arrow hit a Goblin carrying gold.");
            }

            Destroy(gameObject); // Destroy the arrow
        }
    }
}
