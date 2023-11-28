using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerHealth : MonoBehaviour

{
    public GameObject player;
    public GameObject enemy;
    public int maxHealth = 5;
    public int health;
    public int damage = 1;
    float attackCooldown = 2f;
    private float nextAttack;
    // Start is called before the first frame update
    private void Awake()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, player.transform.position);
        if (distanceToPlayer < 5 && Time.time > nextAttack)
        {
            nextAttack = Time.time + attackCooldown;
            PlayerTakeDamage();
        }

        //if player dies, the scene reloads
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void PlayerTakeDamage()
    {
        health = health - damage;
    }
}
