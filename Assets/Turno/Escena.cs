using UnityEngine;
using UnityEngine.SceneManagement;

public class Escena : MonoBehaviour
{
    public int sceneIndex;

    public Vector2 playerDestination;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DataInstance.Instance.playerPosition = playerDestination;
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
