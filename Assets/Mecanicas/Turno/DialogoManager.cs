using System.Collections;
using TMPro;
using UnityEngine;

public class DialogoManager : MonoBehaviour
{
    [System.Serializable]
    public class Dialogo
    {
        public GameObject panelDialogo; // Panel que contiene el di�logo
        public TextMeshProUGUI textoDialogo; // Texto del di�logo
        public string[] lineasDialogo; // L�neas de texto
        public GameObject[] imagenesDialogo; // Varias im�genes que acompa�an el di�logo
    }

    public Dialogo[] dialogos; // Todos los di�logos
    public float velocidadTexto = 0.05f;
    public GameObject imagenFinal; // Imagen final que se activa al terminar

    private int dialogoActivo = -1; // �ndice del di�logo activo
    private int indiceLinea = 0; // L�nea actual
    private bool escribiendo = false;

    void Start()
    {
        foreach (var d in dialogos)
        {
            d.panelDialogo.SetActive(false);
            foreach (var imagen in d.imagenesDialogo)
            {
                if (imagen != null)
                    imagen.SetActive(false);
            }
        }

        if (imagenFinal != null)
            imagenFinal.SetActive(false);
    }

    void Update()
    {
        if (dialogoActivo != -1 && Input.GetKeyDown(KeyCode.Space))
        {
            if (escribiendo)
            {
                StopAllCoroutines();
                dialogos[dialogoActivo].textoDialogo.text = dialogos[dialogoActivo].lineasDialogo[indiceLinea];
                escribiendo = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    public void ActivarDialogo(int indice)
    {
        if (dialogoActivo != -1)
        {
            dialogos[dialogoActivo].panelDialogo.SetActive(false);
            foreach (var imagen in dialogos[dialogoActivo].imagenesDialogo)
            {
                if (imagen != null)
                    imagen.SetActive(false);
            }
        }

        dialogoActivo = indice;
        indiceLinea = 0;
        dialogos[dialogoActivo].panelDialogo.SetActive(true);
        foreach (var imagen in dialogos[dialogoActivo].imagenesDialogo)
        {
            if (imagen != null)
                imagen.SetActive(true);
        }

        StartCoroutine(EscribirLinea());
    }

    IEnumerator EscribirLinea()
    {
        escribiendo = true;
        dialogos[dialogoActivo].textoDialogo.text = "";

        foreach (char letra in dialogos[dialogoActivo].lineasDialogo[indiceLinea].ToCharArray())
        {
            dialogos[dialogoActivo].textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadTexto);
        }

        escribiendo = false;
    }

    public void NextLine()
    {
        indiceLinea++;

        if (indiceLinea < dialogos[dialogoActivo].lineasDialogo.Length)
        {
            StartCoroutine(EscribirLinea());
        }
        else
        {
            if (imagenFinal != null)
                imagenFinal.SetActive(true);

            foreach (var imagen in dialogos[dialogoActivo].imagenesDialogo)
            {
                if (imagen != null)
                    imagen.SetActive(false);
            }

            Destroy(dialogos[dialogoActivo].panelDialogo);
            dialogoActivo = -1;
        }
    }
    public void DestruirDialogo(int indice)
    {
        if (indice < 0 || indice >= dialogos.Length)
        {
            Debug.LogWarning("�ndice de di�logo fuera de rango al intentar destruir: " + indice);
            return;
        }

        if (dialogos[indice].panelDialogo != null)
        {
            Destroy(dialogos[indice].panelDialogo); // �Destruye el panel del di�logo!
            dialogos[indice].panelDialogo = null;    // Limpieza para evitar referencias rotas
        }

        // Si destruimos el activo, reseteamos
        if (dialogoActivo == indice)
        {
            dialogoActivo = -1;
        }
    }
}
