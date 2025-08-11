using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button startButton;

    void Start()
    {
        startButton = GetComponent<Button>();

        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
    }

    private void OnStartButtonClicked()
    {
        UIManager.Instance.OpenDifficultyPanel();
    }
}
