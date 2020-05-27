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

        while (true)
        {
            int i = random.Next(0, (int)Math.Pow(GameManager.boardWidth, 2));
            if (GameManager.boardSlots3x3[i].SOccupier == null)
            {
                Slot newComputerSlot = GameManager.boardSlots3x3[i];
                return newComputerSlot;
            }
        }
    }

    public static Rigidbody2D PickComputerPiece(Rigidbody2D[] players)// In case of easy game only, otherwise it will be picked in the pick slot method
    {
            System.Random random3 = new System.Random();
            int i = random3.Next(0, players.Length/2);
            return players.Where(player => (player.name.ToCharArray()[6].ToString() == "2")).ToArray()[i];
    }
}
