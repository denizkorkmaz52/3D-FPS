using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    public float damage;
    [Header("Health Settings")]
    public GameObject target;
    float health = 100;
    bool isDead = false;
    GameObject gameControl;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        gameControl = GameObject.FindWithTag("GameController");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.transform.position);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameObject.tag = "Untagged";
            Died();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            
            gameControl.GetComponent<GameController>().TakeDamage(damage);
            Died();
        }
    }
    public void Died()
    {
        if (!isDead)
        {
            if (health <= 0)
            {
                gameControl.GetComponent<GameController>().UpdateEnemyCount();
                animator.SetTrigger("dead");
                Destroy(gameObject, 3f);
                isDead = true;
            }
            else
            {
                gameControl.GetComponent<GameController>().UpdateEnemyCount();
                Destroy(gameObject);
                isDead = true;
            }

            
        }
        
    }
}
