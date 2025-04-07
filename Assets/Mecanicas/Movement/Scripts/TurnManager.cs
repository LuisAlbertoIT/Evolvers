
﻿using NUnit.Framework;

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TurnManager : MonoBehaviour
{
    private static TurnManager _instance;
    public static TurnManager Instance { get { return _instance; } }

    public List<CharacterInfo> characters = new List<CharacterInfo>();
    public List<CharacterInfo> enemies = new List<CharacterInfo>();

    public int activeCharacter = 0;
    public int activeEnemy = 0;
    public int turn = 0;
    public bool playerTurn = true;

    public Button attackBtn;
    public GameObject turnPanel;
    public GameObject attackPanel;
    private Button btn;
    public List<Button> buttons;
    private bool gameOver = false;
    public GameObject ZoneSpawner;

    public GameObject inventoryPanel;
    public List<Image> inventorySlots = new List<Image>(); // Asignar en Inspector
    private bool inventoryOpen = false;

    private CharacterInfo currentCharacter;

    public GameObject selector;
    public int selectedSlotIndex = 0;
    public int inventoryState = 0; // 0 = navegando, 1 = opciones
    public GameObject opcionesPanel;
    public UnityEngine.UI.Image[] seleccionOpciones;
    public Sprite[] seleccionSprites; // [0]=normal, [1]=seleccionado
    public int opcionSeleccionada = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Battle starts!");
        foreach (var character in FindObjectsByType<CharacterInfo>(FindObjectsSortMode.None))
        {
            if(character.CompareTag("Player"))
                characters.Add(character);
            else
                enemies.Add(character);
        }
        currentCharacter = characters[activeCharacter]; // ✅ Agregado para evitar NullReference

        if (Input.GetKeyDown(KeyCode.KeypadEnter) && playerTurn)
        {
            OpenInventory();
        }
        //characters.AddRange(FindObjectsByType<CharacterInfo>(FindObjectsSortMode.None));

        Debug.Log("INVENTARIO EN ESCENA DE COMBATE:");
        for (int i = 0; i < currentCharacter.inventory.Length; i++)
        {
            var item = currentCharacter.inventory[i];
            if (item != null) ;
                
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckGameOver();

        if (inventoryOpen)
        {
            switch (inventoryState)
            {
                case 0: // 🔹 Navegando por inventario
                    if (Input.GetKeyDown(KeyCode.D))
                        selectedSlotIndex = Mathf.Min(selectedSlotIndex + 1, inventorySlots.Count - 1);
                    if (Input.GetKeyDown(KeyCode.A))
                        selectedSlotIndex = Mathf.Max(selectedSlotIndex - 1, 0);
                    if (Input.GetKeyDown(KeyCode.W))
                        selectedSlotIndex = Mathf.Max(selectedSlotIndex - 4, 0);
                    if (Input.GetKeyDown(KeyCode.S))
                        selectedSlotIndex = Mathf.Min(selectedSlotIndex + 4, inventorySlots.Count - 1);

                    selector.transform.position = inventorySlots[selectedSlotIndex].transform.position;

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (
                            selectedSlotIndex >= 0 &&
                            selectedSlotIndex < inventorySlots.Count &&
                            currentCharacter.inventory[selectedSlotIndex] != null &&
                            inventorySlots[selectedSlotIndex].enabled)
                        {
                            Debug.Log("Item válido detectado. Mostrando opciones...");
                            inventoryState = 1;
                            opcionesPanel.SetActive(true);
                            opcionSeleccionada = 0;
                            opcionesPanel.transform.position = inventorySlots[selectedSlotIndex].transform.position;
                        }
                        else
                        {
                            Debug.Log("No hay ítem visible en el slot seleccionado.");
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.I))
                    {
                        CloseInventory();
                    }
                    break;

                case 1: // 🔸 Selección de acción (usar / eliminar)
                    if (Input.GetKeyDown(KeyCode.W))
                        opcionSeleccionada = Mathf.Max(opcionSeleccionada - 1, 0);
                    if (Input.GetKeyDown(KeyCode.S))
                        opcionSeleccionada = Mathf.Min(opcionSeleccionada + 1, seleccionOpciones.Length - 1);

                    for (int i = 0; i < seleccionOpciones.Length; i++)
                    {
                        seleccionOpciones[i].sprite = (i == opcionSeleccionada) ? seleccionSprites[1] : seleccionSprites[0];
                    }

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (opcionSeleccionada == 0)
                        {
                            UseItem(selectedSlotIndex); // Ya definido en tu clase
                        }
                        else if (opcionSeleccionada == 1)
                        {
                            currentCharacter.inventory[selectedSlotIndex] = null;
                            UpdateInventoryVisuals();
                            currentCharacter.canAct = false;
                        }

                        opcionesPanel.SetActive(false);
                        inventoryState = 0;
                        CloseInventory();
                    }

                    if (Input.GetKeyDown(KeyCode.G)) // Cancelar
                    {
                        opcionesPanel.SetActive(false);
                        inventoryState = 0;
                    }
                    break;
            }

            return; // 🔁 Cortar el resto del update mientras estás en inventario
        }

        if (!gameOver)
        {
            if (turn == 0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartCombat();
                }
            }

            if (playerTurn)
            {
                if (!MouseController.Instance.isMoving)
                {
                    if (Input.GetKeyDown(KeyCode.D))
                        NextCharacter();
                    if (Input.GetKeyDown(KeyCode.A))
                        PreviousCharacter();

                    if (Input.GetKeyDown(KeyCode.I))
                    {
                        if (inventoryOpen)
                            CloseInventory();
                        else
                            OpenInventory();
                    }
                }

                if (!characters[activeCharacter].canMove && !characters[activeCharacter].canAct)
                {
                    bool allDone = true;
                    foreach (CharacterInfo character in characters)
                    {
                        if (character.canMove || character.canAct)
                        {
                            allDone = false;
                            break;
                        }
                    }
                    if (allDone)
                    {
                        EnemyTurn();
                    }
                    else
                        NextCharacter();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<SceneChanger>().CargarEscena("Base");
        }
    }

    private void EnemyTurn()
    {
        playerTurn = false;
        turnPanel.SetActive(false);
        attackPanel.SetActive(false);
        Debug.Log("Enemy Turn");
        foreach (CharacterInfo enemy in enemies)
        {
            enemy.canMove = true;
            enemy.canAct = true;
        }
        activeEnemy = 0;

        StartCoroutine(EnemyTurnRoutine());
    }

    IEnumerator EnemyTurnRoutine()
    {
        foreach (CharacterInfo enemy in enemies)
        {
            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null)
            {
                FindAnyObjectByType<Camera>().transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, -10);
                ai.TakeTurn();
                while (enemy.canMove || enemy.canAct)
                {
                    yield return null;
                }
            }
        }

        bool allEnemiesDone = false;
        while (!allEnemiesDone)
        {
            allEnemiesDone = true;
            foreach (CharacterInfo enemy in enemies)
            {
                if (enemy.canMove || enemy.canAct)
                {
                    allEnemiesDone = false;
                    break;
                }
            }
            yield return null;
        }
        if(!gameOver)
            StartCombat();
    }

    private void StartCombat()
    {
        turnPanel.SetActive(true);
        attackPanel.SetActive(false);
        playerTurn = true;
        turn++;
        Debug.Log("Turn" + turn);

        foreach (CharacterInfo character in characters)
        {
            MouseController.Instance.SetCharacter(character);
            MouseController.Instance.PositionCharacterOnTile(character, character.activeTile);
            character.canMove = true;
            character.canAct = false;
        }
        activeCharacter = 0;

        UpdateUI();
    }

    private void NextCharacter()
    {
        ClearAttackMenu();

        int initialIndex = activeCharacter;

        // Recorrer todos los personajes para buscar uno que pueda moverse o actuar
        do
        {
            activeCharacter = (activeCharacter + 1) % characters.Count;
            // Si encontramos uno que pueda moverse o actuar, salimos del bucle.
            if (characters[activeCharacter].canMove || characters[activeCharacter].canAct)
            {
                break;
            }
        }
        while (activeCharacter != initialIndex);

        UpdateUI();
    }

    private void PreviousCharacter()
    {
        ClearAttackMenu();

        int initialIndex = activeCharacter;

        // Recorrer todos los personajes para buscar uno que pueda moverse o actuar
        do
        {
            activeCharacter = (activeCharacter - 1 + characters.Count) % characters.Count;
            // Si encontramos uno que pueda moverse o actuar, salimos del bucle.
            if (characters[activeCharacter].canMove || characters[activeCharacter].canAct)
            {
                break;
            }
        }
        while (activeCharacter != initialIndex);

        UpdateUI();
    }

    public void Skip()
    {
        if (characters[activeCharacter].canMove)
        {
            characters[activeCharacter].canMove = false;
            characters[activeCharacter].canAct = true;
            MouseController.Instance.GetInAttackRangeTiles(characters[activeCharacter].activeTile);
            MouseController.Instance.HideRangeTiles();
        }
        else
        {
            MouseController.Instance.HideAttackRangeTiles();
            characters[activeCharacter].canAct = false;
        }
        
    }

    void Back()
    {
        attackPanel.SetActive(false);
        turnPanel.SetActive(true);
        Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text);
        for(int i = 0; i< buttons.Count; i++)
        {
            if (EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text == buttons[i].GetComponentInChildren<TMP_Text>().text)
            {
                characters[activeCharacter].activeAtk = characters[activeCharacter].attacks[i].attackName;
                break;
            }
        }
    }

    private void AttackMenu()
    {
        foreach (Attacks attack in characters[activeCharacter].attacks)
        {
            btn = Instantiate(attackBtn);
            btn.transform.SetParent(attackPanel.transform);
            btn.transform.localScale = Vector3.one;
            btn.onClick.AddListener(Back);
            btn.GetComponentInChildren<TMP_Text>().text = attack.attackName;
            buttons.Add(btn);
        }
    }

    private void ClearAttackMenu()
    {
        MouseController.Instance.HideRangeTiles();
        MouseController.Instance.HideAttackRangeTiles();
        if (buttons != null)
        {
            foreach (Button btn in buttons)
            {
                Destroy(btn.gameObject);
            }
            buttons.Clear();
        }
    }

    private void UpdateUI()
    {
        AttackMenu();

        FindAnyObjectByType<Camera>().transform.position = new Vector3(characters[activeCharacter].transform.position.x, characters[activeCharacter].transform.position.y, -10);
        MouseController.Instance.SetCharacter(characters[activeCharacter]);
        if (characters[activeCharacter].canMove)
            MouseController.Instance.GetInRangeTiles();
        else
            MouseController.Instance.GetInAttackRangeTiles(characters[activeCharacter].activeTile);
    }

    private void CheckGameOver()
    {
        if (characters.Count == 0 && !gameOver)
        {
            ClearAttackMenu();
            Debug.Log("Game Over");
            gameOver = true;
        }

        if (enemies.Count == 0 && !gameOver)
        {
            ClearAttackMenu();
            Debug.Log("Winner");
            gameOver = true;
        }
    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        inventoryOpen = true;
        selector.SetActive(true);
        selectedSlotIndex = 0;
        inventoryState = 0;
        selector.transform.position = inventorySlots[selectedSlotIndex].transform.position;

        currentCharacter = characters[activeCharacter];

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            var slotImage = inventorySlots[i];
            var item = currentCharacter.inventory[i];

            if (item != null && item.quantity > 0)
            {
                slotImage.sprite = item.icon;
                slotImage.enabled = true;
            }
            else
            {
                slotImage.enabled = false;
            }
        }
    }
    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        inventoryOpen = false;
        selector.SetActive(false);
        opcionesPanel.SetActive(false);
        inventoryState = 0;
    }

    private void UseItem(int index)
    {
        var item = currentCharacter.inventory[index];
        if (item != null && item.quantity > 0)
        {
            item.Use(currentCharacter);
            item.quantity--;

            if (item.quantity <= 0)
                currentCharacter.inventory[index] = null;

            UpdateInventoryVisuals();
            inventoryPanel.SetActive(false);
            currentCharacter.canAct = false;
            UpdateUI();
        }
    }
    private void UpdateInventoryVisuals()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            var item = currentCharacter.inventory[i];
            var slot = inventorySlots[i];

            if (item != null && item.quantity > 0)
            {
                slot.sprite = item.icon;
                slot.enabled = true;
            }
            else
            {
                slot.enabled = false;
            }
        }
    }
    public void OnSlotClicked(int index)
    {
        var item = currentCharacter.inventory[index];

        if (item != null && item.quantity > 0)
        {
            item.Use(currentCharacter);
            item.quantity--;

            if (item.quantity <= 0)
            {
                currentCharacter.inventory[index] = null;
            }

            UpdateInventoryVisuals();
            inventoryPanel.SetActive(false);
            currentCharacter.canAct = false;
            UpdateUI();
        }
    }


}
