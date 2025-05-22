using UnityEngine;
using UnityEngine.Pool;
using System.Collections;
using System.Collections.Generic;

public class ThornSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject thornPrefab;
    [SerializeField] private float spawnInterval = 1.2f;
    [SerializeField] private float minSpawnInterval = 0.2f;
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
    private bool thornsStopped = false;

    void Start()
    {
        if (thornPrefab == null)
            return;

        InitializePool();
        StartSpawning();

        if (GameManager.Instance != null)
            GameManager.Instance.RegisterThornSpawner(this);
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
        thornsStopped = false;
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        while (!thornsStopped)
        {
            if (GameManager.Instance != null && !GameManager.Instance.isGameOver && spawnPositions.Length > 0)
            {
                UpdateSpawnInterval();
                SpawnThorns();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void UpdateSpawnInterval()
    {
        if (GameManager.Instance == null) return;
        float speed = GameManager.Instance.GameSpeed;
        spawnInterval = Mathf.Max(minSpawnInterval, 1.2f - (speed * 0.18f));
    }

    void SpawnThorns()
    {
        if (thornPool == null || spawnPositions.Length == 0) return;

        int thornsThisSpawn = Random.Range(1, 3);
        List<int> availablePositions = new List<int>();
        for (int i = 0; i < spawnPositions.Length; i++)
            availablePositions.Add(i);

        int count = Mathf.Min(thornsThisSpawn, availablePositions.Count);
        for (int i = 0; i < count; i++)
        {
            int randIndex = Random.Range(0, availablePositions.Count);
            int posIndex = availablePositions[randIndex];
            availablePositions.RemoveAt(randIndex);

            GameObject thorn = thornPool.Get();
            thorn.transform.position = transform.position + spawnPositions[posIndex];

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
    }

    IEnumerator ReturnThornAfterDelay(GameObject thorn, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnThorn(thorn);
    }

    public void ReturnThorn(GameObject thorn)
    {
        if (thorn != null && thornPool != null && thorn.activeSelf)
            thornPool.Release(thorn);
    }

    public void DecreaseSpawnInterval(float amount)
    {
        spawnInterval -= amount;
        if (spawnInterval < minSpawnInterval)
            spawnInterval = minSpawnInterval;
        StartSpawning();
    }

    public void HandleGameRestart()
    {
        foreach (var thorn in activeThorns.ToArray())
            ReturnThorn(thorn);
        StartSpawning();
    }

    public void StopAllThornsOnCollision()
    {
        if (thornsStopped) return;
        thornsStopped = true;

        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        foreach (var thornObj in activeThorns)
        {
            Thorn thornScript = thornObj.GetComponent<Thorn>();
            if (thornScript != null)
                thornScript.Freeze();
        }
    }

    void OnDestroy()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
    }
}
