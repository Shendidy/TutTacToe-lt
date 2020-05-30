using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool newGame { get; set; }
    public static Slot[] boardSlots3x3 { get; set; }
    public static int playerInTurn { get; set; }
    public static bool gameOver { get; set; }
    public static int difficulty { get; set; }
    public static int boardWidth { get; set; }
    public static bool[] playersMoved3x3 { get; set; }
    public static List<string[]> winningSlotsArray3x3 { get; private set; }
    public static bool isMicOn { get; set; }
    public static int keysTotal { get; set; }

    GameManager()
    {
        winningSlotsArray3x3 = new List<string[]>
        {
            new string[] { "slot11", "slot21", "slot31" },
            new string[] { "slot12", "slot22", "slot32" },
            new string[] { "slot13", "slot23", "slot33" },
            new string[] { "slot11", "slot12", "slot13" },
            new string[] { "slot21", "slot22", "slot23" },
            new string[] { "slot31", "slot32", "slot33" },
            new string[] { "slot11", "slot22", "slot33" },
            new string[] { "slot31", "slot22", "slot13" }
        };
    }
}
