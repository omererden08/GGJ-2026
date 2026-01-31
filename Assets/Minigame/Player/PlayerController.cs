using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

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
    }

    private void Update()
    {
        if (isDead || isLocked) return;
        HandleInput();
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

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        rb.Sleep();

        Debug.Log("☠️ Player died");
        OnPlayerDied?.Invoke(); // Event gönder
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

        yield return new WaitForSeconds(cooldown);

        isLocked = false;
    }
}
