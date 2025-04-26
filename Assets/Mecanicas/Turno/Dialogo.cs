using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.ObjectModel;

public class Dialogo : MonoBehaviour
{
    public TextMeshProUGUI dialogoIncubar;
    public string[] lines;
    public float textSpeed = 0.1f;
    public GameObject imagenFinal; // Imagen a activar al finalizar el diálogo
    int index;

    void Start()
    {
        imagenFinal.SetActive(false);
        dialogoIncubar.text = string.Empty;
        StarDialogue();

        if (imagenFinal != null)
        {
            imagenFinal.SetActive(false); // Asegurarse de que esté oculta al inicio
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (dialogoIncubar.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogoIncubar.text = lines[index];
            }
        }
    }

    public void StarDialogue()
    {
        index = 0;
        StartCoroutine(WriteLine());
    }

    IEnumerator WriteLine()
    {
        foreach (char letter in lines[index].ToCharArray())
        {
            dialogoIncubar.text += letter;
            yield return new WaitForSeconds(textSpeed);
            
                imagenFinal.SetActive(true);
            
        }
    }

    public void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogoIncubar.text = string.Empty;
            StartCoroutine(WriteLine());
        }
        else
        {

            Destroy(gameObject);

        }
    }
}
