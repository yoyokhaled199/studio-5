using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] private float scrollFactor = 0.8f;
    private Renderer stemRenderer;
    private Material stemMat;

    void Start()
    {
        stemRenderer = GetComponent<Renderer>();
        if (stemRenderer == null)
        {
            Debug.LogError("No Renderer found on " + gameObject.name);
            return;
        }

        stemMat = stemRenderer.material;
        if (stemMat == null)
        {
            Debug.LogError("No Material found on renderer of " + gameObject.name);
        }
    }

    void Update()
    {
        if (stemMat == null || GameManager.Instance == null || GameManager.Instance.isGameOver)
            return;

        float gameSpeed = GameManager.Instance.GameSpeed;
        float yOffset = gameSpeed * scrollFactor * Time.deltaTime;

        Vector2 offset = stemMat.mainTextureOffset;
        offset.y += yOffset;

        stemMat.mainTextureOffset = offset;
    }
}
