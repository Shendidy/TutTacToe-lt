using System;
using UnityEngine;
using System.Linq;

public class ChecksService
{
    public static bool CheckIfMoved(Rigidbody2D playerPiece)
    {
        switch (playerPiece.name)
        {
            case "Player1-1":
                return GameManager.playersMoved3x3[0];
            case "Player1-2":
                return GameManager.playersMoved3x3[1];
            case "Player1-3":
                return GameManager.playersMoved3x3[2];
            case "Player2-1":
                return GameManager.playersMoved3x3[3];
            case "Player2-2":
                return GameManager.playersMoved3x3[4];
            case "Player2-3":
                return GameManager.playersMoved3x3[5];
            default:
                break;
        }
        return true;
    }
    public static bool SlotIsFree(Slot finalSlot)
    {
        foreach (Slot slot in GameManager.boardSlots3x3)
            if (slot == finalSlot && slot.SOccupier) return false;
        return true;
    }
    public static bool IsWinner(Slot[] slotsBoard)
    {
        // I want to check if all pieces have moved
        if (DidAllPlayerPiecesMove())
        {
            Slot[] tempSlotsArray = slotsBoard.Where(slot
                => slot.SOccupier?.name.ToCharArray()[6].ToString()
                == GameManager.playerInTurn.ToString()).ToArray();

            String[] playersSlotsArray = tempSlotsArray.Select(x => x.SName).ToArray();

            foreach (String[] winningSlots in GameManager.winningSlotsArray3x3)
            {
                bool[] winning = new bool[winningSlots.Length];
                for (int i = 0; i < winningSlots.Length; i++)
                {
                    if (playersSlotsArray.Contains(winningSlots[i])) winning[i] = true;
                }

                if (!winning.Contains(false))
                {
                    GameManager.interstitialAdCounter++;
                    if (GameManager.interstitialAdCounter % 3 == 0)
                        AdMob.instance.ShowInterstitialAd();

                    return true;
                }
            }
        }

        return false;
    }
    private static bool DidAllPlayerPiecesMove()
    {
        if (GameManager.playerInTurn == 1
            && GameManager.playersMoved3x3[0]
            && GameManager.playersMoved3x3[1]
            && GameManager.playersMoved3x3[2])
            return true;

        if (GameManager.playerInTurn == 2
            && GameManager.playersMoved3x3[3]
            && GameManager.playersMoved3x3[4]
            && GameManager.playersMoved3x3[5])
            return true;

        return false;
    }
    public static bool CheckIfOutOfBoard(Vector2 playingPiece, Vector2 nodeTopRight, Vector2 nodeBottomLeft)
    {
        if (playingPiece.x > nodeTopRight.x ||
           playingPiece.x < nodeBottomLeft.x ||
           playingPiece.y > nodeTopRight.y ||
           playingPiece.y < nodeBottomLeft.y)
            return true;

        return false;
    }
}
