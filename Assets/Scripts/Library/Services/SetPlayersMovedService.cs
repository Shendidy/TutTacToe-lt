using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayersMovedService
{
    public static void FirstSetupPlayersMoved()
    {
        GameManager.playersMoved3x3 = new bool[GameManager.boardWidth * 2];
        for (int i = 0; i < GameManager.boardWidth * 2; i++)
            GameManager.playersMoved3x3[i] = false;
    }

    public static void SetPlayersMoved3x3(Rigidbody2D playerPiece)
    {
        int playerNumber = Int16.Parse(playerPiece.name.ToCharArray()[6].ToString()) == 1 ? 0 : 3;
        int pieceNumber = Int16.Parse(playerPiece.name.ToCharArray()[8].ToString());

        int playerPieceIndex = (playerNumber) + (pieceNumber - 1);

        GameManager.playersMoved3x3[playerPieceIndex] = true;
    }

    public static void SetPlayersMoved3x3(RectTransform playerPiece)
    {
        int playerNumber = Int16.Parse(playerPiece.name.ToCharArray()[6].ToString()) == 1 ? 0 : 3;
        int pieceNumber = Int16.Parse(playerPiece.name.ToCharArray()[8].ToString());

        int playerPieceIndex = (playerNumber) + (pieceNumber - 1);

        GameManager.playersMoved3x3[playerPieceIndex] = true;
    }

    public static void SetPlayersNotMoved3x3(Rigidbody2D playerPiece) // this method will only be used when calculating AI's move
    {
        int playerNumber = Int16.Parse(playerPiece.name.ToCharArray()[6].ToString()) == 1 ? 0 : 3;
        int pieceNumber = Int16.Parse(playerPiece.name.ToCharArray()[8].ToString());

        int playerPieceIndex = (playerNumber) + (pieceNumber - 1);

        GameManager.playersMoved3x3[playerPieceIndex] = false;
    }
}