using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCountdown : MonoBehaviour
{
    [SerializeField] private GameObject ready;
    [SerializeField] private GameObject steady;
    [SerializeField] private GameObject go;
    
    
    public IEnumerator Show()
    {
        yield return new WaitForSeconds(1);
        ready.SetActive(true);
        yield return new WaitForSeconds(1);
        ready.SetActive(false);
        steady.SetActive(true);
        yield return new WaitForSeconds(1);
        steady.SetActive(false);
        go.SetActive(true);
        
    }
}
