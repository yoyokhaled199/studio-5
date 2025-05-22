using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private PlayerConfig antConfig;
    [SerializeField] private PlayerConfig wormConfig;

    [SerializeField] private GameObject antHighlight;
    [SerializeField] private GameObject wormHighlight;

    [SerializeField] private GameObject openingSpriteUI; 


    private string selectedCharacter = "ant";

    void Start()
    {
        StartCoroutine(ShowOpeningSprite());
    }

    private IEnumerator ShowOpeningSprite()
    {
        if (openingSpriteUI != null) openingSpriteUI.SetActive(true);
        if (antHighlight != null) antHighlight.SetActive(false);
        if (wormHighlight != null) wormHighlight.SetActive(false);

        yield return new WaitForSeconds(1f);

        if (openingSpriteUI != null) openingSpriteUI.SetActive(false);

        string savedCharacter = PlayerPrefs.GetString("SelectedCharacter", "ant");
        selectedCharacter = savedCharacter;

        UpdateSelectionVisuals();

    }

    public void SelectAnt()
    {
        selectedCharacter = "ant";
        PlayerPrefs.SetString("SelectedCharacter", selectedCharacter);
        UpdateSelectionVisuals();
    }

    public void SelectWorm()
    {
        selectedCharacter = "worm";
        PlayerPrefs.SetString("SelectedCharacter", selectedCharacter);
        UpdateSelectionVisuals();
    }

    private void UpdateSelectionVisuals()
    {
        if (antHighlight != null && wormHighlight != null)
        {
            antHighlight.SetActive(selectedCharacter == "ant");
            wormHighlight.SetActive(selectedCharacter == "worm");
        }
    }

    public void PlayGame()
    {
        PlayerPrefs.SetString("SelectedCharacter", selectedCharacter);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
