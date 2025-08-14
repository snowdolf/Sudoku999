using UnityEngine;
using UnityEngine.UI;

public class UndoButton : MonoBehaviour
{
    private Button undoButton;

    void Start()
    {
        undoButton = GetComponent<Button>();

        if (undoButton != null)
        {
            undoButton.onClick.AddListener(OnUndoButtonClicked);
        }
    }

    private void OnUndoButtonClicked()
    {
        GameManager.Instance.UndoHistory();
    }
}
