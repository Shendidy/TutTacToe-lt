using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateService
{
    public static Rigidbody2D[] PopulatePlayers(Rigidbody2D[] playersInGame)
    {
        Rigidbody2D[] players = new Rigidbody2D[playersInGame.Length];

        for(int i = 0; i < playersInGame.Length; i++)
            players[i] = playersInGame[i];

        return players;
    }

    public static List<Slot> PopulatePlayersLocation()
        => GameManager.boardSlots3x3.Where(slot => (slot.SOccupier != null)).ToArray().ToList();
}
