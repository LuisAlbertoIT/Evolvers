using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnCollider : MonoBehaviour
{
    [SerializeField] private string sceneName;

    void OnTriggerEnter2D(Collider2D other)
    {
        print("Cambiando a la escena " + sceneName);

        if (other.CompareTag("Player"))
        {
            SceneChanger sceneChanger = FindFirstObjectByType<SceneChanger>();
            sceneChanger.CargarEscena(sceneName);
        }
    }

}
