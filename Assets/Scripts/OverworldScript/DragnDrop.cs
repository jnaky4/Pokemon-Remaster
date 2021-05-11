using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pokemon
{
    public class DragnDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        Vector3 defaultPos;
        public bool droppedOnSlot;

        private void Start()
        {
            defaultPos = rectTransform.position;
        }

        private void Awake()
        {
            //rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = 0.60f;
            //canvasGroup.blocksRaycasts = false;
            //eventData.pointerDrag.GetComponent<DragnDrop>().droppedOnSlot = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = 1.00f;
            //canvasGroup.blocksRaycasts = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            rectTransform.SetAsLastSibling();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (rectTransform.position.y > -540 /*+ defaultPos.y + 90*/ && rectTransform.position.y <= -360 /*+ defaultPos.y + 90*/) rectTransform.position = new Vector3(defaultPos.x, -450 /*+ defaultPos.y + 90*/, 0);
            else if (rectTransform.position.y > -360 /*+ defaultPos.y + 90*/ && rectTransform.position.y <= -180 /*+ defaultPos.y + 90*/) rectTransform.position = new Vector3(defaultPos.x, -270 /*+ defaultPos.y + 90*/, 0);
            else if (rectTransform.position.y > -180 /*+ defaultPos.y + 90*/ && rectTransform.position.y <= 0 /*+ defaultPos.y + 90*/) rectTransform.position = new Vector3(defaultPos.x, -90 /*+ defaultPos.y + 90*/, 0);
            else if (rectTransform.position.y > 0 /*+ defaultPos.y + 90*/ && rectTransform.position.y <= 180 /*+ defaultPos.y + 90*/) rectTransform.position = new Vector3(defaultPos.x, 90 /*+ defaultPos.y + 90*/, 0);
            else if (rectTransform.position.y > 180 /*+ defaultPos.y + 90*/ && rectTransform.position.y <= 360 /*+ defaultPos.y + 90*/) rectTransform.position = new Vector3(defaultPos.x, 270 /*+ defaultPos.y + 90*/, 0);
            else if (rectTransform.position.y > 360 /*+ defaultPos.y + 90*/ && rectTransform.position.y <= 540 /*+ defaultPos.y + 90*/) rectTransform.position = new Vector3(defaultPos.x, 450 /*+ defaultPos.y + 90*/, 0);
            else rectTransform.position = defaultPos;
        }
    }
}

