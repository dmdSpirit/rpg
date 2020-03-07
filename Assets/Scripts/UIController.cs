using System;
using UnityEngine.EventSystems;

public class UIController : MonoSingleton<UIController>, IPointerEnterHandler, IPointerExitHandler
{
    public event Action OnMouseEnterUI;
    public event Action OnMouseExitUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnterUI?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExitUI?.Invoke();
    }
}