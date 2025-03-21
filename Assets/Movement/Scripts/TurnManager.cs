using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        //characters.AddRange(FindObjectsByType<CharacterInfo>(FindObjectsSortMode.None));
    }

    // Update is called once per frame
    void Update()
    {
        CheckGameOver();
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
        else
            if (Input.GetKeyDown(KeyCode.Space))
                GetComponent<SceneChanger>().CargarEscena("Base");
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
}
