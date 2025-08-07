using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject difficultyPanel;
    public GameObject difficultyBackgroundPanel;
    public Animator difficultyPanelAnimator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenPanel(GameObject panel, Animator animator = null, string trigger = null)
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
        if (animator != null && trigger != null)
        {
            animator.SetTrigger(trigger);
        }
    }

    public void OpenDifficultyPanel()
    {
        OpenPanel(difficultyPanel, difficultyPanelAnimator, "OpenPanel");
        OpenPanel(difficultyBackgroundPanel);
    }

    public void ClosePanel(GameObject panel, Animator animator = null, string trigger = null)
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
        if (animator != null && trigger != null)
        {
            animator.SetTrigger(trigger);
        }
    }

    public void CloseDifficultyPanel()
    {
        ClosePanel(difficultyPanel);
        ClosePanel(difficultyBackgroundPanel);
    }
}
