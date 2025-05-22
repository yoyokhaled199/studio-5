using UnityEngine;

public class Thorn : MonoBehaviour
{
    public float moveSpeed = 1f;
    private ThornSpawner spawner;
    private bool gameEnded = false;
    private bool isFrozen = false;

    public void Initialize(ThornSpawner thornSpawner)
    {
        spawner = thornSpawner;
        gameEnded = false;
        isFrozen = false;
    }

    void Update()
    {
        if (spawner == null || gameEnded || isFrozen) return;

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
        if (gameEnded) return;
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
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

    public void Freeze()
    {
        isFrozen = true;
    }

    void EndGame()
    {
        GameManager.Instance?.GameOver();
        gameEnded = true;
        isFrozen = true;
        if (spawner != null)
            spawner.StopAllThornsOnCollision();
    }
}
