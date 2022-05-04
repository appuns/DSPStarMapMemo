using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace DSPStarMapMemo
{

    public class ClickHandler : MonoBehaviour, IPointerClickHandler
    {

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                //MarkerList.OnRightClick(eventData.pointerPress);
            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {
            }
        }
    }
}