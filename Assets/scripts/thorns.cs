using UnityEngine; 

public class Thorn : MonoBehaviour
{
    public float moveSpeed = 1f;
    private ThornSpawner spawner;
  
    private bool gameEnded = false; 

   
    public void Initialize(ThornSpawner thornSpawner)
    {
        spawner = thornSpawner;
     
    }

    void Update()
    {
        if (spawner == null || gameEnded) return;

        float speed = GameManager.Instance != null ? GameManager.Instance.GameSpeed * 10 : 1f;
        Vector3 movement = Vector3.down * speed * Time.deltaTime;

        transform.Translate(movement);

        if (transform.position.y < -30f)
        {
            ResetThorn();
            spawner.ReturnThorn(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(gameEnded) return;

        Player player = other.gameObject.GetComponent<Player>();
        if (null != player) 
        {
            
            

            EndGame();
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayLoseSound();
        }
    }

    public void ResetThorn()
    {
        transform.position = new Vector3(0, 6f, 0); 
        gameObject.SetActive(false);
    }


    void EndGame()
    {
        GameManager.Instance?.GameOver();

        gameEnded = true;
      
    }
}
