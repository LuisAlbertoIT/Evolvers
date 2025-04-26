using UnityEngine;

public class Persistente : MonoBehaviour
{
    public static Persistente Instance;

    public InventarioGlobal inventarioGlobal;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
