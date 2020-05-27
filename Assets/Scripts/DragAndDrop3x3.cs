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
        GameManager.playersMoved3x3 = new bool[] { false, false, false, false, false, false };
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

            foreach (Slot slot in GameManager.slotCentres3x3)
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

        foreach (Slot slot in GameManager.slotCentres3x3)
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

            SetPlayersMovedService.SetPlayersMoved3x3(rectTransform);

            foreach (Slot slot in GameManager.slotCentres3x3)
            {
                if (slot.SOccupier?.name == rectTransform.name)
                {
                    slot.SOccupier = null;
                    break;
                }
            }

            foreach (Slot slot in GameManager.slotCentres3x3)
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
        foreach (Slot slot in GameManager.slotCentres3x3)
            if (slot == finalSlot && slot.SOccupier) return false;
        return true;
    }
    private void PopulateSlotCentres()
    {
        GameManager.slotCentres3x3 = new Slot[9];

        GameManager.slotCentres3x3[0] = new Slot("slot11", slot11.position);
        GameManager.slotCentres3x3[1] = new Slot("slot21", slot21.position);
        GameManager.slotCentres3x3[2] = new Slot("slot31", slot31.position);
        GameManager.slotCentres3x3[3] = new Slot("slot12", slot12.position);
        GameManager.slotCentres3x3[4] = new Slot("slot22", slot22.position);
        GameManager.slotCentres3x3[5] = new Slot("slot32", slot32.position);
        GameManager.slotCentres3x3[6] = new Slot("slot13", slot13.position);
        GameManager.slotCentres3x3[7] = new Slot("slot23", slot23.position);
        GameManager.slotCentres3x3[8] = new Slot("slot33", slot33.position);
    }
    private void AddSlotCentresOccupiers()
    {
        if (GameManager.newGame)
        {
            GameManager.slotCentres3x3[0].SOccupier = player11;
            GameManager.slotCentres3x3[1].SOccupier = player12;
            GameManager.slotCentres3x3[2].SOccupier = player13;
            GameManager.slotCentres3x3[6].SOccupier = player21;
            GameManager.slotCentres3x3[7].SOccupier = player22;
            GameManager.slotCentres3x3[8].SOccupier = player23;
        }
    }
    private void PopulatePlayersLocation()
    {
        playersLocation = new List<Slot>();
        foreach (Slot slot in GameManager.slotCentres3x3)
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
            PickRandomSlot();
            PickComputerPiece();
        }














        // all you need here is to set newComputerSlot and newComputerPiece...
        else
        {
            Slot[] slotsArray = GameManager.slotCentres3x3;
            Rigidbody2D computerPieceToMove = new Rigidbody2D();
            Slot newSlot = new Slot("temp", new Vector2(0, 0));
            Rigidbody2D[] computerPlayers = players.Where(player => (player.name.ToCharArray()[6].ToString() == "2")).ToArray();

            foreach (Rigidbody2D computerPiece in players)
            {
                bool moved = ChecksService.CheckIfMoved(computerPiece);
                if (!moved) SetPlayersMovedService.SetPlayersMoved3x3(computerPiece);

                if (GameManager.difficulty == 2)
                {
                    // put here the logic of checking all available moves in a depth of 1 check level
                    if (computerPiece.name.ToCharArray()[6].ToString() == "2")
                    {
                        //move piece to new location and update game arrays!
                    }
                }



                else if (GameManager.difficulty == 3)
                {
                    // put here the logic of checking all available moves in a depth of 2 check levels
                }

                // If didn't move before this check then mark as unmoved
                if (!moved) SetPlayersMovedService.SetPlayersNotMoved3x3(computerPiece);
            }

            newComputerSlot = newSlot;
            newComputerPiece = computerPieceToMove;
        }

        // ******************************************************* //
        // *************** Above is the trial code *************** //
        // ******************************************************* //

        //boardScore = CalculationsService.CalculateBoardScore();

        // Get old slot
        Slot oldSlot = GameManager.slotCentres3x3.Where(slot => (slot.SOccupier == newComputerPiece)).ToArray()[0];

        newComputerPiece.position = newComputerSlot.SVector2;
        SetPlayersMovedService.SetPlayersMoved3x3(newComputerPiece);

        // Empty SOccupier of player 2 old slot
        oldSlot.SOccupier = null;

        // Add SOccupier to newSlot
        foreach (Slot slot in GameManager.slotCentres3x3)
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

    private void PickRandomSlot()
    {
        System.Random random = new System.Random();

        while (true)
        {
            int i = random.Next(0, (int)Math.Pow(GameManager.boardWidth, 2));
            if (GameManager.slotCentres3x3[i].SOccupier == null)
            {
                newComputerSlot = GameManager.slotCentres3x3[i];
                break;
            }
        }
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
        if(DidAllPlayerPiecesMove())
        {
            Slot[] tempSlotsArray = GameManager.slotCentres3x3.Where(slot
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
    private bool DidAllPlayerPiecesMove()
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
}