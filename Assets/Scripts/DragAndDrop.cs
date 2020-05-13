using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region Class Variables
    private RectTransform rectTransform;
    [SerializeField] private Canvas mainCanvas;
    public Transform boardCanvas;
    private CanvasGroup canvasGroup;
    public CanvasGroup canvasGroup21;
    public CanvasGroup canvasGroup22;
    public CanvasGroup canvasGroup23;
    public Rigidbody2D nodeTopRight;
    public Rigidbody2D nodeBottomLeft;
    private Slot pieceStartSlot;
    private Rigidbody2D[] players;
    private List<Slot> playersLocation;
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
        GameManager.newGame = true;
        GameManager.playerInTurn = 1;
    }

    public void OnBeginDrag(PointerEventData eventData)
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
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        isOutOfBoard = CheckIfOutOfBoard();
        MovePiece(isOutOfBoard);
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
            GameManager.playerInTurn = GameManager.playerInTurn == 1 ? 2 : 1;
            if (GameManager.playerInTurn == 2) ComputerMove();
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
        Slot newSlot = PickSlot();
        Rigidbody2D newPlayerPiece = PickPlayerPiece();
        Slot oldSlot = new Slot("temp", new Vector2(0, 0));

        // Get old slot
        foreach (Slot slot in GameManager.slotCentres)
        {
            if (slot.SOccupier == newPlayerPiece)
            {
                oldSlot = slot;
                break;
            }
        }

        Thread.Sleep(250);
        newPlayerPiece.position = newSlot.SVector2;

        // Empty SOccupier of player 2 old slot
        oldSlot.SOccupier = null;

        // Add SOccupier to newSlot
        foreach (Slot slot in GameManager.slotCentres)
        {
            if (Vector2.Distance(slot.SVector2, newPlayerPiece.position) < 0.25)
            {
                slot.SOccupier = newPlayerPiece;
                break;
            }
        }

        // Make alpha of newPlayerPiece = 1
        switch (newPlayerPiece.name)
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

        GameManager.playerInTurn = 1;
    }
    private Slot PickSlot()
    {
        System.Random random = new System.Random();

        while(true)
        {
            int i = random.Next(0, 9);
            if(GameManager.slotCentres[i].SOccupier == null) return GameManager.slotCentres[i];
        }
    }
    private Rigidbody2D PickPlayerPiece()
    {
        System.Random random = new System.Random();

        while(true)
        {
            int i = random.Next(0, players.Length);
            if (players[i].name.ToCharArray()[6].ToString() == "2") return players[i];
        }
    }
}