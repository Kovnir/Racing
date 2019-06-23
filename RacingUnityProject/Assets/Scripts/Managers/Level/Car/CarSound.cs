using Kovnir.FastTweener;
using UnityEngine;

public class CarSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource start;
    [SerializeField]
    private AudioSource drifting1;
    [SerializeField]
    private AudioSource drifting2;
    [SerializeField]
    private AudioSource boost;
    [SerializeField]
    private AudioSource idle;
    
    FastTween boostTween;
    private float startBoostVolume;

    private void Awake()
    {
        startBoostVolume = boost.volume;
    }

    public void PlayStart()
    {
        start.Play();
        idle.Play();
    }

    private bool lastDriftingIsSecond;
    public void StartDrifting()
    {
        if (!drifting1.isPlaying && !drifting2.isPlaying)
        {
            if (lastDriftingIsSecond)
            {
                drifting1.Play();
            }
            else
            {
                drifting2.Play();
            }
            lastDriftingIsSecond = !lastDriftingIsSecond;
        }
    }

    public void EndDrifting()
    {
//        drifting.Stop();
    }
    
    public void StartBoost()
    {
        boostTween.Kill();
        boost.volume = startBoostVolume;
        if (!boost.isPlaying)
        {
            boost.Play();
        }
    }

    public void EndBoost()
    {
        boostTween.Kill();
        boostTween = FastTweener.Float(boost.volume, 0, 0.5f, f => { boost.volume = f; }, () =>
        {
            boost.Stop();            
        });
    }


    private void OnDestroy()
    {
        boostTween.Kill();
    }
}
