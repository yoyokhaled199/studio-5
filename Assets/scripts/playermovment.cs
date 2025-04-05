using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveDistance = 2f; 
    public float moveHeight = 1f;   
    public float upDownSpeed = 2f;  
    public SpriteRenderer spriteRenderer;
    public Sprite middleSprite;
    public Sprite rightSprite;
    public Sprite leftSprite;

    private Vector3 startPosition;
    private bool isAtMiddle = true; 
    private bool isAtRight = false; 
    private bool isAtLeft = false;  
    private bool isMovingUp = false;
    private Vector3 targetPosition; 

    void Start()
    {
        startPosition = transform.position; 
        targetPosition = startPosition;
    }

    void Update()
    {
    
        if (Input.GetKeyDown(KeyCode.RightArrow) && isAtMiddle)
        {
            targetPosition = startPosition + new Vector3(moveDistance, 0, 0);
            spriteRenderer.sprite = rightSprite;
            isAtMiddle = false;
            isAtRight = true;
            isAtLeft = false;
        }
     
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && isAtMiddle)
        {
            targetPosition = startPosition + new Vector3(-moveDistance, 0, 0);
            spriteRenderer.sprite = leftSprite;
            isAtMiddle = false;
            isAtLeft = true;
            isAtRight = false;
        }
      
        else if ((Input.GetKeyDown(KeyCode.LeftArrow) && isAtRight) ||
                 (Input.GetKeyDown(KeyCode.RightArrow) && isAtLeft))
        {
            targetPosition = startPosition;
            spriteRenderer.sprite = middleSprite;
            isAtMiddle = true;
            isAtRight = false;
            isAtLeft = false;
        }

       
        if (isAtRight && Input.GetKeyDown(KeyCode.RightArrow))
        {
            return;
        }

       
        if (isAtLeft && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return;
        }

      
        if (Input.GetKey(KeyCode.UpArrow))
        {
            isMovingUp = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            isMovingUp = false;
        }

       
        float newY = startPosition.y + (isMovingUp ? moveHeight : 0);
        targetPosition = new Vector3(targetPosition.x, newY, targetPosition.z);

      
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * upDownSpeed);
    }
}
