using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticleController : MonoBehaviour {
    private UnityAction callback;

    public void SetCallBack(UnityAction cb)
    {
        this.callback = cb;
    }
    public void OnParticleSystemStopped()
    {
        Debug.Log("Stop");
        this.callback();
    }
}
