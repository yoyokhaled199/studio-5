using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerConfig config = null;

    [Header("Movement Settings")]
    public float moveDistance = 2f;
    public float moveHeight = 1f;
    // Code review : put these in player config
    // so that worm and ant have different stats
    public float moveSpeed = 5f;
    public float fallSpeed = 5f;

    [Header("Sprite Settings")]
    public SpriteRenderer spriteRenderer;

    private Vector3 middlePosition;
    private Vector3 leftPosition;
    private Vector3 rightPosition;

    private Vector3 targetPosition;
    private PlayerPosition currentPosition = PlayerPosition.Middle;
    private bool isMoving = false;
    private bool isFrozen = false;
    private bool isFalling = false;

    private enum PlayerPosition
    {
        Middle,
        Right,
        Left
    }

    // Code review : the player movement needs to be initialized
    // with the config (ant or worm) that the player chose in the menu

    void Awake()
    {
        middlePosition = transform.position;
        leftPosition = middlePosition + Vector3.left * moveDistance;
        rightPosition = middlePosition + Vector3.right * moveDistance;

        targetPosition = transform.position;
        UpdateCurrentPositionBasedOnLocation();
    }

    void UpdateCurrentPositionBasedOnLocation()
    {
        float distToLeft = Vector3.Distance(transform.position, leftPosition);
        float distToMiddle = Vector3.Distance(transform.position, middlePosition);
        float distToRight = Vector3.Distance(transform.position, rightPosition);

        if (distToLeft < distToMiddle && distToLeft < distToRight)
        {
            currentPosition = PlayerPosition.Left;
            spriteRenderer.sprite = config.leftSprite;
        }
        else if (distToRight < distToMiddle && distToRight < distToLeft)
        {
            currentPosition = PlayerPosition.Right;
            spriteRenderer.sprite = config.rightSprite;
        }
        else
        {
            currentPosition = PlayerPosition.Middle;
            spriteRenderer.sprite = config.middleSprite;
        }
    }

    void Update()
    {
        if (isFalling)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            isFalling = true;
            isFrozen = true;
            isMoving = false;
            return;
        }

        if (!isFrozen)
        {
            HandleInput();
            MovePlayer();
        }
    }

    private void HandleInput()
    {
       // if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            switch (currentPosition)
            {
                case PlayerPosition.Middle:
                    MoveToPosition(PlayerPosition.Right, rightPosition);
                    break;
                case PlayerPosition.Left:
                    MoveToPosition(PlayerPosition.Middle, middlePosition);
                    break;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            switch (currentPosition)
            {
                case PlayerPosition.Middle:
                    MoveToPosition(PlayerPosition.Left, leftPosition);
                    break;
                case PlayerPosition.Right:
                    MoveToPosition(PlayerPosition.Middle, middlePosition);
                    break;
            }
        }
    }

    private void MoveToPosition(PlayerPosition newPosition, Vector3 position)
    {
        currentPosition = newPosition;
        targetPosition = position;
        isMoving = true;

        spriteRenderer.sprite = newPosition switch
        {
            PlayerPosition.Left => config.leftSprite,
            PlayerPosition.Right => config.rightSprite,
            _ => config.middleSprite
        };
    }

    private void MovePlayer()
    {
        if (isMoving && !isFrozen)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
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

    public void FreezePlayer()
    {
        isFrozen = true;
        isMoving = false;
    }

    public void UnfreezePlayer()
    {
        isFrozen = false;
    }

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
            UpdateCurrentPositionBasedOnLocation();
        }
    }
}
