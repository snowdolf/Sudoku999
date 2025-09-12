using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using Random = UnityEngine.Random;

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

        SetRandomSudoku(() =>
        {
            StartCoroutine(LoadMainSceneAsync());
        });
    }

    private IEnumerator LoadMainSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void SetRandomSudoku(Action onComplete)
    {
        StartCoroutine(LoadAndSetSudoku(onComplete));
    }
    private IEnumerator LoadAndSetSudoku(Action onComplete)
    {
        string uriPath = GetFilePath();

        using (UnityWebRequest www = UnityWebRequest.Get(uriPath))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to load file for difficulty {difficulty}");
                yield break;
            }

            SetPuzzleFromLines(www.downloadHandler.text.Split('\n'));

            yield return null;

            onComplete?.Invoke();
        }
    }

    private string GetFilePath()
    {
        string directory = Application.streamingAssetsPath;
        return Path.Combine(directory, $"sudoku_difficulty_{difficulty}.csv");
    }

    private void SetPuzzleFromLines(string[] lines)
    {
        if (lines.Length == 0)
        {
            Debug.LogError($"Error: No file found in the file for difficulty {difficulty}");
            return;
        }

        string randomLine = lines[Random.Range(0, lines.Length)];
        string[] data = randomLine.Split(',');
        string sudokuString = data[1];
        string solutionString = data[2];

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                char c = sudokuString[i * 9 + j];
                cellValue[i, j] = c == '.' ? 0 : c - '0';
            }
        }

        int cellsToFill = 15 - 3 * difficulty;
        List<int> emptyCells = new List<int>();

        for (int i = 0; i < 81; i++)
        {
            if (cellValue[i / 9, i % 9] == 0)
            {
                emptyCells.Add(i);
            }
        }

        for (int i = 0; i < cellsToFill && emptyCells.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, emptyCells.Count);
            int cellIdx = emptyCells[randomIndex];
            emptyCells.RemoveAt(randomIndex);
            cellValue[cellIdx / 9, cellIdx % 9] = solutionString[cellIdx] - '0';
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
