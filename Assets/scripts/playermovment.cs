using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] private PlayerConfig antConfig;
    [SerializeField] private PlayerConfig wormConfig;

    [Header("Sprite & Animation")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Tooltip("Seconds per animation frame")]
    [SerializeField] private float animationFrameRate = 0.15f;

    [Header("Movement Settings")]
    [SerializeField] private float moveDistance = 2f;
    [SerializeField] private float moveSpeed = 30f; 
    [SerializeField] private float fallSpeed = 8f;

    private PlayerConfig config;
    private Vector3 middlePosition, leftPosition, rightPosition, targetPosition;
    private PlayerPosition currentPosition = PlayerPosition.Middle;
    private bool isFrozen = false, isFalling = false;

    private int animFrame = 0;
    private float animTimer = 0f;

    private enum PlayerPosition { Left, Middle, Right }

    void Awake()
    {
        string selected = PlayerPrefs.GetString("SelectedCharacter", "ant");
        config = selected == "worm" ? wormConfig : antConfig;
        if (config == null) Debug.LogError("PlayerConfig not assigned!");

        middlePosition = transform.position;
        leftPosition = middlePosition + Vector3.left * moveDistance;
        rightPosition = middlePosition + Vector3.right * moveDistance;
        targetPosition = middlePosition;

        animFrame = 0;
        animTimer = 0f;
        SetSpriteForPosition(currentPosition, animFrame);
    }

    void Update()
    {
        if (isFalling)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward, 360f * Time.deltaTime); 
            AnimateSprite();
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            isFalling = true;
            isFrozen = true;
            return;
        }

        if (!isFrozen)
        {
            HandleInput();
            MovePlayer();
        }

        AnimateSprite(); 
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (currentPosition == PlayerPosition.Middle)
            {
                SetTarget(PlayerPosition.Left, leftPosition);
            }
            else if (currentPosition == PlayerPosition.Right)
            {
                SetTarget(PlayerPosition.Middle, middlePosition);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (currentPosition == PlayerPosition.Middle)
            {
                SetTarget(PlayerPosition.Right, rightPosition);
            }
            else if (currentPosition == PlayerPosition.Left)
            {
                SetTarget(PlayerPosition.Middle, middlePosition);
            }
        }
  
    }

    private void SetTarget(PlayerPosition newPos, Vector3 pos)
    {
        currentPosition = newPos;
        targetPosition = pos;
        animFrame = 0;
        animTimer = 0f;
        SetSpriteForPosition(currentPosition, animFrame);
    }

    private void MovePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
        }
    }

    private void AnimateSprite()
    {
        Sprite[] sprites = GetSpritesForPosition(currentPosition);
        if (sprites == null || sprites.Length == 0) return;

        animTimer += Time.deltaTime;
        if (animTimer >= animationFrameRate)
        {
            animTimer = 0f;
            animFrame = (animFrame + 1) % sprites.Length;
            spriteRenderer.sprite = sprites[animFrame];
        }
    }

    private void SetSpriteForPosition(PlayerPosition pos, int frame)
    {
        Sprite[] sprites = GetSpritesForPosition(pos);
        if (sprites != null && sprites.Length > 0)
            spriteRenderer.sprite = sprites[Mathf.Clamp(frame, 0, sprites.Length - 1)];
    }

    private Sprite[] GetSpritesForPosition(PlayerPosition pos)
    {
        switch (pos)
        {
            case PlayerPosition.Left: return config.leftSprites;
            case PlayerPosition.Right: return config.rightSprites;
            case PlayerPosition.Middle: return config.middleSprites;
            default: return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            FreezePlayer();
            GameManager.Instance?.GameOver();
        }
    }

    public void FreezePlayer() { isFrozen = true; }
    public void UnfreezePlayer() { isFrozen = false; }
    public void SavePosition()
    {
        PlayerPrefs.SetFloat("PlayerX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", transform.position.z);
    }
    public void LoadPosition()
    {
        if (PlayerPrefs.HasKey("PlayerX"))
        {
            Vector3 savedPos = new Vector3(
                PlayerPrefs.GetFloat("PlayerX"),
                PlayerPrefs.GetFloat("PlayerY"),
                PlayerPrefs.GetFloat("PlayerZ")
            );
            transform.position = savedPos;
        }
    }
}
