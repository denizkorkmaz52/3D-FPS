using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellScript : MonoBehaviour
{
    AudioSource fallSound;
    // Start is called before the first frame update
    void Start()
    {
        fallSound = GetComponent<AudioSource>();
        Destroy(gameObject, 2f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            fallSound.Play();
            
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
