using UnityEngine;

public class DataInstance : MonoBehaviour
{
   private static DataInstance instance;

    public Vector2 playerPosition;

    public static DataInstance Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("DataInstance");
                instance = go.AddComponent<DataInstance>();
                DontDestroyOnLoad(go);
               instance.playerPosition =  FindAnyObjectByType<Movimiento>().transform.position;
                
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance != null && instance !& this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad (gameObject);
        }
    }
}
