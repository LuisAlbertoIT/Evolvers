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
    public GameObject imagenFinal; // Imagen a activar al finalizar el di�logo
    int index;
    bool isDestroyed = false; // <- A�adido para evitar errores

    void Start()
    {
        if (imagenFinal != null)
            imagenFinal.SetActive(true);

        dialogoIncubar.text = string.Empty;
        StarDialogue();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isDestroyed) return; // Si ya fue destruido, no hacemos nada

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
        dialogoIncubar.text = string.Empty; // <- Asegura que empieza vac�o
        foreach (char letter in lines[index].ToCharArray())
        {
            dialogoIncubar.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        if (imagenFinal != null)
            imagenFinal.SetActive(true);
    }

    public void NextLine()
    {
        if (isDestroyed) return; // Previene m�ltiples destrucciones

        if (index < lines.Length - 1)
        {
            index++;
            dialogoIncubar.text = string.Empty;
            StartCoroutine(WriteLine());
        }
        else
        {
            DestroyDialogue();
        }
    }

    public void DestroyDialogue()
    {
        if (isDestroyed) return;

        isDestroyed = true;

        // Ocultar imagen final si existe
        if (imagenFinal != null)
            imagenFinal.SetActive(false);

        StopAllCoroutines(); 

        Destroy(gameObject);
    }

    // Esta funci�n la puedes conectar al bot�n en el editor
    public void OnButtonDestroyPressed()
    {
        DestroyDialogue();
    }
}
