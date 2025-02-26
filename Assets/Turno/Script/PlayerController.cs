using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public bool playerEndTurn;
    public GameObject markerSelect;
    public GameObject action;
    public BattleManeger sbm;

    private void Awake()
    {
        sbm = FindAnyObjectByType<BattleManeger>();

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
            Debug.Log("Atacando al " + sbm.enemyTarget.name);
            playerEndTurn = true;
        }
    }
}
