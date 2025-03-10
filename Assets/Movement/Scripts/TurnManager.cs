using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public List<CharacterInfo> characters = new List<CharacterInfo>();
    public int activeCharacter;
    public int totMoved = 0;
    public int turn = 0;
    public Button attackBtn;
    public GameObject turnPanel;
    public GameObject attackPanel;
    private Button btn;
    public List<Button> buttons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Battle starts!");
        characters.AddRange(FindObjectsByType<CharacterInfo>(FindObjectsSortMode.None));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCombat();
        if (Input.GetKeyDown(KeyCode.D))
            NextCharacter();
        if (Input.GetKeyDown(KeyCode.A))
            PreviousCharacter();
        if (!characters[activeCharacter].canMove && !characters[activeCharacter].canAct)
        {
            totMoved++;
            if(totMoved == characters.Count)
                StartCombat();
            else
                NextCharacter();
        }
    }

    private void StartCombat()
    {
        turn++;
        Debug.Log("Turn" + turn);
        totMoved = 0;
        foreach (CharacterInfo character in characters)
        {
            MouseController.Instance.SetCharacter(character);
            MouseController.Instance.PositionCharacterOnTile(character, character.activeTile);
            character.canMove = true;
            character.canAct = false;
        }
        activeCharacter = 0;

        AttackMenu();

        FindAnyObjectByType<Camera>().transform.position = new Vector3(characters[activeCharacter].transform.position.x, characters[activeCharacter].transform.position.y, -10);
        MouseController.Instance.SetCharacter(characters[activeCharacter]);
        MouseController.Instance.GetInRangeTiles();
    }

    private void NextCharacter()
    {
        MouseController.Instance.HideRangeTiles();
        MouseController.Instance.HideAttackRangeTiles();
        ClearAttackMenu();
        if (activeCharacter < characters.Count - 1)
        {
            activeCharacter++;
            AttackMenu();
            if (!characters[activeCharacter].canMove && !characters[activeCharacter].canAct)
                NextCharacter();
        }
        else
        {
            activeCharacter = 0;
            AttackMenu();
            if (!characters[activeCharacter].canMove && !characters[activeCharacter].canAct)
                NextCharacter();
        }
        FindAnyObjectByType<Camera>().transform.position = new Vector3(characters[activeCharacter].transform.position.x, characters[activeCharacter].transform.position.y, -10);
        MouseController.Instance.SetCharacter(characters[activeCharacter]);
        if(characters[activeCharacter].canMove)
            MouseController.Instance.GetInRangeTiles();
        else
            MouseController.Instance.GetInAttackRangeTiles(characters[activeCharacter].activeTile);
    }

    private void PreviousCharacter()
    {
        MouseController.Instance.HideRangeTiles();
        MouseController.Instance.HideAttackRangeTiles();
        ClearAttackMenu();
        if (activeCharacter > 0)
        {
            activeCharacter--;
            AttackMenu();
            if (!characters[activeCharacter].canMove && !characters[activeCharacter].canAct)
                PreviousCharacter();
        }
        else
        {
            activeCharacter = characters.Count-1;
            AttackMenu();
            if (!characters[activeCharacter].canMove && !characters[activeCharacter].canAct)
                PreviousCharacter();
        }
        FindAnyObjectByType<Camera>().transform.position = new Vector3(characters[activeCharacter].transform.position.x, characters[activeCharacter].transform.position.y, -10);
        MouseController.Instance.SetCharacter(characters[activeCharacter]);
        if (characters[activeCharacter].canMove)
            MouseController.Instance.GetInRangeTiles();
        else
            MouseController.Instance.GetInAttackRangeTiles(characters[activeCharacter].activeTile);
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
            btn.onClick.AddListener(Back);
            btn.GetComponentInChildren<TMP_Text>().text = attack.attackName;
            buttons.Add(btn);
        }
    }

    private void ClearAttackMenu()
    {
        if (buttons != null)
        {
            foreach (Button btn in buttons)
            {
                Destroy(btn.gameObject);
            }
            buttons.Clear();
        }
    }
}
