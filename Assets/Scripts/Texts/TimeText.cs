using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour
{
    private TMP_Text timeText;
    public bool isVertical = true;

    void Start()
    {
        timeText = GetComponent<TMP_Text>();
        if (timeText != null)
        {
            SetTimeText();
        }
        UIManager.Instance.timeText = this;
    }

    public void SetTimeText()
    {
        timeText.text = "½Ã°£" + (isVertical ? "\n" : " ") + GameManager.Instance.GetFormattedTime();
    }
}
