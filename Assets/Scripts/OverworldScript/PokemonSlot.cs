using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pokemon
{
    public class PokemonSlot : MonoBehaviour//, IDropHandler
    {
/*        public void OnDrop(PointerEventData eventData)
        {
*//*            Debug.Log("OnDrop");
            if (eventData.pointerDrag != null)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.GetComponent<DragnDrop>().droppedOnSlot = true;
                var currentPos = transform.position;
                transform.position = new Vector3(Mathf.Round(currentPos.x), Mathf.Round(currentPos.y), 0);

            }*//*
        }*/
    }
}

