using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
    private Button optionButton;

    void Start()
    {
        optionButton = GetComponent<Button>();

        if (optionButton != null)
        {
            optionButton.onClick.AddListener(OnOptionButtonClicked);
        }
    }

    private void OnOptionButtonClicked()
    {
        UIManager.Instance.OpenOptionPanel();
    }
}
