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
    private Vector2[] slotCentres;
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
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        pieceStartPosition = transform.position;
        canvasGroup.alpha = 0.6f;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;

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
        PopulateSlotCentres();

        float distance = float.MaxValue;
        Vector2 vector = new Vector2();
        foreach (Vector2 slot in slotCentres)
            if (Vector2.Distance(rectTransform.position, slot) < distance)
            {
                distance = Vector2.Distance(rectTransform.position, slot);
                vector = slot;
            }

        return vector;
    }

    private void PopulateSlotCentres()
    {
        slotCentres = new Vector2[9];
        slotCentres[0] = slot11.position;
        slotCentres[1] = slot21.position;
        slotCentres[2] = slot31.position;
        slotCentres[3] = slot12.position;
        slotCentres[4] = slot22.position;
        slotCentres[5] = slot32.position;
        slotCentres[6] = slot13.position;
        slotCentres[7] = slot23.position;
        slotCentres[8] = slot33.position;
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
}
