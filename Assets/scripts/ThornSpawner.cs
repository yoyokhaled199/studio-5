using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class ThornSpawner : MonoBehaviour
{
    public GameObject thornPrefab;

    public Sprite[] leftThornSprites;
    public Sprite[] middleThornSprites;
    public Sprite[] rightThornSprites;

    private ObjectPool<GameObject> thornPool;
    private Vector3[] spawnPositions; 
    private Transform playerTransform; 

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        spawnPositions = new Vector3[] {
            new Vector3(-2f, 0f, 0f),
            new Vector3(0f, 0f, 0f), 
            new Vector3(2f, 0f, 0f)   
        };

        if (thornPrefab == null)
        {
            Debug.LogError("Thorn Prefab is not assigned!");
            return;
        }

        thornPool = new ObjectPool<GameObject>(
            createFunc: () => InstantiateThorn(),
            actionOnGet: (thorn) => thorn.SetActive(true),
            actionOnRelease: (thorn) => thorn.SetActive(false),
            actionOnDestroy: (thorn) => Destroy(thorn),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );

        InvokeRepeating(nameof(SpawnThorn), 2f, 2f); 
    }


    GameObject InstantiateThorn()
    {
        GameObject thorn = Instantiate(thornPrefab);
        Thorn thornScript = thorn.GetComponent<Thorn>();

        if (thornScript == null)
        {
            thornScript = thorn.AddComponent<Thorn>();
        }

        return thorn;
    }

    void SpawnThorn()
    {
        GameObject thorn = thornPool.Get();

        int positionIndex = Random.Range(0, spawnPositions.Length);
        Vector3 spawnPosition = spawnPositions[positionIndex];
        thorn.transform.position = transform.position + spawnPosition;

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

        Thorn thornScript = thorn.GetComponent<Thorn>();
        if (thornScript != null && playerTransform != null)
        {
            thornScript.Initialize(this, playerTransform.position);
        }

        StartCoroutine(ReturnThornToPool(thorn, 5f));
    }

    private Sprite GetRandomThornSprite(int positionIndex)
    {
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

        return null;
    }

    IEnumerator ReturnThornToPool(GameObject thorn, float delay)
    {
        yield return new WaitForSeconds(delay);
        thornPool.Release(thorn); 
    }

    public void ReturnThorn(GameObject thorn)
    {
        thornPool.Release(thorn); 
    }
}
