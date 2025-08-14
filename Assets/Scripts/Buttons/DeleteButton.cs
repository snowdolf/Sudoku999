using UnityEngine;
using UnityEngine.UI;

public class DeleteButton : MonoBehaviour
{
    private Button deleteButton;

    void Start()
    {
        deleteButton = GetComponent<Button>();

        if (deleteButton != null )
        {
            deleteButton.onClick.AddListener(OnDeleteButtonClicked);
        }
    }

    private void OnDeleteButtonClicked()
    {
        UIManager.Instance.DeleteCellValue();
    }
}
