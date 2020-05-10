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

    //slots

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
        if (isOutOfBoard) transform.position = pieceStartPosition;
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
}
