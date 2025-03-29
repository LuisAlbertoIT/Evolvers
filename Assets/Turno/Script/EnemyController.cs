using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyController : MonoBehaviour
{
    public GameObject markerSelect;
    public BattleManeger sbm;
    public bool enemyEndTurn;
    public GameObject playerTarget;

    public Image healthBar;
    private int maxHealth = 100;
    public float currentHealth;

    [Obsolete]
    private void Awake()
    {
        sbm = FindObjectOfType<BattleManeger>();
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void OnMouseDown()
    {
        if (sbm.enemyTarget != gameObject && sbm.enemyTarget != null)
        {
            sbm.EnemyDeSelect();
        }
        EnemySelect();
        sbm.EnemySelect(gameObject);
    }

    public void EnemySelect()
    {
        markerSelect.SetActive(true);

        if (sbm.PlayerActive != null)
        {
            sbm.PlayerActive.GetComponent<PlayerController>().action.SetActive(true);
        }
    }

    public void EnemyDeSelect()
    {
        markerSelect.SetActive(false);
    }

    public void EnemyAtk()
    {
        if (!enemyEndTurn)
        {
            playerTarget = sbm.players[UnityEngine.Random.Range(0, sbm.players.Length)];
            if (playerTarget != null)
            {
                playerTarget.GetComponent<PlayerController>().TakeDamage(15); // Ejemplo de daño
                print("El enemigo " + gameObject.name + " ataca a " + playerTarget.name);
                enemyEndTurn = true;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    void Die()
    {
        sbm.enemies = Array.FindAll(sbm.enemies, enemy => enemy != gameObject);
        Destroy(gameObject);
        sbm.CheckGameOver();
    }
}
