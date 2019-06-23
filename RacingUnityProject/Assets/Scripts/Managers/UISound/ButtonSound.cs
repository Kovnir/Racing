using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Inject] private UISoundManager soundManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (soundManager != null)
        {
            soundManager.ButtonHover();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (soundManager != null)
        {
            soundManager.ButtonClick();
        }
    }
}
