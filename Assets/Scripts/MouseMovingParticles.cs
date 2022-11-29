using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovingParticles : MonoBehaviour
{
    Vector3 particlePosition;
    public float particleSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        particlePosition = Input.mousePosition;
        particlePosition.z = particleSpeed;
        transform.position = Camera.main.ScreenToWorldPoint(particlePosition);
    }
}
