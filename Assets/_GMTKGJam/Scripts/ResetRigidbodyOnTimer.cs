using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ResetRigidbodyOnTimer : MonoBehaviour
{
    public float randomVariance = 3f;
    public float delayDuration = 5f;
    public float timerDuration = 20f;

    private Rigidbody rb;

    private Vector3 initPos;
    private Quaternion initRot;
    private float timer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initRot = transform.rotation;
        initPos = transform.position;
        timer = delayDuration + randomVariance * (1f - 2f * Random.value);
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ResetRB();
            ResetTimer();
        }
    }
    private void ResetTimer()
    {
        timer = timerDuration + randomVariance * (1f - 2f * Random.value);
    }
    private void ResetRB()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.MovePosition(initPos);
        rb.MoveRotation(initRot);
    }
}
