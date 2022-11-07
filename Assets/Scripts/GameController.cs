using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class GameController : MonoBehaviour
{
    [Header("Gun Settings")]
    public GameObject[] guns;
    public AudioSource changeSound;

    [Header("Bomb Settings")]
    public GameObject bomb;
    public GameObject bombPoint;
    public Camera bombCam;

    [Header("Health Settings")]
    float health = 100;
    public Image healthBar;

    [Header("Enemy Settings")]
    public GameObject[] enemies;
    public GameObject[] spawnPoints;
    public GameObject[] targets;
    public TextMeshProUGUI enemyCountText;
    public int totalEnemyCount;
    public float enemeySpawnTime;
    int remainingEnemy;
    int spawnedEnemyCount;

    [Header("Others")]
    public GameObject GameOverCanvas;
    public GameObject WinCanvas;
    public GameObject PauseCanvas;
    public AudioSource GameMusic;
    public TextMeshProUGUI medboxText;
    public TextMeshProUGUI bombText;
    public static bool isPaused = false;

    void Start()
    {
        isPaused = false;
        if (!PlayerPrefs.HasKey("GameStarted"))
        {
            PlayerPrefs.SetInt("GameStarted", 1);

            PlayerPrefs.SetInt("Rifle_Ammo", 990);
            PlayerPrefs.SetInt("Remaining_Rifle_Ammo", 0);

            PlayerPrefs.SetInt("Sniper_Ammo", 936);
            PlayerPrefs.SetInt("Remaining_Sniper_Ammo", 0);

            PlayerPrefs.SetInt("Magnum_Ammo", 927);
            PlayerPrefs.SetInt("Remaining_Magnum_Ammo", 0);

            PlayerPrefs.SetInt("Shotgun_Ammo", 918);
            PlayerPrefs.SetInt("Remaining_Shotgun_Ammo", 0);

            PlayerPrefs.SetInt("MedBox", 1);
            PlayerPrefs.SetInt("Grenade", 5);


        }
        enemyCountText.text = totalEnemyCount.ToString();
        remainingEnemy = totalEnemyCount;
        spawnedEnemyCount = totalEnemyCount;
        medboxText.text = PlayerPrefs.GetInt("MedBox").ToString();
        bombText.text = PlayerPrefs.GetInt("Grenade").ToString();

        GameMusic.Play();
        
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isPaused)
        {
            Change(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !isPaused)
        {
            Change(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && !isPaused)
        {
            Change(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && !isPaused)
        {
            Change(3);
        }
        if (Input.GetKeyDown(KeyCode.G) && !isPaused)
        {
            UseGrenade();          
        }
        if (Input.GetKeyDown(KeyCode.T) && !isPaused)
        {
            UseMedBox();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            Pause();
        }
    }

    void Change(int index)
    {
        for (int i = 0; i < guns.Length; i++)
        {
            if (i != index)
            {
                guns[i].SetActive(false);
            }
            else
            {
                guns[i].SetActive(true);
                if (i == 0)
                {
                    guns[i].GetComponent<AK47>().WriteBulletCounts();
                }
                else if (i == 1)
                {
                    guns[i].GetComponent<Magnum>().WriteBulletCounts();
                }
                else if (i == 2)
                {
                    guns[i].GetComponent<Shotgun>().WriteBulletCounts();
                }
                else
                {
                    guns[i].GetComponent<Sniper>().WriteBulletCounts();
                }
            }

        }
        changeSound.Play();
    }
    //-------------------MedBox Ops-------------------------
    public void UseMedBox()
    {
        if (PlayerPrefs.GetInt("MedBox") != 0 && health < 100)
        {
            RefillHealth();
            PlayerPrefs.SetInt("MedBox", PlayerPrefs.GetInt("MedBox") - 1);
            medboxText.text = PlayerPrefs.GetInt("MedBox").ToString();
        }
    }
    public void TakeMedBox()
    {
        PlayerPrefs.SetInt("MedBox", PlayerPrefs.GetInt("MedBox") + 1);
        medboxText.text = PlayerPrefs.GetInt("MedBox").ToString();
    }
    public void RefillHealth()
    {
        health = 100;
        healthBar.fillAmount = health / 100;
    }
    //--------------------Bomb Ops---------------------------
    public void TakeGrenadeBox()
    {
        PlayerPrefs.SetInt("Grenade", PlayerPrefs.GetInt("Grenade") + 1);
        bombText.text = PlayerPrefs.GetInt("Grenade").ToString();
    }
    public void UseGrenade()
    {
        if (PlayerPrefs.GetInt("Grenade") > 0)
        {
            GameObject bombObj = Instantiate(bomb, bombPoint.transform.position, bombPoint.transform.rotation);
            Rigidbody rg = bombObj.GetComponent<Rigidbody>();
            Vector3 angle = Quaternion.AngleAxis(90, bombCam.transform.forward) * bombCam.transform.forward;
            rg.AddForce(angle * 250f);
            PlayerPrefs.SetInt("Grenade", PlayerPrefs.GetInt("Grenade") - 1);
            bombText.text = PlayerPrefs.GetInt("Grenade").ToString();
        }
        
    }

    //---------------Enemy Ops-------------------------------
    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / 100;
        //Debug.Log(healthBar.fillAmount);
        if (health <= 0)
        {
            Debug.Log("Game Over");
            GameOver();
        }
    }
    public void UpdateEnemyCount()
    {
        remainingEnemy--;
        enemyCountText.text = remainingEnemy.ToString();
        if (remainingEnemy <= 0)
        {
            WinCanvas.SetActive(true);
            Time.timeScale = 0;
        }
        
    }
    IEnumerator SpawnEnemy()
    {
        
        while (true)
        {
            yield return new WaitForSeconds(enemeySpawnTime);
            if (spawnedEnemyCount > 0)
            {
                Debug.Log("içerdema");
                int enemy = Random.Range(0, 5);
                int spawnPoint = Random.Range(0, 2);
                int target = Random.Range(0, 2);

                GameObject spawnedEnemy = Instantiate(enemies[enemy], spawnPoints[spawnPoint].transform.position, Quaternion.identity);
                spawnedEnemy.GetComponent<Enemy>().target = targets[target];
                spawnedEnemyCount--;
            }
           
        }
    }
  
    //------------------------Menu Operations----------------------------------
    //------------------------------------------------------------------------
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        isPaused = false;
        Cursor.visible = false;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void GameOver()
    {
        GameOverCanvas.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Pause()
    {
        PauseCanvas.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Continue()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }
    public void Exit()
    {
        SceneManager.LoadScene(0);
        isPaused = false;
        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
    }
    //----------------------------------------------
    //----------------------------------------------
}
