using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pokemon
{
    public class DragnDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        Vector3 defaultPos;
        public bool droppedOnSlot;

        private void Start()
        {
            defaultPos = transform.position;
        }

        private void Awake()
        {
            //rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            defaultPos = transform.position;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = 0.60f;
            canvasGroup.blocksRaycasts = false;
            eventData.pointerDrag.GetComponent<DragnDrop>().droppedOnSlot = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = 1.00f;
            canvasGroup.blocksRaycasts = true;
            if (!droppedOnSlot)
            {
                transform.position = defaultPos;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            rectTransform.SetAsLastSibling();
        }
    }
}

