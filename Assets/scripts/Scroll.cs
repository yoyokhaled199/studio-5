using UnityEngine;

public class Scroll : MonoBehaviour
{
    public float speed = 10f; 
    private Renderer stemRenderer;

    void Start()
    {
   
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
         
            Vector2 offset = stemRenderer.material.mainTextureOffset;

         
            offset.y += speed * Time.deltaTime;

          
            stemRenderer.material.mainTextureOffset = offset;
        }
        else
        {
            Debug.LogWarning("Renderer or Main Texture is missing on " + gameObject.name);
        }
    }
}
