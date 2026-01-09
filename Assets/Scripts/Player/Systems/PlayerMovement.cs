using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input Manager")]
    [SerializeField] private InputManagerSO inputManager;

    [Header("Component Values")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerSteps steps;

    [Header("Movement Values")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float crouchedSpeed = 2.5f;

    [Header("Noise Values")]
    [SerializeField] private float walkNoise = 0.6f;
    [SerializeField] private float runNoise = 1f;
    [SerializeField] private float crouchNoise = 0.2f;

    // Movement
    private Vector2 inputMovementVector;
    private float movementSpeed = 5;
    private int pixelsPerUnit = 64;
    private Vector2 lastInputDirection;
    private Vector2 lastRbPosition;
    private bool isSprinting = false;

    // Aim Script ref
    private PlayerAim playerAimScript;

    // currently treated as walking instead of crouching
    private bool isCrouched = false;

    #region ENABLE-DISABLE Input
    private void OnEnable()
    {
        inputManager.OnMove += MoveInput;
        inputManager.OnSprint += Sprint;
        inputManager.OnCrouch += Crouch;
    }
    private void OnDisable()
    {
        inputManager.OnMove -= MoveInput;
        inputManager.OnSprint -= Sprint;
        inputManager.OnCrouch -= Crouch;
    }
    #endregion

    void Start()
    {
        movementSpeed = walkSpeed;
        lastRbPosition = rb.position;
    }

    public void HandleMovementAnimation()
    {
        bool isMoving = inputMovementVector.magnitude > 0.1f;
        if (isMoving)
        {
            lastInputDirection = inputMovementVector.normalized;
        }

        float speedMultiplier = 1f;

        if (playerAimScript != null && isMoving)
        {
            Vector2 moveDir = inputMovementVector.normalized;
            Vector2 aimDir = playerAimScript.targetDir;

            float dot = Vector2.Dot(moveDir, aimDir); // 1 = forward, -1 = backward

            // Backward movement slower and negative, forward normal
            float backwardMultiplier = -0.8f;
            speedMultiplier = dot >= 0 ? 1f : backwardMultiplier;

            // Sprinting multiplier
            if (isSprinting)
                speedMultiplier *= 1.5f;

            // Scale by input magnitude (smooth speed)
            speedMultiplier *= inputMovementVector.magnitude;
        }

        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("AnimationSpeed", speedMultiplier);

        // Keep Blend Tree synced with AimIndex
        if (playerAimScript != null)
            animator.SetFloat("AimDirection", playerAimScript.currentDirIndex);
    }

    public void HandleMovement()
    {
        if(inputMovementVector.magnitude > 0.01 || rb.linearVelocity.magnitude > 0.6f)
        {
            Vector2 targetPos = rb.position + inputMovementVector * movementSpeed * Time.deltaTime;

            targetPos.x = Mathf.Round(targetPos.x * pixelsPerUnit) / pixelsPerUnit;
            targetPos.y = Mathf.Round(targetPos.y * pixelsPerUnit) / pixelsPerUnit;

            rb.MovePosition(targetPos);
            lastRbPosition = rb.position;

            float noiseVolume = 0f;
            if (isCrouched) noiseVolume = crouchNoise;
            else if (isSprinting) noiseVolume = runNoise;
            else noiseVolume = walkNoise;

            NoiseSystem.MakeNoise(transform.position, noiseVolume);
        }
    }

    private void MoveInput(Vector2 ctx)
    {
        inputMovementVector = new Vector2(ctx.x, ctx.y);
    }

    private void Sprint()
    {
        isSprinting = !isSprinting;
        isCrouched = false;
        if (isSprinting)
        {
            movementSpeed = runSpeed;
        }
        else
        {
            movementSpeed = walkSpeed;
        }
        // SetCrouched animations

    }

    private void Crouch()
    {
        isCrouched = !isCrouched;
        isSprinting = false;
        if (isCrouched)
        {
            movementSpeed = crouchedSpeed;
        }
        else
        {
            movementSpeed = walkSpeed;
        }
    }

    public void SetAimScriptReference(PlayerAim aimScript)
    {
        playerAimScript = aimScript;
    }

    public void SetFootstep(AudioClip clip)
    {
        steps.SetFootstepClip(clip);
    }
}
