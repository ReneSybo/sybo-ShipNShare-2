using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerVFX : MonoBehaviour
{
    public ParticleSystem VfxR;
    public ParticleSystem VfxL;
    // Start is called before the first frame update
    public void FireVFXR()
    {
        VfxR.Play();
    }
    public void FireVFXL()
    {
        VfxL.Play();
    }

}
