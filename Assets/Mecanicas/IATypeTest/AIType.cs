using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIType : MonoBehaviour
{
    public CharacterInfo enemyCharacter;
    public float moveSpeed = 3f;

    private bool turnActive = false;
    private bool isMoving = false;

    public int expReward = 100;

    private void Start()
    {
        if (enemyCharacter == null)
            enemyCharacter = GetComponent<CharacterInfo>();
    }

    private void Update()
    {
        if (isMoving && !TurnManager.Instance.playerTurn)
        {
            FindAnyObjectByType<Camera>().transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }

    public void TakeTurn()
    {
        if (!turnActive)
        {
            turnActive = true;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        // Opción de huir si la salud es extremadamente baja (por ejemplo, <15% del máximo)
        /*if (enemyCharacter.currentHP < enemyCharacter.maxHP * 0.15f)
        {
            Debug.Log(enemyCharacter.characterName + " decide huir.");
            // Aquí podrías implementar lógica de huida (por ejemplo, moverse hacia un tile seguro)
            EndTurn();
            yield break;
        }*/

        if (enemyCharacter.currentHP < enemyCharacter.maxHP * 0.3f)
        {
            Debug.Log(enemyCharacter.characterName + " heals itself.");
            enemyCharacter.Heal(10);
            EndTurn();
            yield break;
        }

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        if (playerObjects.Length == 0)
        {
            Debug.Log("No player found. End turn.");
            EndTurn();
            yield break;
        }

        // Find player target: nearer and with lower health
        CharacterInfo targetCharacter = null;
        int bestDistance = int.MaxValue;
        int bestHP = int.MaxValue;
        foreach (GameObject playerObj in playerObjects)
        {
            CharacterInfo ci = playerObj.GetComponent<CharacterInfo>();
            int dist = GetManhattanDistance(enemyCharacter.activeTile, ci.activeTile);
            if (dist < bestDistance || (dist == bestDistance && ci.currentHP < bestHP))
            {
                bestDistance = dist;
                bestHP = ci.currentHP;
                targetCharacter = ci;
            }
        }

        if (targetCharacter == null)
        {
            Debug.Log("No valid target. End turn.");
            EndTurn();
            yield break;
        }

        int chosenAttackIndex = ChooseBestAttack(targetCharacter);
        Attacks chosenAttack = enemyCharacter.attacks[chosenAttackIndex];
        int attackRange = chosenAttack.range;

        // Calculate path
        Pathfinder pathfinder = new Pathfinder();
        List<OverlayTile> path = pathfinder.FindPath(enemyCharacter.activeTile, targetCharacter.activeTile, MapManager.Instance.map.Values.ToList());
        if (path == null || path.Count == 0)
        {
            Debug.Log("No path found for the target. End turn.");
            EndTurn();
            yield break;
        }

        int tilesMoved = 0;

        // Moves until range limit
        while (GetManhattanDistance(enemyCharacter.activeTile, targetCharacter.activeTile) > attackRange && tilesMoved < enemyCharacter.range)
        {
            isMoving = true;
            yield return MoveAlongPath(path);
            tilesMoved++;

            // Recalculate path in case of changes
            path = pathfinder.FindPath(enemyCharacter.activeTile, targetCharacter.activeTile, MapManager.Instance.map.Values.ToList());
            if (path == null || path.Count == 0)
            {
                Debug.Log("Path lost. End turn.");
                EndTurn();
                yield break;
            }
        }

        enemyCharacter.canAct = true;
        enemyCharacter.activeAtk = chosenAttack.attackName;

        // If in range, attack
        if (GetManhattanDistance(enemyCharacter.activeTile, targetCharacter.activeTile) <= enemyCharacter.attacks[ChooseBestAttack(targetCharacter)].range)
        {
            Debug.Log(enemyCharacter.characterName + " attacks " + targetCharacter.characterName + " using " + enemyCharacter.attacks[ChooseBestAttack(targetCharacter)].attackName);
            enemyCharacter.activeAtk = enemyCharacter.attacks[ChooseBestAttack(targetCharacter)].attackName;
            enemyCharacter.PerformAttack(ChooseBestAttack(targetCharacter), targetCharacter);
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            Debug.Log(enemyCharacter.characterName + " couldn't get near enough to attack.");
        }
        EndTurn();
    }

    int GetManhattanDistance(OverlayTile tileA, OverlayTile tileB)
    {
        return Mathf.Abs(tileA.gridLocation.x - tileB.gridLocation.x) + Mathf.Abs(tileA.gridLocation.y - tileB.gridLocation.y);
    }

    int ChooseBestAttack(CharacterInfo target)
    {
        int distance = GetManhattanDistance(enemyCharacter.activeTile, target.activeTile);
        int bestIndex = 0;
        int maxDamage = 0;
        for (int i = 0; i < enemyCharacter.attacks.Count; i++)
        {
            Attacks atk = enemyCharacter.attacks[i];
            if (atk.range >= distance)
            {
                if (atk.damage > maxDamage)
                {
                    maxDamage = atk.damage;
                    bestIndex = i;
                }
            }
        }
        return bestIndex;
    }

    IEnumerator MoveAlongPath(List<OverlayTile> path)
    {
        if (path.Count == 0)
        {
            isMoving = false;
            yield break;
        }

        OverlayTile nextTile = path[0];
        while (Vector2.Distance(enemyCharacter.transform.position, nextTile.transform.position) > 0.05f)
        {
            enemyCharacter.transform.position = Vector2.MoveTowards(enemyCharacter.transform.position, nextTile.transform.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        enemyCharacter.activeTile = nextTile;
        path.RemoveAt(0);
        yield return null;
    }

    void EndTurn()
    {
        enemyCharacter.canMove = false;
        enemyCharacter.canAct = false;
        Debug.Log(enemyCharacter.characterName + " ends its turn.");
        turnActive = false;
        isMoving = false;
    }
}
