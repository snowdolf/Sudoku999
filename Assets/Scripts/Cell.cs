using UnityEngine;

public enum CellState
{
    Empty,
    Given,
    Normal
}

public class Cell : MonoBehaviour
{
    public int idx;

    public int row;
    public int col;
    public int square;

    public int val;
    public CellState state;

    public bool isSelected;

    public bool[] memo = new bool[9];

    private void Start()
    {
        isSelected = false;
        for (int i = 0; i < memo.Length; i++)
        {
            memo[i] = false;
        }
    }

    public void SetIdx(int n)
    {
        idx = n;

        square = n / 9;

        int x = square / 3 * 3;
        int y = square % 3 * 3;

        row = x + n % 9 / 3;
        col = y + n % 3;
    }
}
