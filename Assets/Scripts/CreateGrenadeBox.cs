using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrenadeBox : MonoBehaviour
{
    public List<GameObject> GrenadePickupPoints = new List<GameObject>();
    public GameObject pickupGrenade;
    public static bool GrenadeBoxCreated;
    int rand;
    // Start is called before the first frame update
    void Start()
    {
        GrenadeBoxCreated = false;
        StartCoroutine(Create());
    }

    IEnumerator Create()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(3f);
            //Debug.Log("sa gardaþ");
            if (!GrenadeBoxCreated)
            {
                rand = Random.Range(0, GrenadePickupPoints.Count);
                Instantiate(pickupGrenade, GrenadePickupPoints[rand].transform.position, GrenadePickupPoints[rand].transform.rotation);
                GrenadeBoxCreated = true;
            }
            

        }


    }
}
