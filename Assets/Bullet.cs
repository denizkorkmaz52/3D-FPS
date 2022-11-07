using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.up * 250;
        Destroy(gameObject,1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
