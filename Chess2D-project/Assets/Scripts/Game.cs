//Completed unity project, this guide helped me to create the chess game   https://www.youtube.com/watch?v=lFZeeTZ29w0&list=PLXV-vjyZiT4b7WGjgiqMy422AVyMaigl1
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Special moves weren't implemented
public class Game : MonoBehaviour
{
    public GameObject chesspiece;

    private GameObject[,] positions = new GameObject[8, 8]; 
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    private string currentPlayer = "white";

    private bool gameOver = false;


    /// <summary>
    /// Starts the game by creating and positioning all chess pieces.
    /// </summary>
    void Start()
    {
        playerWhite = new GameObject[]
        {
           Create("white_rook",0,0),Create("white_knight",1,0),Create("white_bishop",2,0), Create("white_queen",3,0),
           Create("white_king",4,0),Create("white_bishop",5,0),Create("white_knight",6,0),Create("white_rook",7,0),
           Create("white_pawn",0,1),Create("white_pawn",1,1),Create("white_pawn",2,1),Create("white_pawn",3,1),
           Create("white_pawn",4,1),Create("white_pawn",5,1),Create("white_pawn",6,1),Create("white_pawn",7,1)
        };

        playerBlack = new GameObject[]
        {
           Create("black_rook",0,7),Create("black_knight",1,7),Create("black_bishop",2,7), Create("black_queen",3,7),
           Create("black_king",4,7),Create("black_bishop",5,7),Create("black_knight",6,7),Create("black_rook",7,7),
           Create("black_pawn",0,6),Create("black_pawn",1,6),Create("black_pawn",2,6),Create("black_pawn",3,6),
           Create("black_pawn",4,6),Create("black_pawn",5,6),Create("black_pawn",6,6),Create("black_pawn",7,6)
        };

        //set all piece positions on the board
        for(int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }

    /// <summary>
    /// Creates a new chess piece with the specified name and board coordinates.
    /// </summary>
    /// <param name="name">The name of the chess piece.</param>
    /// <param name="x">The initial x-coordinate of the chess piece on the board.</param>
    /// <param name="y">The initial y-coordinate of the chess piece on the board.</param>
    /// <returns>The created chess piece GameObject.</returns>
    public GameObject Create(string name,int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>();
        cm.name = name;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate();
        return obj;
    }

    /// <summary>
    /// Sets the position of a chess piece on the board.
    /// </summary>
    /// <param name="obj">The chess piece GameObject.</param>
    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();

        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    /// <summary>
    /// Sets the position of a chess piece on the board as empty.
    /// </summary>
    /// <param name="x">The x-coordinate of the position.</param>
    /// <param name="y">The y-coordinate of the position.</param>
    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    /// <summary>
    /// Retrieves the chess piece at the specified position on the board.
    /// </summary>
    /// <param name="x">The x-coordinate of the position.</param>
    /// <param name="y">The y-coordinate of the position.</param>
    /// <returns>The chess piece GameObject at the specified position.</returns>
    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    /// <summary>
    /// Checks if the given position is on the board.
    /// </summary>
    /// <param name="x">The x-coordinate of the position.</param>
    /// <param name="y">The y-coordinate of the position.</param>
    /// <returns>True if the position is on the board, False otherwise.</returns>
    public bool PositionOnBoard(int x, int y)
    {
        if(x<0 || y<0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    /// <summary>
    /// Retrieves the current player's color.
    /// </summary>
    /// <returns>The color of the current player.</returns>
    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    /// <summary>
    /// Checks if the game is over.
    /// </summary>
    /// <returns>True if the game is over, False otherwise.</returns>
    public bool IsGameOver()
    {
        return gameOver;
    }

    /// <summary>
    /// Advances the turn to the next player.
    /// </summary>
    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
        }
    }

    /// <summary>
    /// Updates the game state.
    /// </summary>
    public void Update()
    {
        if(gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            SceneManager.LoadScene("Game");
        }

    }

    /// <summary>
    /// Declares the winner of the game.
    /// </summary>
    /// <param name="playerWinner">The color of the winning player.</param>
    public void Winner(string playerWinner)
    {
        gameOver = true;

        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " is the winner";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }
}
