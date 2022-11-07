using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoScript : MonoBehaviour
{

    string[] guns =
    {
        "Magnum",
        "Shotgun",
        "Sniper",
        "Rifle"
    };

    int[] bulletCount =
    {
        10,
        20,
        7,
        30
    };

    public List<Sprite> gunSprites = new List<Sprite>();
    public Image gunsImage;
    public string randGun;
    public int randBullet;
    public int point;
    // Start is called before the first frame update
    void Start()
    {
        int key = Random.Range(0, guns.Length);
        randGun = guns[key];
        randBullet = bulletCount[Random.Range(0, bulletCount.Length)];
        gunsImage.sprite = gunSprites[key];
        
    }

}
