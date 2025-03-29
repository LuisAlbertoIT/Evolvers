using UnityEngine;

public class TriggerActivator : MonoBehaviour
{

    public GameObject targetObject; // El objeto con el script a activar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Activar el script del objeto cuando el jugador entra
            if (targetObject != null)
            {
                var script = targetObject.GetComponent<BattleManeger>(); // Tu script
                if (script != null)
                {
                    script.enabled = true;
                    Debug.Log("activo");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Desactivar el script del objeto cuando el jugador sale
            if (targetObject != null)
            {
                var script = targetObject.GetComponent<BattleManeger>(); // Tu script
                if (script != null)
                {
                    script.enabled = false;
                }
            }
        }

    }
}
