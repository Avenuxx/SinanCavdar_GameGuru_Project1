using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    GameManager manager;
    UIManager uiManager;

    public GameObject squarePrefab;
    public int gridSize;
    public float padding;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        OnGenerateGrid();
    }

    private void OnGenerateGrid()
    {
        if (uiManager.texts.inputText.text == string.Empty)
            return;

        ClearOldGrid();

        SetSize();

        CreateSquare();
    }

    void ClearOldGrid()
    {
        if (manager.lists.currentSquares.Count == 0)
            return;

        int listCount = manager.lists.currentSquares.Count;
        for (int i = 0; i < listCount; i++)
        {
            Destroy(manager.lists.currentSquares[i]);
        }

        manager.lists.currentSquares.Clear();
    }

    void SetSize()
    {
        gridSize = int.Parse(uiManager.texts.inputText.text);

        padding = 1 / (float)(gridSize * .65f);
    }

    void CreateSquare()
    {
        float screenHeight = Camera.main.orthographicSize * 2.0f;
        float screenWidth = screenHeight * Camera.main.aspect;

        float squareSize = (Mathf.Min(screenHeight, screenWidth) - padding * (gridSize - 1)) / gridSize;

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                GameObject square = Instantiate(squarePrefab, transform);
                square.name = "x_" + col + "," + "y_" + row;

                SquareController squareController = square.GetComponent<SquareController>();

                squareController.xPosition = col;
                squareController.yPosition = row;

                float offsetX = (gridSize * squareSize + padding * (gridSize - 1)) / 2 - squareSize / 2;
                float offsetY = (gridSize * squareSize + padding * (gridSize - 1)) / 2 - squareSize / 2;

                square.transform.position = new Vector3(col * (squareSize + padding) - offsetX, row * (squareSize + padding) - offsetY + 1, 10f);

                square.transform.localScale = new Vector3(squareSize, squareSize, 1f);

                manager.lists.currentSquares.Add(square);
            }
        }
    }



    ////////////////////////////// EVENTS ////////////////////////////
    public void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnGenerateGrid, OnGenerateGrid);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnGenerateGrid, OnGenerateGrid);
    }
}