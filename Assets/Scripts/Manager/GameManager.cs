using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int difficulty;

    // ith row = [i - 1, _], jth col = [_, j - 1]
    public int[,] cellIdx = new int[9, 9];
    public int[,] squareIdx = new int[9, 9];

    public int selectedCellIdx = -1;

    public int[,] cellValue = new int[9, 9];

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

    public void SetRandomSudoku()
    {
        SetRandomSolvedSudoku();
    }

    private void SetRandomSolvedSudoku()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                cellValue[i, j] = 0;
            }
        }

        SolveSudoku(0, 0);
    }

    private bool SolveSudoku(int row, int col)
    {
        if (row == 9)
        {
            return true;
        }
        if (col == 9)
        {
            return SolveSudoku(row + 1, 0);
        }

        var randomNumbers = Enumerable.Range(1, 9).OrderBy(x => Random.value).ToList();

        foreach (int num in randomNumbers)
        {
            if (IsValid(row, col, num))
            {
                cellValue[row, col] = num;
                if (SolveSudoku(row, col + 1))
                {
                    return true;
                }
            }
            cellValue[row, col] = 0;
        }

        return false;
    }

    private bool IsValid(int row, int col, int num)
    {
        for (int i = 0; i < 9; i++)
        {
            if (cellValue[row, i] == num || cellValue[i, col] == num)
            {
                return false;
            }
        }

        int squareRow = row / 3 * 3;
        int squareCol = col / 3 * 3;

        for (int i = squareRow; i < squareRow + 3; i++)
        {
            for (int j = squareCol; j < squareCol + 3; j++)
            {
                if (cellValue[i, j] == num)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
