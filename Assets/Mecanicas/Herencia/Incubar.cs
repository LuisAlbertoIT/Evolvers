using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Incubar : MonoBehaviour
{
    public GameObject criaturaPrefab;
    public float tiempoDeEspera = 5f;
    private GameManager gameManager;
    public UISpriteAnimation spriteAnimation;
    public GameObject image1;
    public GameObject image2;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        image2.SetActive(false);
        image1.SetActive(false);
    }

    public void EsperarIncubacion()
    {
        if (gameManager.listaSinIncubar.Count == 0)
        {
            Debug.Log("No hay criaturas para incubar.");
            return;
        }
        StartCoroutine(IncubarCriatura());
        
    }

    private IEnumerator IncubarCriatura()
    {
        image1.SetActive(true);
        spriteAnimation.Func_PlayUIAnim();

        yield return new WaitForSeconds(tiempoDeEspera);
        if (gameManager.listaSinIncubar.Count > 0)
        {
            yield return new WaitForSeconds(2);
            image1.SetActive(false);
            image2.SetActive(false);

            // Obtener la criatura original de la lista sin incubar
            Criatura criaturaSinIncubar = gameManager.listaSinIncubar[0];
            gameManager.listaSinIncubar.RemoveAt(0);

            // Mover la criatura original a la lista de criaturas
            gameManager.AgregarCriatura(criaturaSinIncubar);

            // Opcional: Ajustar la posición de la criatura en la escena si es necesario
            criaturaSinIncubar.transform.position = transform.position;
            criaturaSinIncubar.transform.rotation = transform.rotation;
            criaturaSinIncubar.gameObject.SetActive(true);
        }
        spriteAnimation.Func_StopUIAnim();
        image2.SetActive(true);
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(IncubarCriatura());
    }
}
