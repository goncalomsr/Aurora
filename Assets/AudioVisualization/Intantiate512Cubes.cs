using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intantiate512Cubes : MonoBehaviour
{
    public GameObject cubePrefab;
    private GameObject[] cubes = new GameObject[512];
    public float maxScale;

    public AudioPeer audioPeer;

    // Start is called before the first frame update
    void Start()
    {


        for(int i =0; i < 512; i++)
        {
            GameObject instanceCubes = (GameObject)Instantiate(cubePrefab);
            instanceCubes.transform.position = this.transform.position;
            instanceCubes.transform.parent = this.transform;
            instanceCubes.name = "Cube" + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            instanceCubes.transform.position = Vector3.forward * 100;
            cubes[i] = instanceCubes;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < 512; i++)
        {
            if(cubes != null)
            {
                cubes[i].transform.localScale = new Vector3(1, (audioPeer.samples[i] * maxScale) + 2, 1);
            }
        }
    }
}
