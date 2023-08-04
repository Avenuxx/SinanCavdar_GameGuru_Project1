using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    GameManager manager;
    UIManager uiManager;

    [Header("GameObjects")]
    public GameObject squarePrefab;

    [Space(10)]
    [Header("Ints & Floats")]
    public int gridSize;
    public float gridSpace;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        CreateSquare();
    }

    private void OnGenerateGrid()
    {
        if (string.IsNullOrEmpty(uiManager.texts.inputText.text))
            return;

        ClearOldGrid();
        SetSize();
        CreateSquare();
    }

    void ClearOldGrid()
    {
        foreach (GameObject square in manager.lists.currentSquares)
        {
            Destroy(square);
        }
        manager.lists.currentSquares.Clear();
    }

    void SetSize()
    {
        gridSize = int.Parse(uiManager.texts.inputText.text);
        gridSpace = 1 / (float)(gridSize * .65f);
    }

    void CreateSquare()
    {
        float screenHeight = (Camera.main.orthographicSize - 0.5f) * 2.0f;
        float screenWidth = screenHeight * Camera.main.aspect;

        float squareSize = (Mathf.Min(screenHeight, screenWidth) - gridSpace * (gridSize - 1)) / gridSize;

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                GameObject square = Instantiate(squarePrefab, transform);
                square.name = "x_" + col + "," + "y_" + row;

                SquareController squareController = square.GetComponent<SquareController>();

                squareController.xPosition = col;
                squareController.yPosition = row;

                float offsetX = (gridSize * squareSize + gridSpace * (gridSize - 1)) / 2 - squareSize / 2;
                float offsetY = (gridSize * squareSize + gridSpace * (gridSize - 1)) / 2 - squareSize / 2;

                square.transform.position = new Vector3(col * (squareSize + gridSpace) - offsetX, row * (squareSize + gridSpace) - offsetY + 1, 10f);

                square.transform.localScale = new Vector3(squareSize, squareSize, 1f);

                manager.lists.currentSquares.Add(square);
            }
        }
    }


    ////////////////////////////// EVENTS ////////////////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnGenerateGrid, OnGenerateGrid);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnGenerateGrid, OnGenerateGrid);
    }
}