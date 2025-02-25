using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<CharacterInfo> characters = new List<CharacterInfo>();
    public int activeCharacter;
    public int totMoved = 0;
    public int turn = 0;

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
        if (!characters[activeCharacter].canMove)
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
        }
        activeCharacter = 0;
        FindAnyObjectByType<Camera>().transform.position = new Vector3(characters[activeCharacter].transform.position.x, characters[activeCharacter].transform.position.y, -10);
        MouseController.Instance.SetCharacter(characters[activeCharacter]);
        MouseController.Instance.GetInRangeTiles();
    }

    private void NextCharacter()
    {
        MouseController.Instance.HideRangeTiles();
        if (activeCharacter < characters.Count - 1)
        {
            activeCharacter++;
            if (!characters[activeCharacter].canMove)
                NextCharacter();
        }
        else
        {
            activeCharacter = 0;
            if (!characters[activeCharacter].canMove)
                NextCharacter();
        }
        FindAnyObjectByType<Camera>().transform.position = new Vector3(characters[activeCharacter].transform.position.x, characters[activeCharacter].transform.position.y, -10);
        MouseController.Instance.SetCharacter(characters[activeCharacter]);
        MouseController.Instance.GetInRangeTiles();
    }

    private void PreviousCharacter()
    {
        MouseController.Instance.HideRangeTiles();
        if (activeCharacter > 0)
        {
            activeCharacter--;
            if (!characters[activeCharacter].canMove)
                PreviousCharacter();
        }
        else
        {
            activeCharacter = characters.Count-1;
            if (!characters[activeCharacter].canMove)
                PreviousCharacter();
        }
        FindAnyObjectByType<Camera>().transform.position = new Vector3(characters[activeCharacter].transform.position.x, characters[activeCharacter].transform.position.y, -10);
        MouseController.Instance.SetCharacter(characters[activeCharacter]);
        MouseController.Instance.GetInRangeTiles();
    }
}
