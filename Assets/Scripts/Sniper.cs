using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Sniper : MonoBehaviour
{
    [Header("Variables")]
    public bool canFire;
    float fireRateTime;
    public float fireRate;
    public float menzil;
    float camFOV;
    float camZoomFov = 12;

    [Header("Others")]
    public GameObject crosshair;
    public GameObject scope;
    public GameController gameController;

    [Header("Audios")]
    public AudioSource fireSound;
    public AudioSource reloadSound;
    public AudioSource emptyMagazine;
    public AudioSource ammoPickupSound;

    [Header("Effects")]
    public ParticleSystem fireEffect;
    public ParticleSystem bulletTrack;
    public ParticleSystem bloodEffect;

    [Header("Gun Info")]
    public string gunsTag;
    int totalBullet;
    public int magazineSize;
    public float damage;
    public float hitForce;
    int remainingBullet;
    public TextMeshProUGUI remainingBulletCountTxt;
    public TextMeshProUGUI totalBulletTxt;
    public GameObject shellDiscard;
    public GameObject shell;

    
    public Camera playerCam;
    Animator animAk;
    // Start is called before the first frame update
    void Start()
    {
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
        PlayerPrefs.SetInt("Remaining" + gunsTag + "_Ammo", remainingBullet);
    }
    // Update is called once per frame
    void Update()
    {
        totalBullet = PlayerPrefs.GetInt(gunsTag + "_Ammo");
        if (Input.GetKey(KeyCode.Mouse0) && !animAk.GetCurrentAnimatorStateInfo(0).IsName(gunsTag + "Reload") && !GameController.isPaused)
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
                animAk.Play(gunsTag + "Reload");
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CollectBullet();
        }
        if (Input.GetMouseButton(1))
        {
            animAk.SetBool("Zoom", true);
            
            Debug.Log("sað týk");
        }
        if (Input.GetMouseButtonUp(1))
        {
            animAk.SetBool("Zoom", false);
            crosshair.SetActive(true);
            scope.SetActive(false);
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
            ammoPickupSound.Play();
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
                ammoPickupSound.Play();
                collectedBullet = hit.transform.gameObject.GetComponent<AmmoScript>().randBullet;
                gunType = hit.transform.gameObject.GetComponent<AmmoScript>().randGun;
                CatchAmmo(collectedBullet, gunType);
                Debug.Log(gunType);
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
        totalBulletTxt.text = PlayerPrefs.GetInt(gunsTag + "_Ammo").ToString();
    }
    void ReloadSoundPlay()
    {
        reloadSound.Play();
        totalBullet = PlayerPrefs.GetInt(gunsTag + "_Ammo");
        //Debug.Log(totalBullet);
        if (totalBullet != 0 && remainingBullet != magazineSize)
        {
            //reloadSoundPlay(); -- animasyonda çaðýrýlýyor
            totalBullet = totalBullet + remainingBullet;

            remainingBullet = 0;
            if (totalBullet >= magazineSize)
            {
                totalBullet -= magazineSize;
                remainingBullet = magazineSize;
                PlayerPrefs.SetInt(gunsTag + "_Ammo", totalBullet);
            }
            else
            {
                remainingBullet = totalBullet;
                totalBullet = 0;
                PlayerPrefs.SetInt(gunsTag + "_Ammo", totalBullet);
            }
            PlayerPrefs.SetInt(gunsTag + "_Ammo", totalBullet);
            PlayerPrefs.SetInt("Remaining" + gunsTag + "_Ammo", remainingBullet);
            WriteBulletCounts();
        }
    }
    void Zoom()
    {
        playerCam.fieldOfView = camZoomFov;
        crosshair.SetActive(false);
        scope.SetActive(true);
        playerCam.cullingMask = ~(1 << 8);
    }
    void Fire()
    {
        animAk.Play(gunsTag + "Shoot");
        fireEffect.Play();
        fireSound.Play();
        GameObject shl = Instantiate(shell, shellDiscard.transform.position, shellDiscard.transform.rotation);
        Rigidbody rb = shl.GetComponent<Rigidbody>();
        rb.AddRelativeForce(new Vector3(-10f, 1f, 5f) * 50f);
        remainingBullet--;
        remainingBulletCountTxt.text = remainingBullet.ToString();
        PlayerPrefs.SetInt("Remaining_" + gunsTag + "_Ammo", remainingBullet);
        animAk.Play(gunsTag + "Idle");
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
