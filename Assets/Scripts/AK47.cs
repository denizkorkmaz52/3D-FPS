using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AK47 : MonoBehaviour
{
    [Header("Variables")]
    public bool canFire;
    float fireRateTime;
    public float fireRate;
    public float menzil;
    float camFOV;
    public float camZoomFov = 30;
    bool isZoomed = false;

    [Header("Audios")]
    public AudioSource fireSound;
    public AudioSource reloadSound;
    public AudioSource emptyMagazine;

    [Header("Effects")]
    public ParticleSystem fireEffect;
    public ParticleSystem bulletTrack;
    public ParticleSystem bloodEffect;

    [Header("Gun Info")]
    public string gunsTag;
    int totalBullet;
    public float damage;
    public float hitForce;
    public int magazineSize;
    int remainingBullet;
    public TextMeshProUGUI remainingBulletCountTxt;
    public TextMeshProUGUI totalBulletTxt;
    public GameObject shellDiscard;
    public GameObject shell;
    public GameObject bullet;
    public GameObject bulletDiscard;

    [Header("Others")]
    public Camera playerCam;
    public GameObject crosshair;
    public GameController gameController;
    Animator animAk;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.SetInt(gunsTag + "_Ammo", 900);
        totalBullet = PlayerPrefs.GetInt(gunsTag + "_Ammo");
        remainingBullet = PlayerPrefs.GetInt("Remaining_" + gunsTag + "_Ammo");
        StartLoadMagazine();
        WriteBulletCounts();
        animAk = GetComponent<Animator>();
        camFOV = playerCam.fieldOfView;
        
    }
    void StartLoadMagazine()
    {
        totalBullet = PlayerPrefs.GetInt(gunsTag + "_Ammo") + PlayerPrefs.GetInt("Remaining_" + gunsTag + "_Ammo");
        if (totalBullet >= magazineSize)
        {
            remainingBullet = magazineSize;
            totalBullet -= magazineSize;
        }
        else
        {
            remainingBullet = totalBullet;
            totalBullet = 0;
        }
        PlayerPrefs.SetInt(gunsTag + "_Ammo", totalBullet);
        PlayerPrefs.SetInt("Remaining_" + gunsTag + "_Ammo", remainingBullet);
    }
    // Update is called once per frame
    void Update()
    {
        totalBullet = PlayerPrefs.GetInt(gunsTag + "_Ammo");
        
        if (Input.GetKey(KeyCode.Mouse0) && !animAk.GetCurrentAnimatorStateInfo(0).IsName("reload") && !GameController.isPaused)
        {

            if (canFire && Time.time >= fireRateTime && remainingBullet != 0)
            {
                Fire();          
                fireRateTime = fireRate + Time.time;
            }
            else if(remainingBullet == 0)
            {
                emptyMagazine.Play();
            }
           
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (totalBullet != 0 && remainingBullet != magazineSize)
            {
                animAk.Play("reload");
                
                //reloadSoundPlay(); -- animasyonda çaðýrýlýyor
                totalBullet = totalBullet + remainingBullet;
                
                remainingBullet = 0;
                if (totalBullet >= magazineSize)
                {
                    totalBullet -= magazineSize;
                    remainingBullet = magazineSize;
                    
                }
                else
                {
                    remainingBullet = totalBullet;
                    totalBullet = 0;
                    
                }
                PlayerPrefs.SetInt(gunsTag + "_Ammo", totalBullet);
                PlayerPrefs.SetInt("Remaining" + gunsTag + "_Ammo", remainingBullet);
                WriteBulletCounts();
            }

            /*if (!reloadSound.isPlaying)
            {
                reloadSound.Play();
            }*/

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CollectBullet();
        }
        if (Input.GetMouseButton(1))
        {
            animAk.SetBool("Zoom", true);
            isZoomed = true;
            Debug.Log("sað týk");
        }
        if (Input.GetMouseButtonUp(1))
        {
            animAk.SetBool("Zoom", false);
            crosshair.SetActive(true);
            isZoomed = false;
            playerCam.cullingMask = -1;
            playerCam.fieldOfView = camFOV;
            Debug.Log("sað týk býraktým");
        }


    }

    //---------------------picking up ammunation-------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ammo"))
        {
            Debug.Log("çarpýþtýk " + other.gameObject.name);
            CatchAmmo(other.transform.gameObject.GetComponent<AmmoScript>().randBullet, other.transform.gameObject.GetComponent<AmmoScript>().gunsImage.sprite.name);
            CreateAmmoBox.points.Remove(other.transform.gameObject.GetComponent<AmmoScript>().point);
            
            Destroy(other.transform.parent.gameObject);

        }
        else if (other.gameObject.CompareTag("HealthBox"))
        {
            Debug.Log("çarpýþtýk " + other.gameObject.name);
            gameController.TakeMedBox();
            CreateHealthBox.HealthBoxCreated = false;
            Destroy(other.transform.gameObject);

        }
        else if (other.gameObject.CompareTag("GrenadeBox"))
        {
            Debug.Log("çarpýþtýk " + other.gameObject.name);
            gameController.TakeGrenadeBox();
            CreateGrenadeBox.GrenadeBoxCreated = false;
            Destroy(other.transform.gameObject);

        }
    }
    void CollectBullet()
    {

        int collectedBullet;
        string gunType;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hit, 3))
        {
            if (hit.transform.CompareTag("Ammo"))
            {
                collectedBullet = hit.transform.gameObject.GetComponent<AmmoScript>().randBullet;
                gunType = hit.transform.gameObject.GetComponent<AmmoScript>().randGun;
                CatchAmmo(collectedBullet, hit.transform.gameObject.GetComponent<AmmoScript>().gunsImage.sprite.name);
                Debug.Log("hee aldýn aldýn");
                CreateAmmoBox.points.Remove(hit.transform.gameObject.GetComponent<AmmoScript>().point);
                Destroy(hit.transform.parent.gameObject);
            }
            Debug.Log(hit.transform.name);
        }
    }
    void CatchAmmo(int ammoCount, string gunType)
    {

        PlayerPrefs.SetInt(gunType + "_Ammo", PlayerPrefs.GetInt(gunType + "_Ammo") + ammoCount);
        WriteBulletCounts();
        //silah türünü de iþin içine katýnca burasý daha mantýklý olucak
    }
    //--------------------------------------------------------------------------
    public void WriteBulletCounts()
    {
        remainingBulletCountTxt.text = remainingBullet.ToString();
        totalBulletTxt.text = PlayerPrefs.GetInt("Rifle_Ammo").ToString();
    }
    void ReloadSoundPlay()
    {
        reloadSound.Play();
    }
    void Zoom()
    {
        crosshair.SetActive(false);
        playerCam.fieldOfView = camZoomFov;
    }
    void Fire()
    {
        if (!isZoomed)
        {
            //Debug.Log("girdim");
            animAk.Play("ak47_anim");
        }
        else
        {         
            animAk.Play("Rifle_Zoom_Shoot");
        }
        fireEffect.Play();
        fireSound.Play();
        GameObject shl = Instantiate(shell, shellDiscard.transform.position, shellDiscard.transform.rotation);
        Instantiate(bullet, bulletDiscard.transform.position, bulletDiscard.transform.rotation);
        Rigidbody rb = shl.GetComponent<Rigidbody>();
        rb.AddRelativeForce(new Vector3(10f, 1, 0) * 30f);
        remainingBullet--;
        remainingBulletCountTxt.text = remainingBullet.ToString();
        PlayerPrefs.SetInt("Remaining_" + gunsTag + "_Ammo", remainingBullet);

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hit, menzil))
        {

            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                ParticleSystem effect = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(effect, 3f);
                hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
            else if (hit.transform.gameObject.CompareTag("OverTurnable"))
            {
                Rigidbody rg = hit.transform.gameObject.GetComponent<Rigidbody>();
                rg.AddForce(-hit.normal * hitForce);
                ParticleSystem effect = Instantiate(bulletTrack, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(effect, 3f);
            }
            else
            {
                ParticleSystem effect = Instantiate(bulletTrack, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(effect, 3f);
            }

            Debug.Log(hit.transform.name);
        }

    }
}
