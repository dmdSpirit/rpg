using System;
using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    public event Action<UIPanel> OnPanelShown;
    public event Action<UIPanel> OnPanelHidden;

    public virtual void ShowPanel()
    {
        gameObject.SetActive(true);
        OnPanelShown?.Invoke(this);
    }

    public virtual void HidePanel()
    {
        gameObject.SetActive(false);
        OnPanelHidden?.Invoke(this);
    }
}