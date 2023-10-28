using UnityEngine;

public class CubePlayer : Player
{
    [SerializeField] private GameObject weapon;
    [SerializeField] private AnimationClip primaryAnimation;
    private Animator animator;

    public override void FirePrimary()
    {
        base.FirePrimary();
        animator.Play(primaryAnimation.name);
    }

    public override void FireSecondary()
    {
        base.FireSecondary();
        Debug.Log("Secondary fire in Cube Player");
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    
}
