using Signals;
using UnityEngine;
using Zenject;

public class CheckPoint : MonoBehaviour
{
    [Inject] private LevelManager levelManager;
    [Inject] private SignalBus signalBus;

    private Material material;
    [SerializeField] private int index;
    [SerializeField] private Color straightColor;
    public int Index
    {
        get { return index; }
    } 
    
    private void Awake()
    {
        levelManager.RegisterCheckPoint(this);
        material = GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>())
        {
            signalBus.Fire(new OnCheckpointAchievedSignal(this));
        }
    }

    public void Close()
    {
        Destroy(this.gameObject);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void ShowStraight()
    {
        material.color = straightColor;
    }
}
