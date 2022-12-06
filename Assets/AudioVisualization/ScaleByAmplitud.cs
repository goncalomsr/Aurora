using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleByAmplitud : MonoBehaviour
{   
    public float startScale, scaleMultiplier;
    public AudioPeer audioPeer;

    public bool useBuffer;
    private Material material;

    public float red, green, blue;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        if (useBuffer)
        {
            transform.localScale = new Vector3((audioPeer.amplitudeBuffer * scaleMultiplier) + startScale, (audioPeer.amplitudeBuffer * scaleMultiplier) + startScale, (audioPeer.amplitudeBuffer * scaleMultiplier) + startScale);

            Color color = new Color(red * audioPeer.amplitude, green * audioPeer.amplitude, blue * audioPeer.amplitude);
            material.SetColor("_EmissionColor", color);
        }

        if (!useBuffer)
        {
            transform.localScale = new Vector3((audioPeer.amplitude * scaleMultiplier) + startScale, (audioPeer.amplitude * scaleMultiplier) + startScale, (audioPeer.amplitude * scaleMultiplier) + startScale);

            Color color = new Color(red * audioPeer.amplitude, green * audioPeer.amplitude, blue * audioPeer.amplitude);
            material.SetColor("_EmissionColor", color);
        }
    }
}
