using TMPro;
using UnityEngine;

public class DifficultyText : MonoBehaviour
{
    TMP_Text difficultyText;

    void Start()
    {
        difficultyText = GetComponent<TMP_Text>();
        if (difficultyText != null )
        {
            difficultyText.text = "���̵�\n" + GameManager.Instance.difficulty.ToString();
        }
    }
}
