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

    private GameObject boardPrefab;
    private GameObject squarePrefab;
    private GameObject cellPrefab;

    private GameObject board;
    private GameObject[] cells = new GameObject[81];

    private GameObject inputBoardPrefab;
    private GameObject inputCellPrefab;

    private GameObject inputBoard;

    private GameObject optionBackgroundPanelPrefab;
    private GameObject optionPanelPrefab;

    private GameObject optionBackgroundPanel;
    private GameObject optionPanel;

    private GameObject resultPanelPrefab;
    private GameObject resultPanel;

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
                button.onClick.AddListener(() => GameManager.Instance.SetDifficulty(difficulty));
            }
        }
        
        CloseDifficultyPanel();
    }

    private void ConnectMainSceneUI()
    {
        canvas = GameObject.Find("Canvas");

        boardPrefab = Resources.Load<GameObject>("Main/Board");
        squarePrefab = Resources.Load<GameObject>("Main/Square");
        cellPrefab = Resources.Load<GameObject>("Main/Cell");

        board = Instantiate(boardPrefab, canvas.transform);
        for (int i = 0; i < 9; i++)
        {
            GameObject square = Instantiate(squarePrefab, board.transform);
            square.name = "Square" + i;

            for (int j = 0; j < 9; j++)
            {
                int idx = 9 * i + j;

                cells[idx] = Instantiate(cellPrefab, square.transform);
                cells[idx].name = "Cell" + idx;

                TMP_Text cellText = cells[idx].GetComponentInChildren<TMP_Text>();
                if (cellText != null)
                {
                    cellText.text = idx.ToString();
                }

                EventTrigger trigger = cells[idx].GetComponent<EventTrigger>();
                if (trigger != null)
                {
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerClick;
                    entry.callback.AddListener((data) => { ToggleCell(idx); });
                    trigger.triggers.Add(entry);
                }

                Cell cellComponent = cells[idx].GetComponent<Cell>();
                if (cellComponent != null)
                {
                    cellComponent.SetIdx(idx);
                }
            }
        }

        InitCellBackgroundColor();

        GameManager.Instance.SetRandomSudoku();
        InitCellValue();

        inputBoardPrefab = Resources.Load<GameObject>("Main/InputBoard");
        inputCellPrefab = Resources.Load<GameObject>("Main/InputCell");

        Button button = GameObject.Find("InputButton").GetComponent<Button>();
        inputBoard = Instantiate(inputBoardPrefab, button.transform);

        for (int i = 1; i <= 9; i++)
        {
            GameObject inputCell = Instantiate(inputCellPrefab, inputBoard.transform);
            inputCell.name = "InputCell" + i;

            TMP_Text cellText = inputCell.GetComponentInChildren<TMP_Text>();
            if (cellText != null)
            {
                cellText.text = i.ToString();
            }
        }

        CloseInputPanel();

        optionBackgroundPanelPrefab = Resources.Load<GameObject>("Main/OptionBackgroundPanel");
        optionPanelPrefab = Resources.Load<GameObject>("Main/OptionPanel");

        optionBackgroundPanel = Instantiate(optionBackgroundPanelPrefab, canvas.transform);
        optionPanel = Instantiate(optionPanelPrefab, canvas.transform);

        CloseOptionPanel();

        resultPanelPrefab = Resources.Load<GameObject>("Main/ResultPanel");

        resultPanel = Instantiate(resultPanelPrefab, canvas.transform);

        CloseResultPanel();
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

    public void OpenInputPanel()
    {
        OpenPanel(inputBoard);
    }

    public void OpenOptionPanel()
    {
        OpenPanel(optionPanel);
        OpenPanel(optionBackgroundPanel);
    }

    public void OpenResultPanel()
    {
        OpenPanel(resultPanel);
        OpenPanel(optionBackgroundPanel);
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

    public void CloseInputPanel()
    {
        ClosePanel(inputBoard);
    }

    public void CloseOptionPanel()
    {
        ClosePanel(optionPanel);
        ClosePanel(optionBackgroundPanel);
    }

    public void CloseResultPanel()
    {
        ClosePanel(resultPanel);
        ClosePanel(optionBackgroundPanel);
    }

    public void ToggleCell(int idx)
    {
        Cell cell = cells[idx].GetComponent<Cell>();
        if (cell != null)
        {
            if (!cell.isSelected)
            {
                InitCellBackgroundColor();
                ChangeCellBackgroundColor(idx, new Color(.85f, .85f, .85f), new Color(.85f, 1f, 1f), new Color(.5f, 1f, 1f), new Color(1f, .25f, .25f));
                cell.isSelected = true;
            }
            else
            {
                InitCellBackgroundColor();
            }
        }
    }

    private void ChangeCellBackgroundColor(int idx, Color nearColor, Color sameColor, Color selfColor, Color errorColor)
    {
        GameManager.Instance.selectedCellIdx = idx;

        Cell cell = cells[idx].GetComponent<Cell>();
        if (cell != null)
        {
            if (cell.state != CellState.Empty)
            {
                for (int i = 0; i < cells.Length; i++)
                {
                    if (i != idx)
                    {
                        Cell otherCell = cells[i].GetComponent<Cell>();
                        if (otherCell != null && otherCell.val == cell.val)
                        {
                            cells[i].GetComponent<Image>().color = sameColor;
                        }
                    }
                }
            }

            for (int i = 0; i < 9; i++)
            {
                int targetIdx = GameManager.Instance.cellIdx[cell.row, i];
                cells[targetIdx].GetComponent<Image>().color = cells[targetIdx].GetComponent<Image>().color == sameColor ? errorColor : nearColor;

                targetIdx = GameManager.Instance.cellIdx[i, cell.col];
                cells[targetIdx].GetComponent<Image>().color = cells[targetIdx].GetComponent<Image>().color == sameColor ? errorColor : nearColor;

                targetIdx = GameManager.Instance.squareIdx[cell.square, i];
                cells[targetIdx].GetComponent<Image>().color = cells[targetIdx].GetComponent<Image>().color == sameColor ? errorColor : nearColor;
            }
            
            cells[idx].GetComponent<Image>().color = selfColor;
        }
    }

    private void InitCellBackgroundColor()
    {
        GameManager.Instance.selectedCellIdx = -1;

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].GetComponent<Cell>().isSelected = false;
            cells[i].GetComponent<Image>().color = Color.white;
        }
    }

    private void InitCellValue()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Cell cell = cells[i].GetComponent<Cell>();
            if (cell != null)
            {
                cell.val = GameManager.Instance.cellValue[cell.row, cell.col];
                cell.state = cell.val == 0 ? CellState.Empty : CellState.Given;

                TMP_Text cellText = cells[i].GetComponentInChildren<TMP_Text>();
                if (cellText != null)
                {
                    cellText.text = cell.state == CellState.Given ? cell.val.ToString() : "";
                }
            }
        }
    }

    public void UpdateCellValue(int num)
    {
        if (GameManager.Instance.selectedCellIdx != -1)
        {
            GameManager.Instance.SaveHistoryToStack();

            Cell cell = cells[GameManager.Instance.selectedCellIdx].GetComponent<Cell>();
            if (cell != null && cell.state != CellState.Given)
            {
                cell.val = num;
                GameManager.Instance.cellValue[cell.row, cell.col] = cell.val;
                cell.state = CellState.Normal;
                TMP_Text cellText = cells[cell.idx].GetComponentInChildren<TMP_Text>();
                if (cellText != null)
                {
                    cellText.text = num.ToString();
                    cellText.color = Color.blue;
                }
                InitCellBackgroundColor();
                ChangeCellBackgroundColor(cell.idx, new Color(.85f, .85f, .85f), new Color(.85f, 1f, 1f), new Color(.5f, 1f, 1f), new Color(1f, .25f, .25f));
                cell.isSelected = true;
            }

            if (GameManager.Instance.CheckSudoku()) OpenResultPanel();
        }
    }

    public void DeleteCellValue()
    {
        if (GameManager.Instance.selectedCellIdx != -1)
        {
            GameManager.Instance.SaveHistoryToStack();

            Cell cell = cells[GameManager.Instance.selectedCellIdx].GetComponent<Cell>();
            if (cell != null && cell.state == CellState.Normal)
            {
                cell.val = 0;
                GameManager.Instance.cellValue[cell.row, cell.col] = cell.val;
                cell.state = CellState.Empty;
                TMP_Text cellText = cells[cell.idx].GetComponentInChildren<TMP_Text>();
                if (cellText != null)
                {
                    cellText.text = "";
                }
                InitCellBackgroundColor();
                ChangeCellBackgroundColor(cell.idx, new Color(.85f, .85f, .85f), new Color(.85f, 1f, 1f), new Color(.5f, 1f, 1f), new Color(1f, .25f, .25f));
                cell.isSelected = true;
            }
        }
    }

    public void UpdateBoardUI()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Cell cell = cells[i].GetComponent<Cell>();
            if (cell != null && cell.val != GameManager.Instance.cellValue[cell.row, cell.col])
            {
                cell.val = GameManager.Instance.cellValue[cell.row, cell.col];
                cell.state = cell.val == 0 ? CellState.Empty : CellState.Normal;

                TMP_Text cellText = cells[cell.idx].GetComponentInChildren<TMP_Text>();
                if (cellText != null)
                {
                    cellText.text = cell.val == 0 ? "" : cell.val.ToString();
                }
            }
        }

        InitCellBackgroundColor();
    }
}
