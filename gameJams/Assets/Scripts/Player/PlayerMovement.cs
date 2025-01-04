using UnityEngine;
using System.Collections;
using UnityEditor;

namespace MyGame.Player
{
    /// <summary>
    /// Oyuncunun temel hareket, z�plama, duvar kayma, duvar z�plama ve dash gibi i�levlerini y�netir.
    /// Kod paneli (isCodePanelActive) a��k oldu�unda input devre d��� kal�r.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TrailRenderer dashTrail;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private PlayerSettings playerSettings;
        [SerializeField] private Animator animator;

        private Rigidbody2D rb;
        private float horizontalInput;
        private bool isFacingRight = true;
        private bool isWallSliding;
        private bool isWallJumping;
        private bool isSideClimbing;
        private bool isDashing;
        private bool canDash = true;
        private float wallJumpingCounter;
        private float wallJumpingDirection;
        private bool canDoubleJump;
        private float coyoteTimeCounter;
        private float jumpBufferCounter;
        private bool isCodePanelActive = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (playerSettings == null)
            {
                Debug.LogError("PlayerSettings not assigned in PlayerMovement.");
            }
        }

        private void Update()
        {
            if (isDashing || isCodePanelActive) return;

            HandleInput();
            HandleWallSlide();
            HandleWallJump();
            FlipCharacter();
            UpdateAnimatorParameters();
        }

        private void FixedUpdate()
        {
            if (!isWallJumping && !isDashing && !isCodePanelActive)
            {
                MoveCharacter();
            }
        }

        private void HandleInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");

            // Coyote time
            if (IsGrounded())
            {
                coyoteTimeCounter = playerSettings.coyoteTime;
                canDoubleJump = true;
            }
            else
            {
                coyoteTimeCounter -= Time.fixedDeltaTime;
            }

            // Jump buffer
            if (Input.GetButtonDown("Jump"))
            {
                jumpBufferCounter = playerSettings.jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.fixedDeltaTime;
            }

            // Jumping
            if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
            {
                Jump();
                jumpBufferCounter = 0f;
            }
            else if (jumpBufferCounter > 0f && canDoubleJump)
            {
                Jump();
                canDoubleJump = false;
                jumpBufferCounter = 0f;
            }

            // Variable jump height
            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * playerSettings.variableJumpHeightMultiplier);
            }

            // Dash
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartCoroutine(DashCoroutine());
            }
        }

        private void MoveCharacter()
        {
            rb.velocity = new Vector2(horizontalInput * playerSettings.speed, rb.velocity.y);
        }

        private void Jump()
        {
            rb.velocity = new Vector2(rb.velocity.x, playerSettings.jumpingPower);
        }

        private bool IsGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, playerSettings.checkRadius, groundLayer);
        }

        private bool IsWalled()
        {
            return Physics2D.OverlapCircle(wallCheck.position, playerSettings.checkRadius, wallLayer);
        }

        private void HandleWallSlide()
        {
            if (IsWalled() && !IsGrounded() && horizontalInput != 0f)
            {
                isWallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -playerSettings.wallSlidingSpeed, float.MaxValue));
            }
            else
            {
                isWallSliding = false;
            }
        }

        private void HandleWallJump()
        {
            if (isWallSliding)
            {
                wallJumpingCounter = playerSettings.wallJumpingTime;
                isWallJumping = false;
                wallJumpingDirection = -Mathf.Sign(transform.localScale.x);
            }
            else
            {
                wallJumpingCounter -= Time.fixedDeltaTime;
            }

            if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
            {
                isWallJumping = true;
                rb.velocity = new Vector2(
                    wallJumpingDirection * playerSettings.wallJumpingPower.x,
                    playerSettings.wallJumpingPower.y
                );
                wallJumpingCounter = 0f;

                FlipCharacterImmediately();
                Invoke(nameof(ResetWallJumping), playerSettings.wallJumpingDuration);
            }
        }

        private IEnumerator DashCoroutine()
        {
            canDash = false;
            isDashing = true;

            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;

            rb.velocity = new Vector2(transform.localScale.x * playerSettings.dashingPower, 0f);
            if (dashTrail != null) dashTrail.emitting = true;

            yield return new WaitForSeconds(playerSettings.dashingTime);

            if (dashTrail != null) dashTrail.emitting = false;
            rb.gravityScale = originalGravity;
            isDashing = false;

            yield return new WaitForSeconds(playerSettings.dashingCooldown);
            canDash = true;
        }

        private void ResetWallJumping()
        {
            isWallJumping = false;
        }

        private void FlipCharacter()
        {
            if (isWallJumping) return;
            if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
            {
                FlipCharacterImmediately();
            }
        }

        private void FlipCharacterImmediately()
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

        private void UpdateAnimatorParameters()
        {
            if (animator == null) return;

            float horizontalSpeed = Mathf.Abs(rb.velocity.x);
            animator.SetFloat("Speed", horizontalSpeed);

            bool grounded = IsGrounded();
            bool isJumping = !grounded && rb.velocity.y > 0.1f;
            bool isFalling = !grounded && rb.velocity.y < -0.1f;
            bool isLanding = (!isFalling && grounded && rb.velocity.y < 0f);

            animator.SetBool("IsJumping", isJumping);
            animator.SetBool("IsFalling", isFalling);
            animator.SetBool("IsLanding", isLanding);
            animator.SetBool("IsWallSliding", isWallSliding);
            animator.SetBool("IsSideClimbing", isSideClimbing);
            animator.SetBool("IsDashing", isDashing);
            animator.SetBool("IsGrounded", grounded);
        }

        public void SetCodePanelState(bool isActive)
        {
            isCodePanelActive = isActive;
        }
    }
}
