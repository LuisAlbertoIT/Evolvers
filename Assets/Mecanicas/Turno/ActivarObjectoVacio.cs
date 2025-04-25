using UnityEngine;

public class ActivarObjectoVacio : MonoBehaviour
{
    public GameObject objetoAVacioActivar;

    void Start()
    {
        if (ActivarGuiaEnemy.instance != null && ActivarGuiaEnemy.instance.regresarDesdeExploracion)
        {
            objetoAVacioActivar.SetActive(true);

            // Reinicia la variable para futuras transiciones
            ActivarGuiaEnemy.instance.regresarDesdeExploracion = false;
        }
        else
        {
            objetoAVacioActivar.SetActive(false);
        }
    }
}
