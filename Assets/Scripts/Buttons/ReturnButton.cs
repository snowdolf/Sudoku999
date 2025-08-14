using UnityEngine;
using UnityEngine.UI;

public class ReturnButton : MonoBehaviour
{
    private Button returnButton;

    void Start()
    {
        returnButton = GetComponent<Button>();
        if (returnButton != null)
        {
            returnButton.onClick.AddListener(OnReturnButtonClicked);
        }
    }

    private void OnReturnButtonClicked()
    {
        UIManager.Instance.CloseOptionPanel();
    }
}
