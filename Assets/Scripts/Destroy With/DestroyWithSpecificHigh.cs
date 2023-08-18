using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithSpecificHigh : MonoBehaviour
{
    [SerializeField] private float specificHigh = -20f;

    private void FixedUpdate()
    {
        if (transform.position.y <= specificHigh) Destroy(gameObject);
    }
}
