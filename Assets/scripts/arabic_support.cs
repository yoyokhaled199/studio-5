using UnityEngine;
using TMPro; // Or use UnityEngine.UI for UI.Text
using ArabicSupport;

public class ArabicDemo : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Assign in Inspector
    [TextArea]
    public string arabicText = "مرحبا بك في لعبتنا!";

    void Start()
    {
        // Fix and display the Arabic text with correct shaping and RTL order
        textComponent.text = ArabicFixer.Fix(arabicText);
    }
}
