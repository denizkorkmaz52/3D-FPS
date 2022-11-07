using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float power;
    public float range;
    public float upPower;
    public ParticleSystem explosionEffect;
    public AudioSource explosionAudio;
    // Start is called before the first frame update
    void Start()
    {
        //explosionAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            Explosion();
            Debug.Log("düþtü");
        }
    }
    void Explosion()
    {
        Vector3 position = transform.position;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        explosionAudio.Play();
        Collider[] colliders = Physics.OverlapSphere(position, range);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (hit != null && rb)
            {
                
                if (hit.gameObject.CompareTag("Enemy"))
                {
                    hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(100);
                }
                rb.AddExplosionForce(power, position, range, upPower, ForceMode.Impulse);
                
            }
        }
        Destroy(gameObject, 1f);
    }
}
