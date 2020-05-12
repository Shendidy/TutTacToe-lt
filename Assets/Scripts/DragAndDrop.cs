using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler//, IPointerDownHandler
{
    #region Class Variables
    private RectTransform rectTransform;
    [SerializeField] private Canvas mainCanvas;
    public Transform boardCanvas;
    private CanvasGroup canvasGroup;
    public Rigidbody2D nodeTopRight;
    public Rigidbody2D nodeBottomLeft;
    private Slot pieceStartSlot;
    private Slot[] slotCentres;
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

    private bool newGame = true;
    private bool isOutOfBoard;
    #endregion

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (newGame)
        {
            PopulateSlotCentres();
            AddSlotCentresOccupiers();
            newGame = false;
        }

        foreach (Slot slot in slotCentres)
            if (slot.SOccupier)
                if (slot.SOccupier.name == rectTransform.name) pieceStartSlot = slot;

        canvasGroup.alpha = Constants.MovingPlayerTransparency;

        PopulatePlayers();
        PopulatePlayersLocation();
    }
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag!");
        rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = Constants.StandingPlayerTransparency;
        isOutOfBoard = CheckIfOutOfBoard();
        MovePiece(isOutOfBoard);
    }
    public void OnDrop(PointerEventData eventData) { }
    //public void OnPointerDown(PointerEventData eventData) { }
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
        //Vector2 vector = new Vector2();
        foreach (Slot slot in slotCentres)
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

        if (isOutOfBoard || !SlotIsFree(finalSlot)) rectTransform.position = pieceStartSlot.SVector2;
        else
        {
            Rigidbody2D currentPlayer = GetCurrentPlayer();
            rectTransform.position = finalSlot.SVector2;

            foreach(Slot slot in slotCentres)
            {
                if (slot.SOccupier.name == rectTransform.name)
                {
                    slot.SOccupier = null;
                    break;
                }
            }

            foreach (Slot slot in slotCentres)
            {
                if(Vector2.Distance(slot.SVector2, rectTransform.position) < 0.25)
                {
                    slot.SOccupier = currentPlayer;
                    break;
                }
            }

                //Debug.Log(from slot in slotCentres
                //          where slot.SVector2.x == rectTransform.position.x
                //          where slot.SVector2.y == rectTransform.position.y
                //          select slot.SName.ToString()
                //    );

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
        foreach (Slot slot in slotCentres)
            if (slot == finalSlot && slot.SOccupier) return false;
        return true;
    }
    private void PopulateSlotCentres()
    {
        slotCentres = new Slot[9];
        slotCentres[0] = new Slot("slot11", slot11.position);
        slotCentres[1] = new Slot("slot21", slot21.position);
        slotCentres[2] = new Slot("slot31", slot31.position);
        slotCentres[3] = new Slot("slot12", slot12.position);
        slotCentres[4] = new Slot("slot22", slot22.position);
        slotCentres[5] = new Slot("slot32", slot32.position);
        slotCentres[6] = new Slot("slot13", slot13.position);
        slotCentres[7] = new Slot("slot23", slot23.position);
        slotCentres[8] = new Slot("slot33", slot33.position);
    }
    private void AddSlotCentresOccupiers()
    {
        if (newGame)
        {
            slotCentres[0].SOccupier = player11;
            slotCentres[1].SOccupier = player12;
            slotCentres[2].SOccupier = player13;
            slotCentres[6].SOccupier = player21;
            slotCentres[7].SOccupier = player22;
            slotCentres[8].SOccupier = player23;
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
        foreach (Slot slot in slotCentres)
        {
            if (slot.SOccupier) playersLocation.Add(slot);
        }

        //foreach(Rigidbody2D player in players)
        //if (Vector2.Distance(player.position, slot.SVector2) < 0.25)
        //playersLocation.Add((player.name, slot));
    }
}