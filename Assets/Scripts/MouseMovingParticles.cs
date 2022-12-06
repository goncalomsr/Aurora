using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovingParticles : MonoBehaviour
{
    Vector3 particlePosition;
    public float particleSpeed = 10f;

    public Vector3 lastMousePos;
    public Vector3 mouseVelocity;

    public GameObject calm;
    public GameObject steady;
    public GameObject chaotic;

    public AITesting aiTesting;

    public Material calmMaterial;

    // Start is called before the first frame update
    void Start()
    {
        aiTesting = GetComponent<AITesting>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseVelocity = Input.mousePosition - lastMousePos;
        lastMousePos = Input.mousePosition;
        //mouseVelocity.x = mouseVelocity.x / Screen.width;
        //mouseVelocity.y = mouseVelocity.y / Screen.height;

        /*
        particlePosition = Input.mousePosition;
        particlePosition.z = particleSpeed;
        transform.position = Camera.main.ScreenToWorldPoint(particlePosition);
        */

        print(mouseVelocity);


        if (mouseVelocity.x > 200f || mouseVelocity.y > 200f)
        {
            particlePosition = Input.mousePosition;
            particlePosition.z = particleSpeed;
            chaotic.transform.position = Camera.main.ScreenToWorldPoint(particlePosition);
        } 
        else if (mouseVelocity.x > 100f || mouseVelocity.y > 100f)
        {
            particlePosition = Input.mousePosition;
            particlePosition.z = particleSpeed;
            steady.transform.position = Camera.main.ScreenToWorldPoint(particlePosition);
        }
        else if (mouseVelocity.x > 1f || mouseVelocity.y > 1f)
        {
            particlePosition = Input.mousePosition;
            particlePosition.z = particleSpeed;
            calm.transform.position = Camera.main.ScreenToWorldPoint(particlePosition);
        }
    }
}
