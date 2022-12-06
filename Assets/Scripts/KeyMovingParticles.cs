using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMovingParticles : MonoBehaviour
{
    public GameObject calm;
    public GameObject steady;
    public GameObject chaotic;

    public float randomPosX;
    public float randomPosY;
    Vector3 particlePosition;
    private float particleSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // ALL
        if (Input.GetKey(KeyCode.Space))
        {
            randomPosX = Random.Range(-15f, 15f);
            randomPosY = Random.Range(-10f, 10f);
            particlePosition = new Vector3(randomPosX, randomPosY, 0);
            transform.position = Vector3.Lerp(transform.position, particlePosition, particleSpeed * Time.deltaTime);
        }

        // CALM
        if (Input.GetKey(KeyCode.E))
        {
            randomPosX = Random.Range(-15f, 15f);
            randomPosY = Random.Range(-10f, 10f);
            particlePosition = new Vector3(randomPosX, randomPosY, 0);
            calm.transform.position = Vector3.Lerp(calm.transform.position, particlePosition, particleSpeed * Time.deltaTime);
        }

        // STEADY
        if (Input.GetKey(KeyCode.W))
        {
            randomPosX = Random.Range(-15f, 15f);
            randomPosY = Random.Range(-10f, 10f);
            particlePosition = new Vector3(randomPosX, randomPosY, 0);
            steady.transform.position = Vector3.Lerp(steady.transform.position, particlePosition, particleSpeed * Time.deltaTime);
        }

        // CHAOTIC
        if (Input.GetKey(KeyCode.Q))
        {
            randomPosX = Random.Range(-15f, 15f);
            randomPosY = Random.Range(-10f, 10f);
            particlePosition = new Vector3(randomPosX, randomPosY, 0);
            chaotic.transform.position = Vector3.Lerp(chaotic.transform.position, particlePosition, particleSpeed * Time.deltaTime);
        }

        /*
        particlePosition = Input.mousePosition;
        particlePosition.z = particleSpeed;
        transform.position = Camera.main.ScreenToWorldPoint(particlePosition);
        */

        print(randomPosX);
        print(randomPosY);
    }
}
