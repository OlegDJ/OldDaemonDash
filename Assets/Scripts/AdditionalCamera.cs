using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalCamera : MonoBehaviour
{
    private GameObject mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main.gameObject;
    }

    private void Update()
    {
        transform.position = mainCamera.transform.position;
        transform.rotation = mainCamera.transform.rotation;
    }
}
