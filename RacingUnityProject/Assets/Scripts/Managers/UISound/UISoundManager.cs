using UnityEngine;
using Zenject;

public class UISoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource buttonClick;
    [SerializeField]
    private AudioSource buttonHover;

    [Inject] private DiContainer container;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void ButtonClick()
    {
        buttonClick.Play();
    }
    
    public void ButtonHover()
    {
        buttonHover.Play();
    }
}
