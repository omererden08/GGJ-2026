using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float cooldown = 1f;
    private Vector2 moveInput;

    private bool isDead = false;
    private bool isLocked = false;

    public static event Action OnPlayerDied;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDead || isLocked)
        {
            moveInput = Vector2.zero;
            SetWalkAnimation(false);
            return;
        }

        HandleInput();
        HandleRotation();
        SetWalkAnimation(moveInput.sqrMagnitude > 0f);
    }

    private void FixedUpdate()
    {
        if (isDead || isLocked) return;
        Move();
    }

    void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(horizontal, vertical).normalized;
    }

    void Move()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void HandleRotation()
    {
        if (moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveInput.x, -moveInput.y) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
    }

    void SetWalkAnimation(bool isWalking)
    {
        if (animator != null)
            animator.SetBool("walk", isWalking);
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        rb.Sleep();

        SetWalkAnimation(false);

        Debug.Log("☠️ Player died");
        OnPlayerDied?.Invoke();
    }

    public void PlayerLocked()
    {
        if (!isLocked)
            StartCoroutine(LockRoutine());
    }

    IEnumerator LockRoutine()
    {
        isLocked = true;
        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        rb.Sleep();

        SetWalkAnimation(false);

        yield return new WaitForSeconds(cooldown);

        isLocked = false;
    }
}
