using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    private Button quitButton;

    void Start()
    {
        quitButton = GetComponent<Button>();

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnStartButtonClicked);
        }
    }

    private void OnStartButtonClicked()
    {
        GameManager.Instance.QuitGame();
    }
}
