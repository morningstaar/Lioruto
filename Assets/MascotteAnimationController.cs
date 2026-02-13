using UnityEngine;

public class MascotteAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayRun()
    {
        animator.SetTrigger("Run");
    }

    public void PlayKick()
    {
        animator.SetTrigger("Kick");
    }
}
