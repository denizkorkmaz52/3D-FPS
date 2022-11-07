using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAmmoBox : MonoBehaviour
{
    public List<GameObject> ammoPickupPoints = new List<GameObject>();
    public GameObject pickupAmmo;
    public static bool ammoBoxCreated;
    public static List<int> points = new List<int>();
    int rand;
    // Start is called before the first frame update
    void Start()
    {
        ammoBoxCreated = false;
        StartCoroutine(create());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator create()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            //Debug.Log("sa gardaþ");
            rand = Random.Range(0, ammoPickupPoints.Count);
            if (!points.Contains(rand))
            {
                
                points.Add(rand);
            }
            else
            {
                rand = Random.Range(0, ammoPickupPoints.Count);
                continue;
            }
            GameObject box = Instantiate(pickupAmmo, ammoPickupPoints[rand].transform.position, ammoPickupPoints[rand].transform.rotation);
            box.transform.gameObject.GetComponentInChildren<AmmoScript>().point = rand;

        }


    }
}
