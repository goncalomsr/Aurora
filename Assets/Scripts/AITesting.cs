using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITesting : MonoBehaviour
{
    private Material material;

    public Color calmColor;
    public Color steadyColor;
    public Color chaoticColor;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<ParticleSystemRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            material.SetColor("_EmissionColor", Color.Lerp(calmColor, steadyColor, 1f * Time.deltaTime));
        }
    }

    
    IEnumerator CalmToSteady()
    {
        yield return new WaitForSeconds(3f);
        print("done");
    }
    
}
