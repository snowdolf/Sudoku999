using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int difficulty;

    public int[,] cellIdx = new int[9, 9];

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

    private void Start()
    {

    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetDifficulty(int difficulty)
    {
        Instance.difficulty = difficulty;
        UIManager.Instance.CloseDifficultyPanel();

        Debug.Log("Difficulty set to: " + Instance.difficulty);

        SceneManager.LoadScene("MainScene");
    }
}
