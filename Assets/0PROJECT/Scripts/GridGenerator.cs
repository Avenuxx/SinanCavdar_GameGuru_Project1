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
        //DEFAULT GENERATED GRID WITH 4X4 SQUARES
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
        //CLEAR LAST SQUARES
        foreach (GameObject square in manager.lists.currentSquares)
        {
            Destroy(square);
        }
        manager.lists.currentSquares.Clear();
    }

    void SetSize()
    {
        //SET THE SIZE AND SPACE OF THE GRID
        gridSize = int.Parse(uiManager.texts.inputText.text);
        gridSpace = 1 / (float)(gridSize * .65f);
    }

    void CreateSquare()
    {
        //GET THE SCREEN HEIGHT AND WIDTH FOR GRID
        float screenHeight = (Camera.main.orthographicSize - 0.5f) * 2.0f;
        float screenWidth = screenHeight * Camera.main.aspect;

        //CALCULATE THE SQUARE SIZE
        float squareSize = (Mathf.Min(screenHeight, screenWidth) - gridSpace * (gridSize - 1)) / gridSize;

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                //SPAWN EVERY SQUARE
                GameObject square = Instantiate(squarePrefab, transform);
                square.name = "x_" + col + "," + "y_" + row;

                SquareController squareController = square.GetComponent<SquareController>();

                //SET POS ELEMENTS
                squareController.xPosition = col;
                squareController.yPosition = row;

                //SET OFFSET VALUES OF THE SQUARE
                float offsetX = (gridSize * squareSize + gridSpace * (gridSize - 1)) / 2 - squareSize / 2;
                float offsetY = (gridSize * squareSize + gridSpace * (gridSize - 1)) / 2 - squareSize / 2;

                //SET POSITION AND SCALE OF EVERY SQUARE
                square.transform.position = new Vector3(col * (squareSize + gridSpace) - offsetX, row * (squareSize + gridSpace) - offsetY + 1, 10f);
                square.transform.localScale = new Vector3(squareSize, squareSize, 1f);

                //ADD THE SQUARE TO LIST
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