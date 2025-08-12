using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int difficulty;

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

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void setDifficulty(int difficulty)
    {
        Instance.difficulty = difficulty;
        UIManager.Instance.CloseDifficultyPanel();

        Debug.Log("Difficulty set to: " + Instance.difficulty);

        SceneManager.LoadScene("MainScene");
    }
}
