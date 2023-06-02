using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject chesspiece;

    void Start()
    {
        Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
    }

}
