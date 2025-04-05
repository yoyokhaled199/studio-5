using UnityEngine;

public class Scroll : MonoBehaviour
{
    public float speed = 10f; // Adjust to control scrolling speed
    private Renderer stemRenderer;
    private Material stemMat;

    void Start()
    {
        // Get the Renderer component attached to this GameObject
        stemRenderer = GetComponent<Renderer>();

        if (stemRenderer == null)
        {
            Debug.LogError("No Renderer found on " + gameObject.name);
        }
        else
        {
            stemMat = stemRenderer.material;
        }
    }

    void Update()
    {
        if(stemMat == null) return;

        // Get current offset
        Vector2 offset = stemMat.mainTextureOffset;

        // Modify Y offset for vertical scrolling
        offset.y += speed * Time.deltaTime;

        // Apply updated offset back to material
        stemMat.mainTextureOffset = offset;

    }
}
