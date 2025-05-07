using UnityEngine;

public class Scroll : MonoBehaviour
{
    public float speed = 1f; // Adjust to control scrolling speed
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
        if (stemMat == null) return;

        float speed = GameManager.Instance != null ? GameManager.Instance.gameSpeed *(float)0.8 : 1f;

        Vector2 offset = stemMat.mainTextureOffset;
        offset.y += speed * Time.deltaTime;

        //Debug.Log(offset.y); // Logs how much the texture has moved vertically

        stemMat.mainTextureOffset = offset;
    }


}
