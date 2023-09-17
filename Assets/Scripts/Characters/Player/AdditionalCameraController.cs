using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalCameraController : MonoBehaviour
{
    private GameObject mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main.gameObject;
    }

    private void LateUpdate()
    {
        transform.position = mainCamera.transform.position;
        transform.rotation = mainCamera.transform.rotation;
    }
}
