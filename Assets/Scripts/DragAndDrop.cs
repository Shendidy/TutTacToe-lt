using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region Class Variables
    private RectTransform rectTransform;
    [SerializeField] private Canvas mainCanvas;
    public Transform boardCanvas;
    private CanvasGroup canvasGroup;
    public Rigidbody2D nodeTopRight;
    public Rigidbody2D nodeBottomLeft;
    private Vector2 pieceStartPosition;
    private Slot[] slotCentres;
    private Rigidbody2D[] players;
    private List<(string player, Slot location)> playersLocation;
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
    #endregion

    private void Awake()
    {
        slotCentres = new Slot[9];
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        pieceStartPosition = transform.position;
        canvasGroup.alpha = 0.6f;

        PopulatePlayers();
        PopulatePlayersLocation();
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;

        PopulateSlotCentres();

        var isOutOfBoard = CheckIfOutOfBoard();

        MovePiece(isOutOfBoard);
    }

    public void OnDrop(PointerEventData eventData){}

    public void OnPointerDown(PointerEventData eventData){}

    private bool CheckIfOutOfBoard()
    {
        if(rectTransform.position.x > nodeTopRight.position.x ||
           rectTransform.position.x < nodeBottomLeft.position.x ||
           rectTransform.position.y > nodeTopRight.position.y ||
           rectTransform.position.y < nodeBottomLeft.position.y)
            return true;
        
        return false;
    }

    private Vector2 GetFinalPosition()
    {
        float distance = float.MaxValue;
        Vector2 vector = new Vector2();
        foreach (Slot slot in slotCentres)
            if (Vector2.Distance(rectTransform.position, slot.SVector2) < distance)
            {
                distance = Vector2.Distance(rectTransform.position, slot.SVector2);
                vector = slot.SVector2;
            }

        return vector;
    }

    private void PopulateSlotCentres()
    {
        //slotCentres = new Slot[9];
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

    private void MovePiece(bool isOutOfBoard)
    {
        if (isOutOfBoard || !SlotIsFree()) rectTransform.position = pieceStartPosition;
        else
        {
            Vector2 finalSlot = GetFinalPosition();
            rectTransform.position = finalSlot;
        }
    }

    private bool SlotIsFree()
    {
        return false;
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
        playersLocation = new List<(string player, Slot location)>();
        foreach(Slot slot in slotCentres)
        {
            foreach(Rigidbody2D player in players)
            {
                if (Vector2.Distance(player.position, slot.SVector2) < 0.25)// Debug.Log(player.name + " is in slot: " + slot.SName);
                    playersLocation.Add((player.name, slot));
            }
        }
    }
}

public class Slot
{
    public string SName { get; set; }
    public Vector2 SVector2 { get; set; }

    public Slot(string sName, Vector2 sVector2)
    {
        SName = sName;
        SVector2 = sVector2;
    }

    //public Slot(string sName, Vector2 sVector2)
    //{
    //    _SName = sName;
    //    _SVector2 = sVector2;
    //}

    //public void SetVector2(Vector2 sVector2)
    //{
    //    _SVector2 = sVector2;
    //}
}
