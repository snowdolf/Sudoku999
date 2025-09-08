using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    private TMP_Text loadingText;
    private int dotCount;
    private float elapsedTime;

    void Start()
    {
        loadingText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        dotCount = 1;
        elapsedTime = 0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 0.2f)
        {
            dotCount = (dotCount % 3) + 1;

            loadingText.text = "Loading" + new string('.', dotCount);

            elapsedTime = 0f;
        }
    }
}
