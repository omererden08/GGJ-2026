using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput;

    [Header("Movement Bounds")]
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float minY = -4.5f;
    [SerializeField] private float maxY = 4.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
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
        Vector2 newPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

        // Pozisyonu sýnýrla
        float clampedX = Mathf.Clamp(newPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(newPosition.y, minY, maxY);

        rb.MovePosition(new Vector2(clampedX, clampedY));

        // Yönlendirme (flip x)
        if (moveInput.x > 0.01f)
            transform.localScale = new Vector3(1, 1, 1); // saða bak
        else if (moveInput.x < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1); // sola bak
    }

}
