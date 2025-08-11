using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private GameObject canvas;

    private GameObject difficultyBackgroundPanelPrefab;
    private GameObject difficultyPanelPrefab;
    private GameObject difficultyButtonPrefab;

    private GameObject difficultyBackgroundPanel;
    private GameObject difficultyPanel;
    private Animator difficultyPanelAnimator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LobbyScene")
        {
            ConnectLobbySceneUI();
        }
        else if (scene.name == "MainScene")
        {
            ConnectMainSceneUI();    
        }
    }

    private void ConnectLobbySceneUI()
    {
        canvas = GameObject.Find("Canvas");

        difficultyBackgroundPanelPrefab = Resources.Load<GameObject>("Lobby/DifficultyBackgroundPanel");
        difficultyPanelPrefab = Resources.Load<GameObject>("Lobby/DifficultyPanel");
        difficultyButtonPrefab = Resources.Load<GameObject>("Lobby/DifficultyButton");

        difficultyBackgroundPanel = Instantiate(difficultyBackgroundPanelPrefab, canvas.transform);
        difficultyPanel = Instantiate(difficultyPanelPrefab, canvas.transform);
        difficultyPanelAnimator = difficultyPanel.GetComponent<Animator>();

        EventTrigger trigger = difficultyBackgroundPanel.GetComponent<EventTrigger>();
        if (trigger != null)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { CloseDifficultyPanel(); });
            trigger.triggers.Add(entry);
        }

        for (int i = 1; i <= 5; i++)
        {
            GameObject difficultyButton = Instantiate(difficultyButtonPrefab, difficultyPanel.transform);
            difficultyButton.name = "DifficultyButton" + i;

            TMP_Text buttonText = difficultyButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = i.ToString();
            }

            Button button = difficultyButton.GetComponent<Button>();
            if (button != null)
            {
                int difficulty = i;
                button.onClick.AddListener(() => GameManager.Instance.setDifficulty(difficulty));
            }
        }
        
        CloseDifficultyPanel();
    }

    private void ConnectMainSceneUI()
    {
        // asdf
    }

    private void OpenPanel(GameObject panel, Animator animator = null, string trigger = null)
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

    private void ClosePanel(GameObject panel, Animator animator = null, string trigger = null)
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
