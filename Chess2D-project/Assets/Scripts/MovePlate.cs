
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    int matrixX;
    int matrixY;

    public bool attack = false;

    /// <summary>
    /// Sets the color of the move plate to red if it represents an attack move.
    /// </summary>
    public void Start()
    {
        if (attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    /// <summary>
    /// Handles the event when the move plate is clicked.
    /// </summary>
    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");
            if (cp.name == "black_king") controller.GetComponent<Game>().Winner("white");

            Destroy(cp);
        }


        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessman>().GetXBoard(),
           reference.GetComponent<Chessman>().GetYBoard());

        reference.GetComponent<Chessman>().SetXBoard(matrixX);
        reference.GetComponent<Chessman>().SetYBoard(matrixY);
        reference.GetComponent<Chessman>().SetCoords();

        controller.GetComponent<Game>().SetPosition(reference);

        controller.GetComponent<Game>().NextTurn();

        reference.GetComponent<Chessman>().firstMove = false;

        reference.GetComponent<Chessman>().DestroyMovePlates();


    }

    /// <summary>
    /// Sets the coordinates of the move plate in the chessboard matrix.
    /// </summary>
    /// <param name="x">The x-coordinate in the matrix.</param>
    /// <param name="y">The y-coordinate in the matrix.</param>
    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    /// <summary>
    /// Sets the reference to the chessman associated with the move plate.
    /// </summary>
    /// <param name="obj">The reference to the chessman GameObject.</param>
    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    /// <summary>
    /// Retrieves the reference to the chessman associated with the move plate.
    /// </summary>
    /// <returns>The reference to the chessman GameObject.</returns>
    public GameObject GetReference()
    {
        return reference;
    }

}
