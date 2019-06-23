using System.Collections;
using System.Collections.Generic;
using Kovnir.FastTweener;
using UnityEngine;

public class StartCountdown : MonoBehaviour
{
    [SerializeField] private GameObject ready;
    [SerializeField] private GameObject steady;
    [SerializeField] private GameObject go;

    private FastTween tween;
    
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
        tween = FastTweener.Schedule(1, () => { go.SetActive(false); });
    }

    private void OnDestroy()
    {
        tween.Kill();
    }
}
