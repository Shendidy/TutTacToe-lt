﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop3x3 : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region Class Variables
    private RectTransform rectTransform;
    public Text gameStatus;
    [SerializeField] private Canvas mainCanvas;
    public Transform boardCanvas;
    private CanvasGroup canvasGroup;
    public CanvasGroup canvasGroup11;
    public CanvasGroup canvasGroup12;
    public CanvasGroup canvasGroup13;
    public CanvasGroup canvasGroup21;
    public CanvasGroup canvasGroup22;
    public CanvasGroup canvasGroup23;
    public Rigidbody2D nodeTopRight;
    public Rigidbody2D nodeBottomLeft;
    private Slot pieceStartSlot;
    private Rigidbody2D[] players;
    private List<Slot> playersLocation;
    private Rigidbody2D newComputerPiece;
    private Slot newComputerSlot;
    // Slots
    public Rigidbody2D slot11;
    public Rigidbody2D slot21;
    public Rigidbody2D slot31;
    public Rigidbody2D slot12;
    public Rigidbody2D slot22;
    public Rigidbody2D slot32;
    public Rigidbody2D slot13;
    public Rigidbody2D slot23;
    public Rigidbody2D slot33;
    // Players
    public Rigidbody2D player11;
    public Rigidbody2D player12;
    public Rigidbody2D player13;
    public Rigidbody2D player21;
    public Rigidbody2D player22;
    public Rigidbody2D player23;

    private bool isOutOfBoard;
    private float canvasDotAlpha;
    private int boardScore;
    #endregion
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        gameStatus.text = "";
        GameManager.newGame = true;
        GameManager.playerInTurn = 1;
        GameManager.gameOver = false;
        GameManager.difficulty = 1;
        GameManager.boardWidth = 3;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!GameManager.gameOver)
        {
            if (GameManager.newGame)
            {
                PopulateSlotCentres();
                AddSlotCentresOccupiers();
                GameManager.newGame = false;
            }

            foreach (Slot slot in GameManager.slotCentres)
                if (slot.SOccupier)
                    if (slot.SOccupier.name == rectTransform.name) pieceStartSlot = slot;

            canvasDotAlpha = canvasGroup.alpha;
            canvasGroup.alpha = Constants.MovingPlayerTransparency;

            PopulatePlayers();
            PopulatePlayersLocation();
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!GameManager.gameOver)
        {
            rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!GameManager.gameOver)
        {
            isOutOfBoard = CheckIfOutOfBoard();
            MovePiece(isOutOfBoard);
        }
    }
    private bool CheckIfOutOfBoard()
    {
        if (rectTransform.position.x > nodeTopRight.position.x ||
           rectTransform.position.x < nodeBottomLeft.position.x ||
           rectTransform.position.y > nodeTopRight.position.y ||
           rectTransform.position.y < nodeBottomLeft.position.y)
            return true;

        return false;
    }
    private Slot GetFinalSlot()
    {
        float distance = float.MaxValue;
        Slot finalSlot = new Slot("temp", new Vector2(0, 0));

        foreach (Slot slot in GameManager.slotCentres)
            if (Vector2.Distance(rectTransform.position, slot.SVector2) < distance)
            {
                distance = Vector2.Distance(rectTransform.position, slot.SVector2);
                finalSlot = slot;//don't attempt to break out of foreach after this step!
            }

        return finalSlot;
    }
    private void MovePiece(bool isOutOfBoard)
    {
        Slot finalSlot = GetFinalSlot();

        if (isOutOfBoard || !SlotIsFree(finalSlot) || !ValidPlayer())
        {
            rectTransform.position = pieceStartSlot.SVector2;
            canvasGroup.alpha = canvasDotAlpha;
        }
        else
        {
            Rigidbody2D currentPlayer = GetCurrentPlayer();
            rectTransform.position = finalSlot.SVector2;

            foreach (Slot slot in GameManager.slotCentres)
            {
                if (slot.SOccupier?.name == rectTransform.name)
                {
                    slot.SOccupier = null;
                    break;
                }
            }

            foreach (Slot slot in GameManager.slotCentres)
            {
                if (Vector2.Distance(slot.SVector2, rectTransform.position) < 0.25)
                {
                    slot.SOccupier = currentPlayer;
                    break;
                }
            }

            canvasGroup.alpha = Constants.StandingPlayerTransparency;

            if (IsWinner())
            {
                GameManager.gameOver = true;
                gameStatus.color = Color.blue;
                gameStatus.text = "YOU WIN!";
                //boardCanvas.GetComponent<Canvas>().enabled = false;
            }
            else
            {
                GameManager.playerInTurn = 2;
                ComputerMove();
            }
        }
    }
    private Rigidbody2D GetCurrentPlayer()
    {
        Rigidbody2D currentPlayer = new Rigidbody2D();
        foreach (Rigidbody2D player in players)
            if (player.name == rectTransform.name) currentPlayer = player;
        return currentPlayer;
    }
    private bool SlotIsFree(Slot finalSlot)
    {
        foreach (Slot slot in GameManager.slotCentres)
            if (slot == finalSlot && slot.SOccupier) return false;
        return true;
    }
    private void PopulateSlotCentres()
    {
        GameManager.slotCentres = new Slot[9];

        GameManager.slotCentres[0] = new Slot("slot11", slot11.position);
        GameManager.slotCentres[1] = new Slot("slot21", slot21.position);
        GameManager.slotCentres[2] = new Slot("slot31", slot31.position);
        GameManager.slotCentres[3] = new Slot("slot12", slot12.position);
        GameManager.slotCentres[4] = new Slot("slot22", slot22.position);
        GameManager.slotCentres[5] = new Slot("slot32", slot32.position);
        GameManager.slotCentres[6] = new Slot("slot13", slot13.position);
        GameManager.slotCentres[7] = new Slot("slot23", slot23.position);
        GameManager.slotCentres[8] = new Slot("slot33", slot33.position);
    }
    private void AddSlotCentresOccupiers()
    {
        if (GameManager.newGame)
        {
            GameManager.slotCentres[0].SOccupier = player11;
            GameManager.slotCentres[1].SOccupier = player12;
            GameManager.slotCentres[2].SOccupier = player13;
            GameManager.slotCentres[6].SOccupier = player21;
            GameManager.slotCentres[7].SOccupier = player22;
            GameManager.slotCentres[8].SOccupier = player23;
        }
    }
    private void PopulatePlayers()
    {
        players = new Rigidbody2D[6];

        players[0] = player11;
        players[1] = player12;
        players[2] = player13;
        players[3] = player21;
        players[4] = player22;
        players[5] = player23;
    }
    private void PopulatePlayersLocation()
    {
        playersLocation = new List<Slot>();
        foreach (Slot slot in GameManager.slotCentres)
        {
            if (slot.SOccupier) playersLocation.Add(slot);
        }
    }
    private bool ValidPlayer() =>
        rectTransform.name.ToCharArray()[6].ToString() == GameManager.playerInTurn.ToString();
    public void ComputerMove()
    {
        Calculations.CalculateBoardScore();

        PickComputerSlot();
        PickComputerPiece();
        Slot oldSlot = new Slot("temp", new Vector2(0, 0));

        // Get old slot
        foreach (Slot slot in GameManager.slotCentres)
        {
            if (slot.SOccupier == newComputerPiece)
            {
                oldSlot = slot;
                break;
            }
        }

        //Thread.Sleep(250);
        newComputerPiece.position = newComputerSlot.SVector2;

        // Empty SOccupier of player 2 old slot
        oldSlot.SOccupier = null;

        // Add SOccupier to newSlot
        foreach (Slot slot in GameManager.slotCentres)
        {
            if (Vector2.Distance(slot.SVector2, newComputerPiece.position) < 0.25)
            {
                slot.SOccupier = newComputerPiece;
                break;
            }
        }

        // Make alpha of newPlayerPiece = 1
        switch (newComputerPiece.name)
        {
            case "Player2-1":
                canvasGroup21.alpha = 1;
                break;
            case "Player2-2":
                canvasGroup22.alpha = 1;
                break;
            default:
                canvasGroup23.alpha = 1;
                break;
        }
        if (IsWinner())
        {
            GameManager.gameOver = true;
            gameStatus.color = Color.red;
            gameStatus.text = "YOU LOSE!";
        }
        GameManager.playerInTurn = 1;
    }

    private void PickComputerSlot()
    {
        // In case of easy game an computer only picks a random slot and piece
        if (GameManager.difficulty == 1)
        {
            PickRandomSlot();
        }
        //else if (GameManager.difficulty == 2)
        //{
        //    #region Attacking
        //    //      Build an array of slot names that are occupied by computer moved pieces
        //    string[] computerActiveSlots = BuildComputersActiveSlots();
        //    //      if computer have moved 2 pieces (or more in bigger boards)
        //    if (computerActiveSlots.Length > 1)
        //    {
        //        //  Create a list with arrays of available winning options
        //        List<string[]> computersWinningOptions = new List<string[]>();
        //        foreach (string[] winArray in GameManager.winningSlotsArray3x3)
        //        {
        //            if (winArray.ContainsOnly(2, computerActiveSlots)) computersWinningOptions.Add(winArray);
        //        }

        //        if (computersWinningOptions.Count > 0)
        //        {
        //            // Remove options where third slot is occupied by player 1
        //            foreach (string[] winningCase in computersWinningOptions)
        //            {
        //                bool loop1 = true;
        //                while (loop1)
        //                {
        //                    foreach (string slotName in winningCase)
        //                    {
        //                        foreach (Slot slot in GameManager.slotCentres)
        //                        {
        //                            if (slot.SName == slotName && slot.SOccupier.name.ToCharArray()[6].ToString() == "1")
        //                            {
        //                                computersWinningOptions.Remove(winningCase);
        //                                loop1 = false;
        //                            }
        //                            else if (slot.SName == slotName && slot.SOccupier == null)
        //                            {
        //                                newComputerSlot = slot;
        //                                // pick remaining computer piece
        //                                newComputerPiece = GetRemainingComputerPiece(winningCase);
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            if (computersWinningOptions.Count == 0)
        //                PickRandomSlot();

        //            else
        //            {
        //                // Pick third slot if empty

        //                foreach (string[] slotsArray in computersWinningOptions)
        //                {
        //                    List<Rigidbody2D> usedPieces = new List<Rigidbody2D>();
        //                    Slot winningSlot;
        //                    Slot[] usedComputerSlots = GameManager.slotCentres.Where(slot => (slot.SOccupier.name.ToCharArray()[6] == 2)).ToArray();
        //                    foreach (Slot slot in usedComputerSlots)
        //                    {
        //                        if (slot.SName.ToCharArray()[6].ToString() == "1") usedPieces.Add(slot.SOccupier);
        //                        if (slot.SOccupier == null) winningSlot = slot;
        //                    }
        //                    //if(winningSlot)
        //                }
        //            }
        //            //  Get name of computer pice needed to win
        //        }
        //    }
        //    else {
        //        PickRandomSlot();
        //        PickComputerPiece();
        //    }
        //    #endregion

        //    #region Defending
        //    // **** Using AI logic ****
        //    //  1- Check if oponent has any 2 out of 3 winning cases
        //    //      build array of slots occupied with moved pieces
        //    //string[] opponentsActiveSlots = BuildOponentsActiveSlots();
        //    //List<string[]> opponentsWinningOptions = new List<string[]>();
        //    //foreach (string[] winArray in GameManager.winningSlotsArray3x3)
        //    //{
        //    //    if (winArray.ContainsOnly(2, opponentsActiveSlots)) opponentsWinningOptions.Add(winArray);
        //    //}
        //    //bool has2 = GameManager.slotCentres.Select(slot => slot.SOccupier.name.ToArray())
        //    //  2- Check if the third place remaining for winning is empty
        //    //  3- If empty, pick a piece to fill and check if this opens another winning opportunity
        //    //  4- If it opens another winning opportunity then pick another piece and check again
        //    //  5- If third place remaining for winning is not empty then look if oponent has another oppotunity opened
        //    //  6- If so then again check to close it
        //    //  7- If oponent doesn't have any opportunity opened then check if computer has opportunity
        //    //  8- If yes, fill it and win the game
        //    //  9- If not, check if any unmoved piece remains unmoved
        //    // 10- If yes, check if moving it opens an oppurtunity to the oponent and move it or pick another one and check again.
        //    // **** End of AI logic ****
        //    #endregion
        //}
    }

    //private Rigidbody2D GetRemainingComputerPiece(string[] winningCase)
    //{
    //    Rigidbody2D computerPiece = new Rigidbody2D();
    //    Rigidbody2D[] computerPieces = new Rigidbody2D[]
    //    {
    //        player21,
    //        player22,
    //        player23
    //    };


    //}

    private void PickRandomSlot()
    {
        System.Random random = new System.Random();

        while (true)
        {
            int i = random.Next(0, 9);
            if (GameManager.slotCentres[i].SOccupier == null)
            {
                newComputerSlot = GameManager.slotCentres[i];
                break;
            }
        }
    }

    private string[] BuildOponentsActiveSlots()
    {
        Slot[] tempSlotsArray = GameManager.slotCentres.
            Where(slot => slot.SOccupier?.name.ToCharArray()[6].ToString() == "1").
            Where(slot => slot.SOccupier?.name == "Player1-1" ? PlayerPieceMoved(canvasGroup11) :
                slot.SOccupier?.name == "Player1-2" ? PlayerPieceMoved(canvasGroup12) : PlayerPieceMoved(canvasGroup13)).
            ToArray();

        String[] playersSlotsArray = tempSlotsArray.Select(x => x.SName).ToArray();

        return playersSlotsArray;
    }
    private string[] BuildComputersActiveSlots()
    {
        Slot[] tempSlotsArray = GameManager.slotCentres.
            Where(slot => slot.SOccupier?.name.ToCharArray()[6].ToString() == "2").
            Where(slot => slot.SOccupier?.name == "Player2-1" ? PlayerPieceMoved(canvasGroup21) :
                slot.SOccupier?.name == "Player2-2" ? PlayerPieceMoved(canvasGroup22) : PlayerPieceMoved(canvasGroup23)).
            ToArray();

        String[] computersSlotsArray = tempSlotsArray.Select(x => x.SName).ToArray();

        return computersSlotsArray;
    }

    private void PickComputerPiece()// In case of easy game only, otherwise it will be picked in the pick slot method
    {
        if (GameManager.difficulty == 1)
        {
            System.Random random3 = new System.Random();

            while (true)
            {
                int i = random3.Next(0, players.Length);
                if (players[i].name.ToCharArray()[6].ToString() == "2")
                {
                    newComputerPiece = players[i];
                    break;
                }
            }
        }
    }
    private bool IsWinner()
    {
        // I want to check if all pieces have moved
        if(AllPlayerPiecesMoved())
        {
            Slot[] tempSlotsArray = GameManager.slotCentres.Where(slot
                => slot.SOccupier?.name.ToCharArray()[6].ToString()
                == GameManager.playerInTurn.ToString()).ToArray();

            String[] playersSlotsArray = tempSlotsArray.Select(x => x.SName).ToArray();

            foreach(String[] winningSlots in GameManager.winningSlotsArray3x3)
            {
                bool[] winning = new bool[winningSlots.Length];
                for(int i = 0; i < winningSlots.Length; i++)
                {
                    if (playersSlotsArray.Contains(winningSlots[i])) winning[i] = true;
                }

                if (!winning.Contains(false)) return true;
            }
        }

        return false;
    }
    private bool AllPlayerPiecesMoved()
    {
        if (GameManager.playerInTurn == 1
            && PlayerPieceMoved(canvasGroup11)
            && PlayerPieceMoved(canvasGroup12)
            && PlayerPieceMoved(canvasGroup13))
            return true;

        if (GameManager.playerInTurn == 2
            && PlayerPieceMoved(canvasGroup21)
            && PlayerPieceMoved(canvasGroup22)
            && PlayerPieceMoved(canvasGroup23))
            return true;

        return false;
    }
    private bool PlayerPieceMoved(CanvasGroup canvasGroup)
    {
        return canvasGroup.alpha == 1 ? true : false;
    }
}