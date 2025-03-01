using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManeger : MonoBehaviour
{
    public GameObject PlayerActive;
    public GameObject enemyTarget;
    public GameObject[] players;
    public bool playerEndTurn;
    public GameObject[] enemies;
    public bool enemyEndTurn;
    private bool enemyTurnActive = false;

    private void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {
        if (!enemyTurnActive)
        {
            CheckPlayerTurn();
        }
    }

    public void CheckPlayerTurn()
    {
        int count = 0;
        foreach (GameObject player in players)
        {
            if (player != null && player.GetComponent<PlayerController>().playerEndTurn)
            {
                count++;
            }
        }

        if (count == players.Length && !playerEndTurn)
        {
            playerEndTurn = true;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        enemyTurnActive = true;

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if (!enemyController.enemyEndTurn)
                {
                    yield return new WaitForSeconds(1.5f); // Pausa entre ataques
                    enemyController.EnemyAtk();
                }
            }
        }

        yield return new WaitForSeconds(1f); // Breve pausa antes de ceder el turno

        // Resetear turnos
        enemyEndTurn = true;
        playerEndTurn = false;
        enemyTurnActive = false;
        ResetTurns();
    }

    void ResetTurns()
    {
        foreach (GameObject player in players)
        {
            if (player != null)
            {
                player.GetComponent<PlayerController>().playerEndTurn = false;
            }
        }

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<EnemyController>().enemyEndTurn = false;
            }
        }
    }

    public void PlayerSelect(GameObject playerSelect)
    {
        PlayerActive = playerSelect;
    }

    public void PlayerDeSelect()
    {
        if (PlayerActive != null)
        {
            PlayerActive.GetComponent<PlayerController>().PlayerDeSelect();
            PlayerActive = null;
        }
    }

    public void EnemySelect(GameObject enemySelect)
    {
        enemyTarget = enemySelect;
    }

    public void EnemyDeSelect()
    {
        if (enemyTarget != null)
        {
            enemyTarget.GetComponent<EnemyController>().EnemyDeSelect();
            enemyTarget = null;
        }
    }

    public void CheckGameOver()
    {
        players = Array.FindAll(players, player => player != null);
        enemies = Array.FindAll(enemies, enemy => enemy != null);

        if (players.Length == 0)
        {
            Debug.Log("Game Over");
        }

        if (enemies.Length == 0)
        {
            Debug.Log("Winner");
        }
    }

}
