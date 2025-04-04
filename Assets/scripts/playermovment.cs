using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveDistance = 2f; // Distance for left/right movement
    public float moveHeight = 1f;   // Distance for up/down motion
    public float upDownSpeed = 2f;  // Speed for smooth up/down motion
    public SpriteRenderer spriteRenderer; // Reference to change sprite
    public Sprite middleSprite;
    public Sprite rightSprite;
    public Sprite leftSprite;

    private Vector3 startPosition;
    private bool isAtMiddle = true; // Tracks if the player is in the middle
    private bool isAtRight = false; // Tracks if player is at far right
    private bool isAtLeft = false;  // Tracks if player is at far left
    private bool isMovingUp = false;
    private Vector3 targetPosition; // Stores the movement position

    void Start()
    {
        startPosition = transform.position; // Store the start position
        targetPosition = startPosition; // Initialize target position
    }

    void Update()
    {
        // Move right if not already at far right
        if (Input.GetKeyDown(KeyCode.RightArrow) && isAtMiddle)
        {
            targetPosition = startPosition + new Vector3(moveDistance, 0, 0);
            spriteRenderer.sprite = rightSprite;
            isAtMiddle = false;
            isAtRight = true;
            isAtLeft = false;
        }
        // Move left if not already at far left
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && isAtMiddle)
        {
            targetPosition = startPosition + new Vector3(-moveDistance, 0, 0);
            spriteRenderer.sprite = leftSprite;
            isAtMiddle = false;
            isAtLeft = true;
            isAtRight = false;
        }
        // Move back to the middle
        else if ((Input.GetKeyDown(KeyCode.LeftArrow) && isAtRight) ||
                 (Input.GetKeyDown(KeyCode.RightArrow) && isAtLeft))
        {
            targetPosition = startPosition;
            spriteRenderer.sprite = middleSprite;
            isAtMiddle = true;
            isAtRight = false;
            isAtLeft = false;
        }

        // Prevent further right movement if at the far right
        if (isAtRight && Input.GetKeyDown(KeyCode.RightArrow))
        {
            return;
        }

        // Prevent further left movement if at the far left
        if (isAtLeft && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return;
        }

        // Check for up/down movement
        if (Input.GetKey(KeyCode.UpArrow))
        {
            isMovingUp = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            isMovingUp = false;
        }

        // Adjust only Y position for up/down
        float newY = startPosition.y + (isMovingUp ? moveHeight : 0);
        targetPosition = new Vector3(targetPosition.x, newY, targetPosition.z);

        // Smoothly move to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * upDownSpeed);
    }
}
