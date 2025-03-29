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
    public GameObject CederTruno;

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
        else if (playerEndTurn && sbm.PlayerActive != gameObject && sbm.PlayerActive != null)
        {
            if (sbm.playerTarget != gameObject && sbm.playerTarget != null)
            {
                sbm.PlayerToPlayerDeSelect();
            }
            PlayerToPlayerSelect();
            sbm.PlayerToPlayerSelect(gameObject);
        }
    }
    public void PlayerToPlayerSelect()
    {
        markerSelect.SetActive(true);
        if (sbm.PlayerActive != null)
        {
            sbm.PlayerActive.GetComponent<PlayerController>().CederTruno.SetActive(true);
        }
    }

    public void CedeTurno()
    {
        if (sbm.playerTarget != null)
        {
            print("Cede Turno al " + sbm.playerTarget.name);
            sbm.playerTarget.GetComponent<PlayerController>().playerEndTurn = true; // El jugador seleccionado puede actuar
            playerEndTurn = true; // Este jugador termina su turno
            CederTruno.SetActive(false);
            markerSelect.SetActive(false);
            sbm.playerTarget.GetComponent<PlayerController>().playerEndTurn = false;
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
            sbm.enemyTarget.GetComponent<EnemyController>().TakeDamage(20); // Ejemplo de da�o
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
