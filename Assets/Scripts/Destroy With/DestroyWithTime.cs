using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithTime : MonoBehaviour
{
    [SerializeField] private float timeDelay = 60f;
    private float time;

    private void Start()
    {
        time = timeDelay;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0f) Destroy(gameObject);
    }
}
