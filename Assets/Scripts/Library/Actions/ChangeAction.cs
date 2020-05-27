using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAction
{
    public static void ChangePlayerInTurn()
    {
        GameManager.playerInTurn =
            GameManager.playerInTurn == 1 ? 2 : 1;
    }
}
