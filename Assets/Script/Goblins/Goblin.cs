using UnityEngine;

public class GoblinFollowGold : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed at which the goblin moves
    public float acceptableDistance = 0.5f; // Distance threshold for reaching the target
    public GameObject goldPrefab; // Reference to the gold prefab to drop
    public Transform dropPoint; // Transform reference to define the drop location of gold
    public Transform spawnPoint; // The spawn point for the goblin to return to
    private Transform target; // Reference to the current target (gold, house, or spawn)
    private bool hasGold = false; // Keeps track of whether the Goblin is carrying a gold bag
    private int timesHit = 0; // Counter for how many times the goblin has been hit
    private bool runningAway = false; // Indicates if the goblin is running back to the spawn area
    private Animator animator; // Reference to the Animator component
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    private int goldBagCounter = 0; // Counter for how many times the goblin reaches home with a gold bag

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the spawn point to the goblin's initial position if not assigned
        if (spawnPoint == null)
        {
            spawnPoint = new GameObject("SpawnPoint").transform;
            spawnPoint.position = transform.position;
        }
    }

    private void Update()
    {
        if (runningAway)
        {
            target = spawnPoint; // Set the spawn point as the target if running away
        }
        else if (hasGold)
        {
            // Locate the home and set it as the target
            GameObject houseObject = GameObject.FindGameObjectWithTag("Home");
            if (houseObject != null)
            {
                target = houseObject.transform;
            }
        }
        else
        {
            // If the goblin does not have gold, look for the closest gold or DroppedGold bag
            FindClosestGold();
        }

        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            if (distanceToTarget > acceptableDistance) // Only move if distance is greater than acceptable
            {
                FollowTarget();
                SetAnimationState(true);
            }
            else
            {
                SetAnimationState(false); // Goblin reached the target
                if (runningAway && target == spawnPoint)
                {
                    runningAway = false; // Stop running away after reaching the spawn point
                }
            }
        }
        else
        {
            SetAnimationState(false); // Goblin is idle, no target to move towards
        }
    }

    // Find the closest gold bag (either "Gold" or "DroppedGold") and set it as the target
    private void FindClosestGold()
    {
        GameObject[] goldBags = GameObject.FindGameObjectsWithTag("Gold");
        GameObject[] droppedGoldBags = GameObject.FindGameObjectsWithTag("DroppedGold");

        // Combine both arrays into one list for searching
        GameObject[] allGoldBags = new GameObject[goldBags.Length + droppedGoldBags.Length];
        goldBags.CopyTo(allGoldBags, 0);
        droppedGoldBags.CopyTo(allGoldBags, goldBags.Length);

        float closestDistance = Mathf.Infinity;
        GameObject closestGold = null;

        // Iterate through each gold bag to find the closest one
        foreach (GameObject gold in allGoldBags)
        {
            float distanceToGold = Vector2.Distance(transform.position, gold.transform.position);
            if (distanceToGold < closestDistance)
            {
                closestDistance = distanceToGold;
                closestGold = gold;
            }
        }

        if (closestGold != null && !runningAway)
        {
            target = closestGold.transform; // Set the closest gold bag as the target
        }
    }

    private void FollowTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        FlipSprite(direction.x);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Gold") && !hasGold && !runningAway)
        {
            hasGold = true;
            target = null;
            Destroy(other.gameObject);
           
        }
        else if (other.CompareTag("DroppedGold") && !runningAway)
        {
            hasGold = true;
            timesHit = 0; // Reset the hit counter
            target = null;
            Destroy(other.gameObject);
            
        }
        else if (other.CompareTag("Home") && hasGold)
        {
            hasGold = false;
            target = null;
            goldBagCounter++; // Increment the gold bag counter
            Debug.Log("Gold Bag Counter: " + goldBagCounter); // Log the gold bag counter
        }
    }

    public void DropGoldOnHit()
    {
        if (hasGold)
        {
            
            // Instantiate the gold at the current position of the goblin
            GameObject droppedGold = Instantiate(goldPrefab, transform.position, Quaternion.identity);

            // Change the tag to "DroppedGold" for the newly instantiated gold
            droppedGold.tag = "DroppedGold";

            hasGold = false; // Set hasGold to false since the goblin no longer has gold
            target = spawnPoint; // Set the target to the spawn point
            runningAway = true; // Start running back to spawn

            timesHit = Mathf.Clamp(timesHit + 1, 0, 10); // Increment the hit counter and cap it at 10

           
        }
    }

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(target.position, acceptableDistance);
        }
    }

    public bool HasGold()
    {
        return hasGold;
    }

    private void SetAnimationState(bool isRunning)
    {
        animator.SetBool("isRunning", isRunning);
    }

    private void FlipSprite(float directionX)
    {
        spriteRenderer.flipX = directionX < 0;
    }
}
