using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
