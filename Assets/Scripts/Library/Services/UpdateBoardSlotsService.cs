using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpdateBoardSlotsService : MonoBehaviour
{
    public static Slot[] UpdateBoardSlots(Slot[] board, RectTransform playerPiece, Slot newSlot)
    {
        return board;
    }

    public static Slot[] UpdateBoardSlots(Slot[] board, Rigidbody2D playerPiece, Slot newSlot, bool updateMoved)
    {
        // Get old slot
        Slot oldSlot = board.Where(slot => (slot.SOccupier == playerPiece)).ToArray()[0];

        playerPiece.position = newSlot.SVector2;
        if (updateMoved) SetPlayersMovedService.SetPlayersMoved3x3(playerPiece);

        // Empty SOccupier of player 2 old slot
        oldSlot.SOccupier = null;

        // Add SOccupier to newSlot
        foreach (Slot slot in board)
        {
            if (Vector2.Distance(slot.SVector2, playerPiece.position) < 0.25)
            {
                slot.SOccupier = playerPiece;
                break;
            }
        }

        return board;
    }
}
