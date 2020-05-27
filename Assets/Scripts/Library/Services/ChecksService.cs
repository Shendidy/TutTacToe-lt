using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChecksService
{
    public static bool CheckIfMoved(Rigidbody2D playerPiece)
    {
        switch (playerPiece.name)
        {
            case "Player1-1":
                return GameManager.playersMoved3x3[0];
            case "Player1-2":
                return GameManager.playersMoved3x3[1];
            case "Player1-3":
                return GameManager.playersMoved3x3[2];
            case "Player2-1":
                return GameManager.playersMoved3x3[3];
            case "Player2-2":
                return GameManager.playersMoved3x3[4];
            case "Player2-3":
                return GameManager.playersMoved3x3[5];
            default:
                break;
        }
        return true;
    }
}
