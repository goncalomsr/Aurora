using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowFromADistance : MonoBehaviour
{
    [SerializeField] private float TrackDistanceFromCamera;
    [Range(0.001f,0.3f)][SerializeField] private float LerpStrength = 1.0f;
    private Camera mainCamera = null;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, TrackDistanceFromCamera)), LerpStrength); ;
    }

}
