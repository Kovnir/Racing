using UnityEngine;
using UnityEngine.UI;

public class LevelButtonView : MonoBehaviour
{
    [SerializeField] private CanvasGroup star1;
    [SerializeField] private CanvasGroup star2;
    [SerializeField] private CanvasGroup star3;

    public void Init(bool isAvailable, int startsCount)
    {
        GetComponent<Button>().interactable = isAvailable;
        star1.alpha = startsCount > 0 ? 1 : 0.25f;
        star2.alpha = startsCount > 1 ? 1 : 0.25f;
        star3.alpha = startsCount > 2 ? 1 : 0.25f;
    }
}
