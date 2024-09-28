using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowScript : MonoBehaviour
{
    public GameObject arrowPrefab; // Reference to the arrow prefab
    public Transform shootPoint;   // Point from where the arrow will be instantiated
    public float arrowSpeed = 10f; // Speed of the arrow
    public float shootInterval = 1f; // Time interval between each shot
    public Animator animator; // Reference to the Animator component

    private GameObject targetGoblin; // The goblin currently in range
    private bool canShoot = true; // Flag to control the shooting interval

    private GameObject arrow; // Store the reference to the arrow instance

    void Update()
    {
        // Check if there is a target goblin in range and if the bow is ready to shoot
        if (targetGoblin != null && canShoot)
        {
            // Set the isShooting boolean to true in the Animator
            animator.SetBool("isShooting", true);

            // Start the cooldown to prevent multiple shots
            StartCoroutine(ShootingCooldown());
        }
    }

    // Method to be called by the animation event
    public void InstantiateArrow()
    {
        // Check if the target goblin is still in range before instantiating the arrow
        if (targetGoblin != null)
        {
            // Instantiate the arrow at the bow's shoot point position
            arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);

            // Calculate direction from bow to targetGoblin
            Vector2 direction = (targetGoblin.transform.position - shootPoint.position).normalized;

            // Apply force to the arrow's Rigidbody2D to make it move towards the target
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            rb.velocity = direction * arrowSpeed;

            // Set arrow rotation to face the target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Reset the isShooting boolean after checking and/or shooting
        ResetShootingState();
    }

    // Method to reset the shooting state in the Animator
    private void ResetShootingState()
    {
        animator.SetBool("isShooting", false);
    }

    // Method to detect when a goblin enters the bow's range
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goblin"))
        {
            targetGoblin = collision.gameObject; // Set the goblin as the target
        }
    }

    // Method to detect when a goblin leaves the bow's range
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Goblin"))
        {
            if (collision.gameObject == targetGoblin)
            {
                targetGoblin = null; // Remove the target if it exits the range
            }
        }
    }

    // Coroutine to add a delay between each shot
    IEnumerator ShootingCooldown()
    {
        canShoot = false; // Prevent shooting again until cooldown is over
        yield return new WaitForSeconds(shootInterval); // Wait for the specified interval
        canShoot = true; // Allow shooting again
    }
}
