﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculations
{
    public static int CalculateBoardScore()
    {
        Debug.Log("Calculating board score!");

        int boardScore = CalculateScoreFromMovedPieces();
        boardScore += CalculateScoreFromCentreSlot();
        boardScore += CalculateScoreFromCorners();
        boardScore += CalculateScoreFrom2InLine();

        return boardScore;
    }
    private static int CalculateScoreFromMovedPieces()
    {
        int aiMoved = 0;
        for (int i = 0; i < 3; i++)
            if (GameManager.playersMoved3x3[i]) aiMoved -= 1;
        for (int i = 3; i < 6; i++)
            if (GameManager.playersMoved3x3[i]) aiMoved += 1;

        return aiMoved;
    }
    private static int CalculateScoreFromCentreSlot()
    {
        int centreScore = GameManager.slotCentres3x3[4].SOccupier == null ? 0
            : GameManager.slotCentres3x3[4].SOccupier.name.ToCharArray()[6].ToString() == "2"
            ? 8
            : -8;

        return centreScore;
    }
    private static int CalculateScoreFromCorners()
    {
        int cornersScore = 0;
        foreach (Slot slot in GameManager.slotCentres3x3)
            if (slot.SName == "slot11"
                || slot.SName == "slot31"
                || slot.SName == "slot13"
                || slot.SName == "slot33")
            {
                if (slot.SOccupier?.name == "Player1-1" && GameManager.playersMoved3x3[0])
                    cornersScore -= 6;
                if (slot.SOccupier?.name == "Player1-2" && GameManager.playersMoved3x3[1])
                    cornersScore -= 6;
                if (slot.SOccupier?.name == "Player1-3" && GameManager.playersMoved3x3[2])
                    cornersScore -= 6;
                if (slot.SOccupier?.name == "Player2-1" && GameManager.playersMoved3x3[3])
                    cornersScore += 6;
                if (slot.SOccupier?.name == "Player2-2" && GameManager.playersMoved3x3[4])
                    cornersScore += 6;
                if (slot.SOccupier?.name == "Player2-3" && GameManager.playersMoved3x3[5])
                    cornersScore += 6;
            }

        return cornersScore;
    }
    private static int CalculateScoreFrom2InLine()
    {
        int score = 0;

        foreach(string[] winningOption in GameManager.winningSlotsArray3x3)
        {
            int player = 0;
            int ai = 0;
            bool thirdIsAI = false;
            bool thirdIsPlayer = false;
            //foreach (string winningSlot in winningOption)
            //{
            //    foreach (Slot slot in GameManager.slotCentres3x3)
            //    {
            //        if (slot.SName == winningSlot)
            //        {
            //            if (slot.SOccupier != null)
            //            {
            //                if (slot.SOccupier.name == "Player1-1")
            //                    if (GameManager.playersMoved3x3[0]) player += 1;
            //                    else if (!GameManager.playersMoved3x3[0]) thirdIsPlayer = true;

            //                if (slot.SOccupier.name == "Player1-2")
            //                    if (GameManager.playersMoved3x3[1]) player += 1;
            //                    else if (!GameManager.playersMoved3x3[1]) thirdIsPlayer = true;

            //                if (slot.SOccupier.name == "Player1-3")
            //                    if (GameManager.playersMoved3x3[2]) player += 1;
            //                    else if (!GameManager.playersMoved3x3[2]) thirdIsPlayer = true;

            //                if (slot.SOccupier.name == "Player2-1")
            //                    if (GameManager.playersMoved3x3[3]) ai += 1;
            //                    else if (!GameManager.playersMoved3x3[3]) thirdIsAI = true;

            //                if (slot.SOccupier.name == "Player2-2")
            //                    if (GameManager.playersMoved3x3[4]) ai += 1;
            //                    else if (!GameManager.playersMoved3x3[4]) thirdIsAI = true;

            //                if (slot.SOccupier.name == "Player2-3")
            //                    if (GameManager.playersMoved3x3[5]) ai += 1;
            //                    else if (!GameManager.playersMoved3x3[5]) thirdIsAI = true;
            //            }
            //        }
            //    }
            //}
            foreach (string winningSlot in winningOption)
            {
                Slot slot = GameManager.slotCentres3x3.Where(x => x.SName == winningSlot).ToArray()[0];
                if (slot.SOccupier?.name == "Player1-1")
                    if (GameManager.playersMoved3x3[0]) player += 1;

                if (slot.SOccupier?.name == "Player1-2")
                    if (GameManager.playersMoved3x3[1]) player += 1;

                if (slot.SOccupier?.name == "Player1-3")
                    if (GameManager.playersMoved3x3[2]) player += 1;

                if (slot.SOccupier?.name == "Player2-1")
                    if (GameManager.playersMoved3x3[3]) ai += 1;

                if (slot.SOccupier?.name == "Player2-2")
                    if (GameManager.playersMoved3x3[4]) ai += 1;

                if (slot.SOccupier?.name == "Player2-3")
                    if (GameManager.playersMoved3x3[5]) ai += 1;
            }
            if (ai == 2)
            {
                foreach(string winningSlot in winningOption)
                {
                    Slot slot = GameManager.slotCentres3x3.Where(x => x.SName == winningSlot).ToArray()[0];
                    string occupierName = slot.SOccupier?.name;
                    if ((occupierName == "Player2-1" && !GameManager.playersMoved3x3[3])
                        || (occupierName == "Player2-2" && !GameManager.playersMoved3x3[4])
                        || (occupierName == "Player2-3" && !GameManager.playersMoved3x3[5]))
                        thirdIsAI = true;
                    else if ((occupierName == "Player1-1")
                        || (occupierName == "Player1-2")
                        || (occupierName == "Player1-3"))
                        thirdIsPlayer = true;
                }
                if (thirdIsAI) score += 15;
                else if (thirdIsPlayer) score += 30;
                else score += 100;
            }
            else if (player == 2)
            {
                foreach (string winningSlot in winningOption)
                {
                    Slot slot = GameManager.slotCentres3x3.Where(x => x.SName == winningSlot).ToArray()[0];
                    string occupierName = slot.SOccupier?.name;
                    if ((occupierName == "Player1-1" && !GameManager.playersMoved3x3[0])
                        || (occupierName == "Player1-2" && !GameManager.playersMoved3x3[1])
                        || (occupierName == "Player1-3" && !GameManager.playersMoved3x3[2]))
                        thirdIsPlayer = true;
                    else if ((occupierName == "Player2-1")
                        || (occupierName == "Player2-2")
                        || (occupierName == "Player2-3"))
                        thirdIsAI = true;
                }
                if (thirdIsPlayer) score -= 15;
                else if (thirdIsAI) score -= 30;
                else score -= 100;
            }
        }
        return score;
    }
}
