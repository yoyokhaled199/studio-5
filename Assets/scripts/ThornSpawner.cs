using UnityEngine;
using UnityEngine.Pool;
using System.Collections;
using System.Collections.Generic;

public class ThornSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject thornPrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float thornLifetime = 5f;

    [Header("Spawn Positions")]
    [SerializeField]
    private Vector3[] spawnPositions = new Vector3[]
    {
        new Vector3(-2f, 0f, 0f),
        new Vector3(0f, 0f, 0f),
        new Vector3(2.5f, 0f, 0f)
    };

    [Header("Sprites")]
    [SerializeField] private Sprite[] leftThornSprites;
    [SerializeField] private Sprite[] middleThornSprites;
    [SerializeField] private Sprite[] rightThornSprites;

    private ObjectPool<GameObject> thornPool;
    private List<GameObject> activeThorns = new List<GameObject>();
    private Coroutine spawnCoroutine;

    void Start()
    {
        if (thornPrefab == null)
        {
            Debug.LogError("Thorn prefab is not assigned!");
            return;
        }

        InitializePool();
        StartSpawning();

        if (GameManager.Instance != null)
            GameManager.Instance.RegisterThornSpawner(this);
        else
            Debug.LogWarning("GameManager.Instance is null in ThornSpawner.");
    }

    void InitializePool()
    {
        thornPool = new ObjectPool<GameObject>(
            createFunc: () => {
                GameObject thorn = Instantiate(thornPrefab);
                Thorn thornScript = thorn.GetComponent<Thorn>() ?? thorn.AddComponent<Thorn>();
                thornScript.Initialize(this);
                return thorn;
            },
            actionOnGet: (thorn) => {
                thorn.SetActive(true);
                activeThorns.Add(thorn);
            },
            actionOnRelease: (thorn) => {
                thorn.SetActive(false);
                activeThorns.Remove(thorn);
            },
            actionOnDestroy: (thorn) => Destroy(thorn),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );
    }

    void StartSpawning()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }


    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(1f); // Initial delay

        while (true)
        {
            if (GameManager.Instance != null && !GameManager.Instance.isGameOver && spawnPositions.Length > 0)
            {
                SpawnThorn();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnThorn()
    {
        if (thornPool == null || spawnPositions.Length == 0) return;

        GameObject thorn = thornPool.Get();
        int posIndex = Random.Range(0, spawnPositions.Length);
        thorn.transform.position = transform.position + spawnPositions[posIndex];

        // Safe sprite assignment
        SpriteRenderer renderer = thorn.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            switch (posIndex)
            {
                case 0:
                    if (leftThornSprites.Length > 0)
                        renderer.sprite = leftThornSprites[Random.Range(0, leftThornSprites.Length)];
                    break;
                case 1:
                    if (middleThornSprites.Length > 0)
                        renderer.sprite = middleThornSprites[Random.Range(0, middleThornSprites.Length)];
                    break;
                case 2:
                    if (rightThornSprites.Length > 0)
                        renderer.sprite = rightThornSprites[Random.Range(0, rightThornSprites.Length)];
                    break;
                default:
                    renderer.sprite = null;
                    break;
            }
        }

        StartCoroutine(ReturnThornAfterDelay(thorn, thornLifetime));
    }

    IEnumerator ReturnThornAfterDelay(GameObject thorn, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnThorn(thorn);
    }

    public void ReturnThorn(GameObject thorn)
    {
        if (thorn != null && thornPool != null && thorn.activeSelf)
        {
            thornPool.Release(thorn);
        }
    }

    public void HandleGameRestart()
    {
        foreach (var thorn in activeThorns.ToArray())
            ReturnThorn(thorn);

        StartSpawning(); // ✅ This MUST start the coroutine again
    }


    void OnDestroy()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
    }
}
