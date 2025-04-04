using UnityEngine; // Add this line at the top if it's not already present

public class Thorn : MonoBehaviour
{
    public float moveSpeed = 3f;
    private ThornSpawner spawner;
    private Vector3 playerStartPosition; // Store the player's start position
    private bool gameEnded = false; // Track if the game has ended

    // Modify Initialize to accept both the ThornSpawner and the player's start position
    public void Initialize(ThornSpawner thornSpawner, Vector3 startPos)
    {
        spawner = thornSpawner;
        playerStartPosition = startPos;
    }

    void Update()
    {
        if (spawner == null || gameEnded) return; // Safety check and don't update if game ended

        // Move the thorn downward
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        // Check if the thorn has gone off the screen and return it to the pool
        if (transform.position.y < -6f) // Adjust this based on your screen size
        {
            ResetThorn();
            spawner.ReturnThorn(gameObject); // Return the thorn to the pool
        }
    }

    // Check for collisions with the player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !gameEnded) // Check if the thorn collides with the player
        {
            // Optionally reset player position, if you want to restart the level from a specific position
            other.transform.position = playerStartPosition;

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
    void EndGame()
    {
        gameEnded = true;
        Time.timeScale = 0f; // Freeze the game
    }
}
