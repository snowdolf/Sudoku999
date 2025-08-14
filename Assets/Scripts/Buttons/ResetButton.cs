using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    private Button resetButton;

    void Start()
    {
        resetButton = GetComponent<Button>();

        if (resetButton != null )
        {
            resetButton.onClick.AddListener(OnResetButtonClicked);
        }
    }

    private void OnResetButtonClicked()
    {
        GameManager.Instance.ResetHistory();
        UIManager.Instance.CloseOptionPanel();
    }
}
