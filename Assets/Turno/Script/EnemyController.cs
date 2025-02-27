using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject markerSelect;
    public BattleManeger sbm;
    public bool enemyEndTurn;
    public GameObject playerTarget;

    private void Awake()
    {
        sbm = FindAnyObjectByType<BattleManeger>();

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
            playerTarget = sbm.players[Random.Range(0, sbm.players.Length)];
            Debug.Log("El enemigo " + gameObject.name + " ataca a " + playerTarget.name);
            enemyEndTurn = true;
        }
    }
}
