using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAction
{
    public static Slot PickRandomSlot()
    {
        System.Random random = new System.Random();
        int i = random.Next(0, GameManager.boardWidth);
        return GameManager.boardSlots3x3.Where(slot => (slot.SOccupier == null)).ToArray()[i];
    }

    public static Rigidbody2D PickComputerPiece(Rigidbody2D[] players)
    {
            System.Random random3 = new System.Random();
            int i = random3.Next(0, players.Length/2);
            return players.Where(player => (player.name.ToCharArray()[6].ToString() == "2")).ToArray()[i];
    }
}
