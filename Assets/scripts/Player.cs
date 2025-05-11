using UnityEngine;

public class Player : MonoBehaviour
{
    Vector2 initialPos;

    void Awake()
    {
        initialPos = transform.position;
    }

    public void ResetToInitialPosition()
    {
        transform.position = initialPos;
    }

}
