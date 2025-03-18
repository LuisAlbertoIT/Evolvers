using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public CharacterInfo enemyCharacter;  // Componente CharacterInfo del enemigo
    public float moveSpeed = 3f;            // Velocidad de movimiento del enemigo

    private bool turnActive = false;       // Indica si la IA está en turno

    private void Start()
    {
        if (enemyCharacter == null)
            enemyCharacter = GetComponent<CharacterInfo>();
    }

    // Llamar a este método para iniciar el turno del enemigo.
    public void TakeTurn()
    {
        if (!turnActive)
        {
            turnActive = true;
            StartCoroutine(EnemyTurn());
        }
    }

    // Coroutine que gestiona el comportamiento durante el turno del enemigo.
    IEnumerator EnemyTurn()
    {
        // Simulación del proceso de decisión
        yield return new WaitForSeconds(1f);

        // Opción de huir si la salud es extremadamente baja (por ejemplo, <15% del máximo)
        /*if (enemyCharacter.currentHP < enemyCharacter.maxHP * 0.15f)
        {
            Debug.Log(enemyCharacter.characterName + " decide huir.");
            // Aquí podrías implementar lógica de huida (por ejemplo, moverse hacia un tile seguro)
            EndTurn();
            yield break;
        }*/

        // Si la salud es baja (<30% del máximo), prioriza curarse
        if (enemyCharacter.currentHP < enemyCharacter.maxHP * 0.3f)
        {
            Debug.Log(enemyCharacter.characterName + " decide curarse.");
            enemyCharacter.Heal(10);  // Valor fijo, ajustable según necesidades
            EndTurn();
            yield break;
        }

        // Buscar objetivos: se asume que los personajes controlados por el jugador tienen la etiqueta "Player"
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        if (playerObjects.Length == 0)
        {
            Debug.Log("No se encontró ningún jugador. Fin del turno.");
            EndTurn();
            yield break;
        }

        // Buscar el objetivo entre los jugadores: se selecciona el más cercano (y en empate, el de menor vida).
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
            Debug.Log("No se encontró objetivo válido. Fin del turno.");
            EndTurn();
            yield break;
        }

        // Elegir el mejor ataque: obtenemos el índice y su rango.
        int chosenAttackIndex = ChooseBestAttack(targetCharacter);
        Attacks chosenAttack = enemyCharacter.attacks[chosenAttackIndex];
        int attackRange = chosenAttack.range;

        // Calcular el camino desde el tile activo del enemigo hasta el tile activo del objetivo.
        Pathfinder pathfinder = new Pathfinder();
        List<OverlayTile> path = pathfinder.FindPath(enemyCharacter.activeTile, targetCharacter.activeTile, MapManager.Instance.map.Values.ToList());
        if (path == null || path.Count == 0)
        {
            Debug.Log("No se encontró camino hacia el objetivo. Fin del turno.");
            EndTurn();
            yield break;
        }

        int tilesMoved = 0;

        // Mientras el enemigo no esté en rango y no haya superado su límite de movimiento:
        while (GetManhattanDistance(enemyCharacter.activeTile, targetCharacter.activeTile) > attackRange && tilesMoved < enemyCharacter.range)
        {
            yield return MoveAlongPath(path);
            tilesMoved++;

            // Recalcular el camino en caso de cambios en el entorno
            path = pathfinder.FindPath(enemyCharacter.activeTile, targetCharacter.activeTile, MapManager.Instance.map.Values.ToList());
            if (path == null || path.Count == 0)
            {
                Debug.Log("Se perdió el camino durante el movimiento. Fin del turno.");
                EndTurn();
                yield break;
            }
        }

        // Antes de atacar, se permite la acción y se actualiza el ataque activo.
        enemyCharacter.canAct = true;
        enemyCharacter.activeAtk = chosenAttack.attackName;

        // Si tras moverse (o al alcanzar el límite) el enemigo está en rango, ataca.
        if (GetManhattanDistance(enemyCharacter.activeTile, targetCharacter.activeTile) <= enemyCharacter.attacks[ChooseBestAttack(targetCharacter)].range)
        {
            Debug.Log(enemyCharacter.characterName + " ataca a " + targetCharacter.characterName + " usando " + enemyCharacter.attacks[ChooseBestAttack(targetCharacter)].attackName);
            enemyCharacter.activeAtk = enemyCharacter.attacks[ChooseBestAttack(targetCharacter)].attackName;
            enemyCharacter.PerformAttack(ChooseBestAttack(targetCharacter), targetCharacter);
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            Debug.Log(enemyCharacter.characterName + " no logró acercarse lo suficiente para atacar.");
        }
        EndTurn();
    }

    // Método que evalúa si hay al menos un ataque con rango suficiente para la distancia actual.
    bool IsAnyAttackInRange(int distance)
    {
        foreach (Attacks atk in enemyCharacter.attacks)
        {
            if (atk.range >= distance)
                return true;
        }
        return false;
    }

    // Calcula la distancia Manhattan entre dos tiles.
    int GetManhattanDistance(OverlayTile tileA, OverlayTile tileB)
    {
        return Mathf.Abs(tileA.gridLocation.x - tileB.gridLocation.x) + Mathf.Abs(tileA.gridLocation.y - tileB.gridLocation.y);
    }

    // Método para elegir el mejor ataque basado en el rango y el daño.
    int ChooseBestAttack(CharacterInfo target)
    {
        int distance = GetManhattanDistance(enemyCharacter.activeTile, target.activeTile);
        int bestIndex = 0;
        int maxDamage = 0;
        for (int i = 0; i < enemyCharacter.attacks.Count; i++)
        {
            Attacks atk = enemyCharacter.attacks[i];
            // Solo considerar ataques que puedan alcanzar el objetivo
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

    // Coroutine para mover al enemigo a lo largo del camino calculado.
    IEnumerator MoveAlongPath(List<OverlayTile> path)
    {
        if (path.Count == 0)
            yield break;

        OverlayTile nextTile = path[0];
        // Movimiento suave hacia el siguiente tile.
        while (Vector2.Distance(enemyCharacter.transform.position, nextTile.transform.position) > 0.05f)
        {
            enemyCharacter.transform.position = Vector2.MoveTowards(enemyCharacter.transform.position, nextTile.transform.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        // Actualizar el tile activo y removerlo del camino.
        enemyCharacter.activeTile = nextTile;
        path.RemoveAt(0);
        yield return null;
    }

    // Finaliza el turno del enemigo.
    void EndTurn()
    {
        enemyCharacter.canMove = false;
        enemyCharacter.canAct = false;
        Debug.Log(enemyCharacter.characterName + " finaliza su turno.");
        turnActive = false;
    }
}
