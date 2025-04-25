using UnityEngine;

public class ActivarGuiaEnemy : MonoBehaviour
{
    public static ActivarGuiaEnemy instance;

    public bool regresarDesdeExploracion = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
