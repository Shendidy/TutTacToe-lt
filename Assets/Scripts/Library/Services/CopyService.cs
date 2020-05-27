using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyService
{
    public static Slot[] CopySlotsArray(Slot[] board)
    {
        Slot[] newBoard = new Slot[board.Length];

        for (int i = 0; i < newBoard.Length; i++)
        {
            newBoard[i] = new Slot("temp", new Vector2(0, 0));
            newBoard[i].SName = board[i].SName;
            newBoard[i].SOccupier = board[i].SOccupier;
            newBoard[i].SVector2 = board[i].SVector2;
        }

        return newBoard;
    }
}
