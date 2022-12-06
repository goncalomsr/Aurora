using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehavior: MonoBehaviour
{
    public int band;
    public float startScale, scaleMultiplier;
    public AudioPeer audioPeer;

    public bool useBuffer;

    void Start()
    {
        
    }

    void Update()
    {
        if (useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (audioPeer.bandBuffer[band] * scaleMultiplier) + startScale, transform.localScale.z);
        }

        if (!useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (audioPeer.frequencyBand[band] * scaleMultiplier) + startScale, transform.localScale.z);
        }
    }
}
