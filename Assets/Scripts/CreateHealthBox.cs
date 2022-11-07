using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHealthBox : MonoBehaviour
{
    public List<GameObject> HealthPickupPoints = new List<GameObject>();
    public GameObject pickupHealth;
    public static bool HealthBoxCreated;
    int rand;
    // Start is called before the first frame update
    void Start()
    {
        HealthBoxCreated = false;
        StartCoroutine(Create());
    }

    IEnumerator Create()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(3f);
            //Debug.Log("sa gardaþ");
            if (!HealthBoxCreated)
            {
                rand = Random.Range(0, HealthPickupPoints.Count);
                Instantiate(pickupHealth, HealthPickupPoints[rand].transform.position, HealthPickupPoints[rand].transform.rotation);
                HealthBoxCreated = true;
            }
            

        }


    }
}
