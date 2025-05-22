using UnityEngine;
using TMPro; 
using ArabicSupport;

public class ArabicDemo : MonoBehaviour
{
    public TextMeshProUGUI textComponent; 
    [TextArea]
    public string arabicText = "مرحبا بك في لعبتنا!";

    void Start()
    {
        textComponent.text = ArabicFixer.Fix(arabicText);
    }
}
