using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool newGame { get; set; }
    public static Slot[] slotCentres { get; set; }
    public static int playerInTurn { get; set; }
}
