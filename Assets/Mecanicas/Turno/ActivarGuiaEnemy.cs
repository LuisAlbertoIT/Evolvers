using UnityEngine;

public class ActivarGuiaEnemy : MonoBehaviour
{
    void Start()
    {
        GameObject canvas = GameObject.Find("GuiaEnemy");

        if (canvas != null)
        {
            canvas.SetActive(true);  
        }
        else
        {
            Debug.LogWarning("No se encontró el Canvas 'GuiaEnemy'");
        }
    }
}
