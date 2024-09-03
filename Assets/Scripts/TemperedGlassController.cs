using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperedGlassController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke("OnActionBroken",1);
        }
    }
    void OnActionBroken()
    {
        rb.useGravity = true;
    }
}
