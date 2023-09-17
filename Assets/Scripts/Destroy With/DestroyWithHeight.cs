using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithHeight : MonoBehaviour
{
    [SerializeField] private float height = -20f;

    private void FixedUpdate()
    {
        if (transform.position.y <= height) Destroy(gameObject);
    }
}
