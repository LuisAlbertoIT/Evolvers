using System.Collections;
using TMPro;
using UnityEngine;

public class DialogoManager : MonoBehaviour
{
    [System.Serializable]
    public class Dialogo
    {
        public GameObject panelDialogo; // Panel que contiene el diálogo
        public TextMeshProUGUI textoDialogo; // Texto del diálogo
        public string[] lineasDialogo; // Líneas de texto
        public GameObject[] imagenesDialogo; // Varias imágenes que acompañan el diálogo
       

    }
    private bool hayDialogoActivo = false;

    public Dialogo[] dialogos; // Todos los diálogos
    public float velocidadTexto = 0.05f;
    public GameObject imagenFinal; // Imagen final que se activa al terminar

    private int dialogoActivo = -1; // Índice del diálogo activo
    private int indiceLinea = 0; // Línea actual
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
        if (hayDialogoActivo && dialogoActivo != -1 && Input.GetKeyDown(KeyCode.Space))
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
        if (indice < 0 || indice >= dialogos.Length)
        {
            Debug.LogWarning("Índice de diálogo inválido en ActivarDialogo: " + indice);
            return;
        }

        if (dialogoActivo != -1)
        {
            // Antes de desactivar imágenes, aseguramos que no estén destruidas
            if (dialogos[dialogoActivo].panelDialogo != null)
                dialogos[dialogoActivo].panelDialogo.SetActive(false);

            foreach (var imagen in dialogos[dialogoActivo].imagenesDialogo)
            {
                if (imagen != null)
                    imagen.SetActive(false);
            }
        }

        dialogoActivo = indice;
        indiceLinea = 0;
        hayDialogoActivo = true; // ✅ Activamos la bandera

        if (dialogos[dialogoActivo].panelDialogo != null)
        {
            dialogos[dialogoActivo].panelDialogo.SetActive(true);

            foreach (var imagen in dialogos[dialogoActivo].imagenesDialogo)
            {
                if (imagen != null)
                    imagen.SetActive(true);
            }

            StartCoroutine(EscribirLinea());
        }

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
            Debug.LogWarning("Índice de diálogo fuera de rango al intentar destruir: " + indice);
            return;
        }

        if (dialogoActivo == indice)
        {
            StopAllCoroutines();
            escribiendo = false;
            dialogoActivo = -1;
            hayDialogoActivo = false; // <- MUY IMPORTANTE: desactivamos bandera

            if (dialogos[indice].textoDialogo != null)
            {
                dialogos[indice].textoDialogo.text = string.Empty;
            }
        }

        if (dialogos[indice].panelDialogo != null)
        {
            Destroy(dialogos[indice].panelDialogo);
            dialogos[indice].panelDialogo = null;
        }
    }
}
