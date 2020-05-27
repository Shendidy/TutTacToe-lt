using System;
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
    #endregion
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        gameStatus.text = "";
        GameManager.newGame = true;
        GameManager.playerInTurn = 1;
        GameManager.gameOver = false;
        GameManager.difficulty = 2;
        GameManager.boardWidth = 3;

        SetPlayersMovedService.FirstSetupPlayersMoved();
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

            foreach (Slot slot in GameManager.boardSlots3x3)
                if (slot.SOccupier)
                    if (slot.SOccupier.name == rectTransform.name) pieceStartSlot = slot;

            canvasDotAlpha = canvasGroup.alpha;
            canvasGroup.alpha = Constants.MovingPlayerTransparency;

            players = PopulateService.PopulatePlayers(new Rigidbody2D[]{player11, player12, player13, player21, player22, player23});
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
            isOutOfBoard = ChecksService.CheckIfOutOfBoard(rectTransform.position, nodeTopRight.position, nodeBottomLeft.position);
            MovePiece(isOutOfBoard);
        }
    }
    private Slot GetFinalSlot()
    {
        float distance = float.MaxValue;
        Slot finalSlot = new Slot("temp", new Vector2(0, 0));

        foreach (Slot slot in GameManager.boardSlots3x3)
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

        if (isOutOfBoard || !ChecksService.SlotIsFree(finalSlot) || !ValidPlayer())
        {
            rectTransform.position = pieceStartSlot.SVector2;
            canvasGroup.alpha = canvasDotAlpha;
        }
        else
        {
            Rigidbody2D currentPlayer = GetCurrentPiece();
            rectTransform.position = finalSlot.SVector2;

            SetPlayersMovedService.SetPlayersMoved3x3(rectTransform);

            foreach (Slot slot in GameManager.boardSlots3x3)
            {
                if (slot.SOccupier?.name == rectTransform.name)
                {
                    slot.SOccupier = null;
                    break;
                }
            }

            foreach (Slot slot in GameManager.boardSlots3x3)
            {
                if (Vector2.Distance(slot.SVector2, rectTransform.position) < 0.25)
                {
                    slot.SOccupier = currentPlayer;
                    break;
                }
            }

            canvasGroup.alpha = Constants.StandingPlayerTransparency;

            if (ChecksService.IsWinner(GameManager.boardSlots3x3))
            {
                GameManager.gameOver = true;
                gameStatus.color = Color.blue;
                gameStatus.text = "YOU WIN!";
            }
            else
            {
                ChangeAction.ChangePlayerInTurn();
                ComputerMove();
            }
        }
    }
    private Rigidbody2D GetCurrentPiece()
    {
        Rigidbody2D currentPlayer = new Rigidbody2D();
        foreach (Rigidbody2D player in players)
            if (player.name == rectTransform.name) currentPlayer = player;
        return currentPlayer;
    }
    private void PopulateSlotCentres()
    {
        GameManager.boardSlots3x3 = new Slot[9];

        GameManager.boardSlots3x3[0] = new Slot("slot11", slot11.position);
        GameManager.boardSlots3x3[1] = new Slot("slot21", slot21.position);
        GameManager.boardSlots3x3[2] = new Slot("slot31", slot31.position);
        GameManager.boardSlots3x3[3] = new Slot("slot12", slot12.position);
        GameManager.boardSlots3x3[4] = new Slot("slot22", slot22.position);
        GameManager.boardSlots3x3[5] = new Slot("slot32", slot32.position);
        GameManager.boardSlots3x3[6] = new Slot("slot13", slot13.position);
        GameManager.boardSlots3x3[7] = new Slot("slot23", slot23.position);
        GameManager.boardSlots3x3[8] = new Slot("slot33", slot33.position);
    }
    private void AddSlotCentresOccupiers()
    {
        if (GameManager.newGame)
        {
            GameManager.boardSlots3x3[0].SOccupier = player11;
            GameManager.boardSlots3x3[1].SOccupier = player12;
            GameManager.boardSlots3x3[2].SOccupier = player13;
            GameManager.boardSlots3x3[6].SOccupier = player21;
            GameManager.boardSlots3x3[7].SOccupier = player22;
            GameManager.boardSlots3x3[8].SOccupier = player23;
        }
    }
    private void PopulatePlayersLocation()
    {
        playersLocation = new List<Slot>();
        foreach (Slot slot in GameManager.boardSlots3x3)
        {
            if (slot.SOccupier) playersLocation.Add(slot);
        }
    }
    private bool ValidPlayer() =>
        rectTransform.name.ToCharArray()[6].ToString() == GameManager.playerInTurn.ToString();
    public void ComputerMove()
    {
        if (GameManager.difficulty == 1)
        {
            newComputerSlot = PickAction.PickRandomSlot();
            newComputerPiece = PickAction.PickComputerPiece(players);
        }
        else
        {
            int boardScore = int.MinValue;
            Rigidbody2D computerPieceToMove = new Rigidbody2D(); // Populate with final computer piece to move decision
            Slot newSlot = new Slot("temp", new Vector2(0, 0)); // Populate with final slot to move piece to
            Rigidbody2D[] computerPlayers = players.Where(player => (player.name.ToCharArray()[6].ToString() == "2")).ToArray();

            foreach (Rigidbody2D computerPiece in computerPlayers)
            {
                bool moved = ChecksService.CheckIfMoved(computerPiece);
                Vector2 oldPosition = computerPiece.position;





                if (GameManager.difficulty == 2)
                {
                    Slot[] tempBoard = CopyService.CopySlotsArray(GameManager.boardSlots3x3);// GameManager.boardSlots3x3;
                    Slot[] freeSlots = tempBoard.Where(slot => (slot.SOccupier == null)).ToArray();
                    foreach (Slot slot in freeSlots)
                    {
                        tempBoard = UpdateBoardSlotsService.UpdateBoardSlots(tempBoard, computerPiece, slot, false);
                        if (!moved) SetPlayersMovedService.SetPlayersMoved3x3(computerPiece);
                        int score = CalculationsService.CalculateBoardScore(tempBoard);
                        if (!moved) SetPlayersMovedService.SetPlayersNotMoved3x3(computerPiece);
                        if (score > boardScore)
                        {
                            boardScore = score;
                            computerPieceToMove = computerPiece;
                            newSlot = slot;
                        }
                    }
                }






                else if (GameManager.difficulty == 3)
                {
                    // put here the logic of checking all available moves in a depth of 2 check levels
                }
                computerPiece.position = oldPosition;
            }

            newComputerSlot = GameManager.boardSlots3x3.Where(slot => (slot.SName == newSlot.SName)).ToArray()[0];
            newComputerPiece = computerPieceToMove;
        }
        GameManager.boardSlots3x3 =
            UpdateBoardSlotsService.UpdateBoardSlots(GameManager.boardSlots3x3, newComputerPiece, newComputerSlot, true);

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
        if (ChecksService.IsWinner(GameManager.boardSlots3x3))
        {
            GameManager.gameOver = true;
            gameStatus.color = Color.red;
            gameStatus.text = "YOU LOSE!";
        }
        ChangeAction.ChangePlayerInTurn();
    }
}