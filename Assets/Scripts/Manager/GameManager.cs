using System.Collections.Generic;
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

    private Stack<int[,]> historyStack = new Stack<int[,]>();

    private float elapsedTime;
    private bool isTimerRunning;
    private int hours, minutes, seconds;

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

    private void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 1f)
            {
                seconds++;
                elapsedTime -= 1f;

                if (seconds >= 60)
                {
                    minutes++;
                    seconds -= 60;
                }

                if (minutes >= 60)
                {
                    hours++;
                    minutes -= 60;
                }

                UIManager.Instance.timeText.SetTimeText();
            }
        }
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

        SceneManager.LoadScene("MainScene");
    }

    public void SetRandomSudoku()
    {
        SetRandomSolvedSudoku();
        RemoveNumbers();
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

    private void RemoveNumbers()
    {
        int numbersToRemove = 0;

        switch (difficulty)
        {
            case 1:
                numbersToRemove = 30;
                break;
            case 2:
                numbersToRemove = 40;
                break;
            case 3:
                numbersToRemove = 50;
                break;
            case 4:
                numbersToRemove = 55;
                break;
            case 5:
                numbersToRemove = 60;
                break;
        }

        var randomNumbers = Enumerable.Range(0, 81).OrderBy(x => Random.value).ToList();

        for (int i = 0; i < numbersToRemove; i++)
        {
            int row = randomNumbers[i] / 9;
            int col = randomNumbers[i] % 9;

            cellValue[row, col] = 0;
        }
    }

    public void SaveHistoryToStack()
    {
        int[,] tmpHistory = new int[9, 9];
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                tmpHistory[i, j] = cellValue[i, j];
            }
        }

        historyStack.Push(tmpHistory);
    }

    public void UndoHistory()
    {
        if (historyStack.Count > 0)
        {
            int[,] prevHistory = historyStack.Pop();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cellValue[i,j] = prevHistory[i, j];
                }
            }
            UIManager.Instance.UpdateBoardUI();
        }
    }

    public void ResetHistory()
    {
        while (historyStack.Count > 1)
        {
            historyStack.Pop();
        }

        UndoHistory();
    }

    public bool CheckSudoku()
    {
        for (int i = 0; i < 9; i++)
        {
            HashSet<int> rowSet = new HashSet<int>();
            for (int j = 0; j < 9; j++)
            {
                int val = cellValue[i,j];
                if (val == 0 || !rowSet.Add(val))
                {
                    return false;
                }
            }
        }

        for (int i = 0; i < 9; i++)
        {
            HashSet<int> colSet = new HashSet<int>();
            for (int j = 0; j < 9; j++)
            {
                int val = cellValue[j, i];
                if (val == 0 || !colSet.Add(val))
                {
                    return false;
                }
            }
        }

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                HashSet<int> boxSet = new HashSet<int>();
                for (int dx = 0; dx < 3; dx++)
                {
                    for (int dy = 0; dy < 3; dy++)
                    {
                        int val = cellValue[x * 3 + dx, y * 3 + dy];
                        if (val == 0 || !boxSet.Add(val))
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isTimerRunning = true;
        hours = minutes = seconds = 0;
    }

    public void PauseTimer()
    {
        isTimerRunning = false;
    }

    public void ResumeTimer()
    {
        isTimerRunning = true;
    }

    public string GetFormattedTime()
    {
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}
