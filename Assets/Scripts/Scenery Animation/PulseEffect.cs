using UnityEngine;
using SonicBloom.Koreo;
using UnityEditor;

public class PulseEffect : MonoBehaviour
{
    public Animation animCom;

    [EventID]
    public string bassDrumEventID = "BassDrum";

    void Awake()
    {
        Koreographer.Instance.RegisterForEvents(bassDrumEventID, OnAnimationTrigger);

    }

    void OnAnimationTrigger(KoreographyEvent evt)
    {
        animCom.Stop();
        animCom.Play();
    }


}

