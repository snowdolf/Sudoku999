using TMPro;
using UnityEngine;

public class DifficultyText : MonoBehaviour
{
    TMP_Text difficultyText;

    public bool isVertical = true;

    void Start()
    {
        difficultyText = GetComponent<TMP_Text>();
        if (difficultyText != null )
        {
            difficultyText.text = "≥≠¿Ãµµ" + (isVertical ? "\n" : " ") + GameManager.Instance.difficulty.ToString();
        }
    }
}
