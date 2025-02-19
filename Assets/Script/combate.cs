using UnityEngine;
using UnityEngine.UI;
public class combate: MonoBehaviour
{
    public GameObject mainPanel; // Panel con las opciones iniciales.
    public GameObject beastPanel; // Panel de habilidades de la bestia.

    private Character player;
    private Character enemy;
    private bool playerTurn = true;
    private bool beastMode = false;
    private System.Random random = new System.Random(); // Generador de números aleatorios

    void Start()
    {
        player = new Character("Hero", 100, 20);
        enemy = new Character("Goblin", 80, 15);
        Debug.Log("¡Combate iniciado! Elige tu acción.");
        ShowMainPanel();
    }

    public void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        beastPanel.SetActive(false);
    }

    public void ShowBeastPanel()
    {
        mainPanel.SetActive(false);
        beastPanel.SetActive(true);
    }

    public void PlayerAction(int action)
    {
        if (!playerTurn) return;

        if (beastMode)
        {
            switch (action)
            {
                case 1:
                    RoarAttack(); // Rugido Feroz
                    break;
                case 2:
                    ClawStrike(); // Zarpazo Mortal
                    break;
                case 3:
                    beastMode = false; // Salir del modo bestia
                    ShowMainPanel();   // Volver al menú principal
                    return;
            }
        }
        else
        {
            switch (action)
            {
                case 1:
                    Attack(); // Ataque normal
                    break;
                case 2:
                    beastMode = true; // Entrar al modo bestia
                    ShowBeastPanel(); // Mostrar el menú de ataques de la bestia
                    return; // Salir sin cambiar el turno aún
            }
        }

        // Solo cambiamos el turno del jugador si hemos completado la acción
        playerTurn = false;
        CheckCombatState();

        if (!playerTurn)
        {
            EnemyTurn(); // El turno pasa al enemigo
        }
    }

    void Attack()
    {
        int randomDamage = random.Next(1, 11); // Daño aleatorio entre 1 y 10
        enemy.Health -= randomDamage;
        Debug.Log($"Has atacado a {enemy.Name} causando {randomDamage} de daño.");
    }

    void RoarAttack()
    {
        int randomDamage = random.Next(1, 11);
        enemy.Health -= randomDamage;
        Debug.Log($"Usaste Rugido en {enemy.Name} causando {randomDamage} de daño.");
    }

    void ClawStrike()
    {
        int randomDamage = random.Next(1, 11);
        enemy.Health -= randomDamage;
        Debug.Log($"Usaste corte en {enemy.Name} causando {randomDamage} de daño.");
    }

    void EnemyTurn()
    {
        int randomDamage = random.Next(1, 11);
        player.Health -= randomDamage;

        if (beastMode)
        {
            Debug.Log($"{enemy.Name} ataca a tu bestia causando {randomDamage} de daño.");
        }
        else
        {
            Debug.Log($"{enemy.Name} te ataca causando {randomDamage} de daño.");
        }

        beastMode = false; // Salir del modo bestia
        playerTurn = true;
        CheckCombatState();
        ShowMainPanel();
    }

    void CheckCombatState()
    {
        if (player.Health <= 0)
        {
            Debug.Log("Has sido derrotado...");
            mainPanel.SetActive(false);
            beastPanel.SetActive(false);
        }
        else if (enemy.Health <= 0)
        {
            Debug.Log("¡Has ganado el combate!");
            mainPanel.SetActive(false);
            beastPanel.SetActive(false);
        }
    }
}

public class Character
{
    public string Name;
    public int Health;
    public int AttackPower;

    public Character(string name, int health, int attackPower)
    {
        Name = name;
        Health = health;
        AttackPower = attackPower;
    }
}


