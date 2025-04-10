using UnityEngine; // Add this line at the top if it's not already present

public class Thorn : MonoBehaviour
{
    public float moveSpeed = 1f;
    private ThornSpawner spawner;
   // private Vector3 playerStartPosition; // Store the player's start position
    private bool gameEnded = false; // Track if the game has ended

    // Modify Initialize to accept both the ThornSpawner and the player's start position
    public void Initialize(ThornSpawner thornSpawner)
    {
        spawner = thornSpawner;
      //  playerStartPosition = startPos;
    }

    void Update()
    {
        if (spawner == null || gameEnded) return;

        float speed = GameManager.Instance != null ? GameManager.Instance.gameSpeed : moveSpeed;
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            ResetThorn();
            spawner.ReturnThorn(gameObject);
        }
    }


    // Check for collisions with the player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(gameEnded) return;

        Player player = other.gameObject.GetComponent<Player>();
        if (null != player) // Check if the thorn collides with the player
        {
            player.ResetToInitialPosition();
            // Optionally reset player position, if you want to restart the level from a specific position

            // Trigger a collision event
            // You game manager could listen to it

            // Trigger the game over function
            EndGame();
        }
    }

    // Reset the thorn's position and deactivate it before returning it to the pool
    public void ResetThorn()
    {
        transform.position = new Vector3(0, 6f, 0); // Example position; adjust as needed
        gameObject.SetActive(false);
    }

    // End the game (Stop time and show Game Over)
    // end game, timescale 0, all that logic
    // needs to be move to some sort of game manager
    void EndGame()
    {
        gameEnded = true;
        Time.timeScale = 0f; // Freeze the game
    }
}
