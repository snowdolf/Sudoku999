using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int difficulty;

    // ith row = [i - 1, _], jth col = [_, j - 1]
    public int[,] cellIdx = new int[9, 9];
    public int[,] squareIdx = new int[9, 9];

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
        SetIdx();
    }

    private void SetIdx()
    {
        for (int i = 0; i < 9; i++)
        {
            int x = i / 3 * 3;
            int y = i % 3 * 3;

            for (int j = 0; j < 9; j++)
            {
                int idx = i * 9 + j;
                cellIdx[x + j / 3, y + j % 3] = idx;
                squareIdx[i, j] = idx;
            }
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

    public void SetDifficulty(int difficulty)
    {
        Instance.difficulty = difficulty;
        UIManager.Instance.CloseDifficultyPanel();

        Debug.Log("Difficulty set to: " + Instance.difficulty);

        SceneManager.LoadScene("MainScene");
    }
}
