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
            

            Criatura criaturaSinIncubar = gameManager.listaSinIncubar[0];
            gameManager.listaSinIncubar.RemoveAt(0);
            GameObject nuevaCriatura = Instantiate(criaturaPrefab, transform.position, transform.rotation);
            Criatura nuevaCriaturaComponent = nuevaCriatura.GetComponent<Criatura>();
            // Copiar propiedades de la criatura sin incubar a la nueva criatura
            nuevaCriaturaComponent.Nombre = criaturaSinIncubar.Nombre;
            nuevaCriaturaComponent.Vida = criaturaSinIncubar.Vida;
            nuevaCriaturaComponent.VidaMax = criaturaSinIncubar.VidaMax;
            nuevaCriaturaComponent.Energia = criaturaSinIncubar.Energia;
            nuevaCriaturaComponent.EnergiaMax = criaturaSinIncubar.EnergiaMax;
            nuevaCriaturaComponent.Acciones = criaturaSinIncubar.Acciones;
            nuevaCriaturaComponent.AccionesMax = criaturaSinIncubar.AccionesMax;
            nuevaCriaturaComponent.Vitalidad = criaturaSinIncubar.Vitalidad;
            nuevaCriaturaComponent.Vigor = criaturaSinIncubar.Vigor;
            nuevaCriaturaComponent.Fuerza = criaturaSinIncubar.Fuerza;
            nuevaCriaturaComponent.resistencia = criaturaSinIncubar.resistencia;
            nuevaCriaturaComponent.Adaptabilidad = criaturaSinIncubar.Adaptabilidad;
            nuevaCriaturaComponent.Inteligencia = criaturaSinIncubar.Inteligencia;
            nuevaCriaturaComponent.Velocidad = criaturaSinIncubar.Velocidad;
            nuevaCriaturaComponent.Metabolismo = criaturaSinIncubar.Metabolismo;
            nuevaCriaturaComponent.sprites = criaturaSinIncubar.sprites;
            nuevaCriaturaComponent.traits = new List<TraitBase>(criaturaSinIncubar.traits);
            gameManager.AgregarCriatura(nuevaCriaturaComponent);
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
