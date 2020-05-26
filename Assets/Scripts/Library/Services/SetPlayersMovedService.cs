using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayersMovedService
{
    public static void SetPlayersMoved3x3(string player, string piece)
    {
        int playerNumber = Int16.Parse(player) == 1 ? 0 : 3;
        int pieceNumber = Int16.Parse(piece);

        int playerPieceIndex = (playerNumber) + (pieceNumber - 1);

        GameManager.playersMoved3x3[playerPieceIndex] = true;
    }
}