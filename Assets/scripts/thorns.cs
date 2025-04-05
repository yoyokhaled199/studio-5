using UnityEngine; 

public class Thorn : MonoBehaviour
{
    public float moveSpeed = 3f;
    private ThornSpawner spawner;
    private Vector3 playerStartPosition; 
    private bool gameEnded = false; 

    public void Initialize(ThornSpawner thornSpawner, Vector3 startPos)
    {
        spawner = thornSpawner;
        playerStartPosition = startPos;
    }

    void Update()
    {
        if (spawner == null || gameEnded) return; 

        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        if (transform.position.y < -6f) 
        {
            ResetThorn();
            spawner.ReturnThorn(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !gameEnded)
        {
            other.transform.position = playerStartPosition;

            EndGame();
        }
    }

   
    public void ResetThorn()
    {
        transform.position = new Vector3(0, 6f, 0); 
        gameObject.SetActive(false);
    }


    void EndGame()
    {
        gameEnded = true;
        Time.timeScale = 0f; 
    }
}
