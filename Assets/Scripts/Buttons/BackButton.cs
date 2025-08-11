using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    private Button backButton;

    void Start()
    {
        backButton = GetComponent<Button>();

        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }
    }

    private void OnBackButtonClicked()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
