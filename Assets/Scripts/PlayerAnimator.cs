using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator animator;

    private readonly int horizontalSpeedHash = Animator.StringToHash("HorizontalSpeed");
    private readonly int verticalSpeedHash = Animator.StringToHash("VerticalSpeed");
    private readonly int isStoppedHash = Animator.StringToHash("isStopped");
    private readonly int lastVerticalHash = Animator.StringToHash("LastVertical");
    private readonly int lastHorizontalHash = Animator.StringToHash("LastHorizontal");
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetFloatVertical(float value)
    {

        animator.SetFloat(verticalSpeedHash, value);

    } 
  
    public void SetFloatHoriontal(float value)
    {

        animator.SetFloat(horizontalSpeedHash, value);

    }

    public void SetLastVertical(float value)
    {
        animator.SetFloat(lastVerticalHash, value);
    }
    public void SetLastHorizontal(float value)
    {
        animator.SetFloat(lastHorizontalHash, value);
    }

    public void SetIsStop(bool boolean)
    {
        animator.SetBool(isStoppedHash, boolean);
    }

}
