using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public bool playerEndTurn;
    public GameObject markerSelect;
    public GameObject action;
    public BattleManeger sbm;

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
        if (!playerEndTurn)
        {
            if (sbm.PlayerActive != gameObject && sbm.PlayerActive != null)
            {
                sbm.PlayerDeSelect();
            }
            PlayerSelect();
            sbm.PlayerSelect(gameObject);
        }
    }

    public void PlayerSelect()
    {
        markerSelect.SetActive(true);
    }

    public void PlayerDeSelect()
    {
        markerSelect.SetActive(false);
        action.SetActive(false);
        if (sbm.enemyTarget != null)
        {
            sbm.enemyTarget.GetComponent<EnemyController>().EnemyDeSelect();
        }
    }

    public void ActionButtom()
    {
        if (sbm.enemyTarget != null)
        {
            print("Atacando al " + sbm.enemyTarget.name);
            sbm.enemyTarget.GetComponent<EnemyController>().TakeDamage(20); // Ejemplo de daño
            playerEndTurn = true;
            action.SetActive(false);
            markerSelect.SetActive(false);
            sbm.enemyTarget.GetComponent<EnemyController>().markerSelect.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
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
        sbm.players = Array.FindAll(sbm.players, player => player != gameObject);
        Destroy(gameObject);
        sbm.CheckGameOver();
    }
}
