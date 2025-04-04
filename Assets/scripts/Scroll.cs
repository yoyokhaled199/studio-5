using UnityEngine;

public class Scroll : MonoBehaviour
{
    public float speed = 10f; // Adjust to control scrolling speed
    private Renderer stemRenderer;

    void Start()
    {
        // Get the Renderer component attached to this GameObject
        stemRenderer = GetComponent<Renderer>();

        if (stemRenderer == null)
        {
            Debug.LogError("No Renderer found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (stemRenderer != null && stemRenderer.material != null && stemRenderer.material.mainTexture != null)
        {
            // Get current offset
            Vector2 offset = stemRenderer.material.mainTextureOffset;

            // Modify Y offset for vertical scrolling
            offset.y += speed * Time.deltaTime;

            // Apply updated offset back to material
            stemRenderer.material.mainTextureOffset = offset;
        }
        else
        {
            Debug.LogWarning("Renderer or Main Texture is missing on " + gameObject.name);
        }
    }
}
