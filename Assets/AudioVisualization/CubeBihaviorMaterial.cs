using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBihaviorMaterial : MonoBehaviour
{
    public int band;
    public float startScale, scaleMultiplier;
    public AudioPeer audioPeer;

    public bool useBuffer;
    private Material material;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        if (useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (audioPeer.bandBuffer[band] * scaleMultiplier) + startScale, transform.localScale.z);

            Color color = new Color(0f, audioPeer.audioBandBuffer[band], 0f);
            material.SetColor("_EmissionColor", color);
        }

        if (!useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (audioPeer.frequencyBand[band] * scaleMultiplier) + startScale, transform.localScale.z);
            Color color = new Color(audioPeer.audioBand[band], audioPeer.audioBand[band], audioPeer.audioBand[band]);
            material.SetColor("_EmissionColor", color);
        }
    }
}
