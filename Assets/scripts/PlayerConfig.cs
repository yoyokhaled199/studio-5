using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig", order = 0)]
public class PlayerConfig : ScriptableObject
{
    [Header("Middle Sprites (e.g. idle, moving)")]
    public Sprite[] middleSprites;
    [Header("Right Sprites (e.g. idle, moving)")]
    public Sprite[] rightSprites;
    [Header("Left Sprites (e.g. idle, moving)")]
    public Sprite[] leftSprites;
}
