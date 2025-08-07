using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject difficultyPanel;
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

    public void OpenPanel(GameObject panel, Animator animator, string trigger)
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
        if (animator != null)
        {
            animator.SetTrigger(trigger);
        }
    }

    public void OpenDifficultyPanel()
    {
        OpenPanel(difficultyPanel, difficultyPanelAnimator, "OpenPanel");
    }
}
