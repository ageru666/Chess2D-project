using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @class Chessman
 * @brief Represents a chess piece in the game.
 */
public class Chessman : MonoBehaviour
{
    //references
    public GameObject controller;
    public GameObject movePlate;

    // positions
    private int xBoard = -1;
    private int yBoard = -1;

    public bool firstMove = true;


    private string player;

    //references for all sprites
    public Sprite black_queen, black_knight, black_bishop, black_king, black_rook, black_pawn;
    public Sprite white_queen, white_knight, white_bishop, white_king, white_rook, white_pawn;

    /// <summary>
    /// Activates the chess piece and sets its initial position and sprite based on its name.
    /// </summary>
    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        //take the instantiated location and adjust the transform
        SetCoords();

        switch (this.name)
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn;player = "black"; break;

            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
        }
    }

    /// <summary>
    /// Sets the visual coordinates of the chess piece based on its board coordinates.
    /// </summary>
    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    /// <summary>
    /// Retrieves the current x-coordinate of the chess piece on the board.
    /// </summary>
    /// <returns>The x-coordinate of the chess piece on the board.</returns>
    public int GetXBoard()
    {
        return xBoard;
    }

    /// <summary>
    /// Retrieves the current y-coordinate of the chess piece on the board.
    /// </summary>
    /// <returns>The y-coordinate of the chess piece on the board.</returns>
    public int GetYBoard()
    {
        return yBoard;
    }

    /// <summary>
    /// Sets the x-coordinate of the chess piece on the board.
    /// </summary>
    /// <param name="x">The new x-coordinate of the chess piece.</param>
    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    /// <summary>
    /// Sets the y-coordinate of the chess piece on the board.
    /// </summary>
    /// <param name="y">The new y-coordinate of the chess piece.</param>
    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    /// <summary>
    /// Triggered when the mouse button is released over the chess piece.
    /// Destroys existing move plates and initiates the creation of new move plates.
    /// </summary>
    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();

            InitiateMovePlates();
        }
    }

    /// <summary>
    /// Destroys all existing move plates in the scene.
    /// </summary>
    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for(int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    /// <summary>
    /// Initiates the creation of move plates based on the type of chess piece.
    /// </summary>
    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;

            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;

            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;

            case "black_king":
            case "white_king":
                SurroundMovePlate();
                break;

            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;

            case "black_pawn":
                PawnMovePlate(xBoard, yBoard - 1);
                break;

            case "white_pawn":
                PawnMovePlate(xBoard, yBoard + 1);
                break;

        }
    }

    /// <summary>
    /// Creates a line of move plates along a specified direction until a blocking piece is encountered.
    /// </summary>
    /// <param name="xIncrement">The increment value for the x-coordinate.</param>
    /// <param name="yIncrement">The increment value for the y-coordinate.</param>
    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if(sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
        {
            MovePlateAttackSpawn(x, y);
        }
    }

    /// <summary>
    /// Creates L-shaped move plates around the chess piece.
    /// </summary>
    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);

        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    /// <summary>
    /// Creates move plates in the surrounding positions of the chess piece.
    /// </summary>
    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 0);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard + 0);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);
    }

    /// <summary>
    /// Creates a move plate at the specified position.
    /// </summary>
    /// <param name="x">The x-coordinate of the move plate.</param>
    /// <param name="y">The y-coordinate of the move plate.</param>
    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    /// <summary>
    /// Creates move plates for the pawn at the specified positions.
    /// </summary>
    /// <param name="x">The x-coordinate of the pawn.</param>
    /// <param name="y">The y-coordinate of the pawn.</param>
    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();

        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y);

                if (firstMove)
                {
                    int doubleMoveY = (player == "black") ? y - 1 : y + 1;
                    if (sc.GetPosition(x, doubleMoveY) == null)
                    {
                        MovePlateSpawn(x, doubleMoveY);
                    }
                }
            }

            if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null && sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x + 1, y);
            }


            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);
            }
        }
    }


    /// <summary>
    /// Spawns a move plate at the specified position.
    /// </summary>
    /// <param name="matrixX">The x-coordinate of the move plate on the matrix.</param>
    /// <param name="matrixY">The y-coordinate of the move plate on the matrix.</param>
    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    /// <summary>
    /// Spawns an attack move plate at the specified position.
    /// </summary>
    /// <param name="matrixX">The x-coordinate of the move plate on the matrix.</param>
    /// <param name="matrixY">The y-coordinate of the move plate on the matrix.</param>
    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}
