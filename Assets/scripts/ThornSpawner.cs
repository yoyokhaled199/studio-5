using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class ThornSpawner : MonoBehaviour
{
    public GameObject thornPrefab; // Thorn prefab reference

    // Sprites for different thorn positions
    public Sprite[] leftThornSprites;
    public Sprite[] middleThornSprites;
    public Sprite[] rightThornSprites;

    private ObjectPool<GameObject> thornPool; // Object pool for thorn management
    private Vector3[] spawnPositions; // Spawn positions for thorns
    private Transform playerTransform; // Reference to the player

    void Start()
    {
        // Get the player reference
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Set spawn positions relative to the ThornSpawner
        spawnPositions = new Vector3[] {
            new Vector3(-2f, 0f, 0f), // Left
            new Vector3(0f, 0f, 0f),  // Middle
            new Vector3(2f, 0f, 0f)   // Right
        };

        // Check if the thornPrefab is assigned
        if (thornPrefab == null)
        {
            Debug.LogError("Thorn Prefab is not assigned!");
            return;
        }

        // Initialize the object pool for thorns
        thornPool = new ObjectPool<GameObject>(
            createFunc: () => InstantiateThorn(),
            actionOnGet: (thorn) => thorn.SetActive(true),
            actionOnRelease: (thorn) => thorn.SetActive(false),
            actionOnDestroy: (thorn) => Destroy(thorn),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );

        // Start spawning thorns periodically
        InvokeRepeating(nameof(SpawnThorn), 2f, 2f); // Spawns every 2 seconds
    }

    // Instantiates a new thorn and ensures it has a Thorn component
    GameObject InstantiateThorn()
    {
        GameObject thorn = Instantiate(thornPrefab);
        Thorn thornScript = thorn.GetComponent<Thorn>();

        // Ensure Thorn script is attached
        if (thornScript == null)
        {
            thornScript = thorn.AddComponent<Thorn>();
        }

        return thorn;
    }

    // Spawns a thorn at a random position and assigns the correct sprite
    void SpawnThorn()
    {
        GameObject thorn = thornPool.Get(); // Get thorn from pool

        // Choose a random spawn position (left, middle, right)
        int positionIndex = Random.Range(0, spawnPositions.Length);
        Vector3 spawnPosition = spawnPositions[positionIndex];
        thorn.transform.position = transform.position + spawnPosition;

        // Assign the correct sprite if available
        SpriteRenderer spriteRenderer = thorn.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Sprite selectedSprite = GetRandomThornSprite(positionIndex);
            if (selectedSprite != null)
            {
                spriteRenderer.sprite = selectedSprite;
            }
            else
            {
                Debug.LogWarning("No sprite available for position index " + positionIndex);
            }
        }

        // Ensure the thorn knows how to reset the player if hit
        Thorn thornScript = thorn.GetComponent<Thorn>();
        if (thornScript != null && playerTransform != null)
        {
            thornScript.Initialize(this, playerTransform.position);
        }

        // Return the thorn to the pool after 5 seconds
        StartCoroutine(ReturnThornToPool(thorn, 5f));
    }

    // Returns a random sprite based on the spawn position
    private Sprite GetRandomThornSprite(int positionIndex)
    {
        // Make sure arrays are not empty and indices are valid
        if (positionIndex == 0 && leftThornSprites.Length > 0)
        {
            return leftThornSprites[Random.Range(0, leftThornSprites.Length)];
        }
        else if (positionIndex == 1 && middleThornSprites.Length > 0)
        {
            return middleThornSprites[Random.Range(0, middleThornSprites.Length)];
        }
        else if (positionIndex == 2 && rightThornSprites.Length > 0)
        {
            return rightThornSprites[Random.Range(0, rightThornSprites.Length)];
        }

        return null; // Return null if no sprite available
    }

    // Returns the thorn to the pool after a delay
    IEnumerator ReturnThornToPool(GameObject thorn, float delay)
    {
        yield return new WaitForSeconds(delay);
        thornPool.Release(thorn); // Return thorn to pool after delay
    }

    // Public function to return thorn manually
    public void ReturnThorn(GameObject thorn)
    {
        thornPool.Release(thorn); // Return thorn to pool
    }
}
