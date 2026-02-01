using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    [SerializeField] GameObject tutorialObject;
    
    [Header("Sprites")]
    [SerializeField] private Sprite[] walkSprites = null;
    [SerializeField] private Sprite[] carrySprites = null; // YENİ: Taşıma sprite'ları
    [Tooltip("0-based index into walkSprites used as the idle sprite (e.g. 3 = 4th element)")]
    [SerializeField, Min(0)] private int idleFrameIndex = 3;
    [SerializeField, Min(1f)] private float framesPerSecond = 8f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput;

    [Header("Movement Bounds")]
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float minY = -4.5f;
    [SerializeField] private float maxY = 4.5f;

    private float animTimer;
    private int currentFrame;
    private Vector3 baseScale;
    
    // YENİ: Taşıma durumu
    private bool isCarrying = false;
    private Sprite[] currentSpriteSet; // Aktif sprite seti

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
        
        // Başlangıçta normal sprite setini kullan
        currentSpriteSet = walkSprites;
        
        if (currentSpriteSet != null && currentSpriteSet.Length > 0)
            idleFrameIndex = Mathf.Clamp(idleFrameIndex, 0, currentSpriteSet.Length - 1);
        currentFrame = idleFrameIndex;
        ApplyIdleSprite();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (tutorialObject.activeSelf) AudioManager.Instance.PlaySFX(4);
            tutorialObject.SetActive(false);
        }
        if (tutorialObject.activeSelf) return;
        HandleInput();
        UpdateAnimation(Time.deltaTime);
        if (GameManager.Instance.storyScore >= 5)
        {
            SceneLoader.Instance.LoadScene("Minigame", GameState.Playing);
        }
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

        float clampedX = Mathf.Clamp(newPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(newPosition.y, minY, maxY);

        rb.MovePosition(new Vector2(clampedX, clampedY));

        spriteRenderer.sortingOrder = rb.position.y > 0 ? 1 : 4;
        
        if (moveInput.x > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
    }
    
    private void UpdateAnimation(float deltaTime)
    {
        if (currentSpriteSet == null || currentSpriteSet.Length == 0)
            return;

        bool isMoving = moveInput.magnitude > 0.01f;

        if (!isMoving)
        {
            if (currentFrame != idleFrameIndex)
            {
                currentFrame = idleFrameIndex;
                ApplyIdleSprite();
            }
            animTimer = 0f;
            return;
        }

        if (currentSpriteSet.Length < 2)
            return;

        if (currentFrame == idleFrameIndex)
        {
            currentFrame = NextWalkIndex(currentFrame);
            spriteRenderer.sprite = currentSpriteSet[currentFrame];
            animTimer = 0f;
            return;
        }

        animTimer += deltaTime;
        float frameTime = 1f / framesPerSecond;
        if (animTimer >= frameTime)
        {
            animTimer -= frameTime;
            currentFrame = NextWalkIndex(currentFrame);
            spriteRenderer.sprite = currentSpriteSet[currentFrame];
        }
    }

    private void ApplyIdleSprite()
    {
        if (currentSpriteSet != null && currentSpriteSet.Length > 0)
            spriteRenderer.sprite = currentSpriteSet[Mathf.Clamp(idleFrameIndex, 0, currentSpriteSet.Length - 1)];
    }

    private int NextWalkIndex(int idx)
    {
        int len = currentSpriteSet.Length;
        if (len <= 1)
            return idx;
        int next = (idx + 1) % len;
        while (next == idleFrameIndex)
            next = (next + 1) % len;
        return next;
    }

    // YENİ: Taşıma durumunu değiştiren public metodlar
    public void StartCarrying()
    {
        if (carrySprites == null || carrySprites.Length == 0)
        {
            Debug.LogWarning("Carry sprites not assigned!");
            return;
        }
        
        isCarrying = true;
        currentSpriteSet = carrySprites;
        currentFrame = idleFrameIndex;
        ApplyIdleSprite();
    }

    public void StopCarrying()
    {
        isCarrying = false;
        currentSpriteSet = walkSprites;
        currentFrame = idleFrameIndex;
        ApplyIdleSprite();
    }
    
    public bool IsCarrying()
    {
        return isCarrying;
    }
}