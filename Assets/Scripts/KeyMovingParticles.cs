using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMovingParticles : MonoBehaviour
{
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
        //if (Input.GetKey(KeyCode.Space))
        //{
            randomPosX = Random.Range(-15f, 15f);
            randomPosY = Random.Range(-10f, 10f);
            particlePosition = new Vector3(randomPosX, randomPosY, 0);
            transform.position = Vector3.Lerp(transform.position, particlePosition, particleSpeed * Time.deltaTime);
        //}

        /*
        particlePosition = Input.mousePosition;
        particlePosition.z = particleSpeed;
        transform.position = Camera.main.ScreenToWorldPoint(particlePosition);
        */

        print(randomPosX);
        print(randomPosY);
    }
}
