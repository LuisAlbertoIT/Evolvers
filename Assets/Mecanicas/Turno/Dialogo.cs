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
    bool isDestroyed = false; // <- Añadido para evitar errores
    private Movimiento movimiento;

    void Start()
    {
        movimiento = FindObjectOfType<Movimiento>();
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
        // 🔵 Pausa el juego y desactiva movimiento
        if (movimiento != null)
            movimiento.enabled = false;

      



    }

    IEnumerator WriteLine()
    {

        dialogoIncubar.text = string.Empty;
        foreach (char letter in lines[index].ToCharArray())
        {
            dialogoIncubar.text += letter;
            yield return new WaitForSecondsRealtime(textSpeed); // <- Realtime!
        }

        if (imagenFinal != null)
            imagenFinal.SetActive(true);
    }

    public void NextLine()
    {
        if (isDestroyed) return; 

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

        gameObject.SetActive(false);
        DontDestroyOnLoad(this.gameObject);
        if (movimiento != null)
            movimiento.enabled = true;

        


    }

  
    public void OnButtonDestroyPressed()
    {
         gameObject.SetActive(false);

        DontDestroyOnLoad(this.gameObject);
    }
}
