using UnityEngine;

public class GoldAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Get the Animator component attached to the prefab
        animator = GetComponent<Animator>();

        // Play the animation on start (optional, depending on your needs)
        PlayGoldAnimation();
    }

    // Function to play the gold animation
    public void PlayGoldAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("PlayGold");
        }
    }
}
