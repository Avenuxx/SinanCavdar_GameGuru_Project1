using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareController : MonoBehaviour
{
    GameManager manager;

    public int xPosition;
    public int yPosition;
    public List<SquareController> neighbors;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("CheckForNeighbors", 0.1f);
    }

    void CheckForNeighbors()
    {
        List<GameObject> currentSquaresList = manager.lists.currentSquares;
        int currentSquares = manager.lists.currentSquares.Count;
        for (int i = 0; i < currentSquares; i++)
        {
            SquareController squareController = currentSquaresList[i].GetComponent<SquareController>();
            if (squareController.xPosition == this.xPosition - 1 && squareController.yPosition == this.yPosition && !neighbors.Contains(squareController))
                neighbors.Add(squareController);
            if (squareController.yPosition == this.yPosition + 1 && squareController.xPosition == this.xPosition && !neighbors.Contains(squareController))
                neighbors.Add(squareController);
            if (squareController.xPosition == this.xPosition + 1 && squareController.yPosition == this.yPosition && !neighbors.Contains(squareController))
                neighbors.Add(squareController);
            if (squareController.yPosition == this.yPosition - 1 && squareController.xPosition == this.xPosition && !neighbors.Contains(squareController))
                neighbors.Add(squareController);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("cross png");
    }
}
