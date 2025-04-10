using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Inventario : MonoBehaviour
{
    public List<GameObject> Bag = new List<GameObject>();
    public GameObject[] inventario;
    public bool activar_inventario;

    public GameObject Selector;
    public int ID;

    public List<UnityEngine.UI.Image> Equipar = new List<UnityEngine.UI.Image>();
    public int ID_equipar;
    public int Fase_Inventario;

    public GameObject opciones;
    public UnityEngine.UI.Image[] seleccion;
    public Sprite[] Seleccion_Sprite;
    public int ID_Selecion;

    public InventarioGlobal inventarioGlobal; // asignar en inspector
    private void Start()
    {
        inventarioGlobal.OnInventarioChanged.AddListener(ActualizarVisual);
        ActualizarVisual();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Sprite itemSprite = collision.GetComponent<SpriteRenderer>().sprite;
            inventarioGlobal.AgregarItem(itemSprite); // ✅ agrega al global
            Destroy(collision.gameObject);
        }

    }

    void ActualizarVisual()
    {
        for (int i = 0; i < Bag.Count; i++)
        {
            if (i < inventarioGlobal.itemIcons.Count)
            {
                Bag[i].GetComponent<UnityEngine.UI.Image>().sprite = inventarioGlobal.itemIcons[i];
                Bag[i].GetComponent<UnityEngine.UI.Image>().enabled = true;
            }
            else
            {
                Bag[i].GetComponent<UnityEngine.UI.Image>().sprite = null;
                Bag[i].GetComponent<UnityEngine.UI.Image>().enabled = false;
            }
        }
    }

    public void navegar()
    {
        switch (Fase_Inventario)
        {
            case 0:
                Selector.SetActive(true);
                opciones.SetActive(false);
                inventario[1].SetActive(false);

                if (Input.GetKeyDown(KeyCode.W))
                {
                    ID_equipar--;
                    if (ID_equipar < 0) ID_equipar = 0; // Validación para evitar índices negativos
                }
                if (Input.GetKeyDown(KeyCode.S) && ID_equipar < Equipar.Count - 1)
                {
                    ID_equipar++;
                }

                if (ID_equipar >= 0 && ID_equipar < Equipar.Count)
                {
                    Selector.transform.position = Equipar[ID_equipar].transform.position;
                }

                if (Input.GetKeyDown(KeyCode.F) && activar_inventario)
                {
                    Fase_Inventario = 1;
                }
                break;

            case 1:
                Selector.SetActive(true);
                opciones.SetActive(false);

                if (ID >= 0 && ID < Bag.Count && Input.GetKeyDown(KeyCode.F) && Bag[ID].GetComponent<UnityEngine.UI.Image>().enabled == true)
                {
                    Fase_Inventario = 2;
                }

                inventario[1].SetActive(true);

                if (Input.GetKeyUp(KeyCode.D) && ID < Bag.Count - 1)
                {
                    ID++;
                }
                if (Input.GetKeyUp(KeyCode.A) && ID > 0)
                {
                    ID--;
                }
                if (Input.GetKeyDown(KeyCode.W) && ID > 3)
                {
                    ID -= 4;
                }
                if (Input.GetKeyDown(KeyCode.S) && ID + 4 < Bag.Count)
                {
                    ID += 4;
                }

                if (ID >= 0 && ID < Bag.Count)
                {
                    Selector.transform.position = Bag[ID].transform.position;
                }

                if (Input.GetKeyDown(KeyCode.G) && activar_inventario)
                {
                    Fase_Inventario = 0;
                }
                break;

            case 2:
                if (Input.GetKeyDown(KeyCode.G))
                {
                    Fase_Inventario = 1;
                }

                opciones.SetActive(true);
                if (ID >= 0 && ID < Bag.Count)
                {
                    opciones.transform.position = Bag[ID].transform.position;
                }

                Selector.SetActive(false);

                if (Input.GetKeyDown(KeyCode.W) && ID_Selecion > 0)
                {
                    ID_Selecion--;
                }
                if (Input.GetKeyDown(KeyCode.S) && ID_Selecion < seleccion.Length - 1)
                {
                    ID_Selecion++;
                }

                switch (ID_Selecion)
                {
                    case 0:
                        seleccion[0].sprite = Seleccion_Sprite[1];
                        seleccion[1].sprite = Seleccion_Sprite[0];

                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            if (ID >= 0 && ID < Bag.Count && ID_equipar >= 0 && ID_equipar < Equipar.Count)
                            {
                                if (Equipar[ID_equipar].GetComponent<UnityEngine.UI.Image>().enabled == false)
                                {
                                    Equipar[ID_equipar].GetComponent<UnityEngine.UI.Image>().sprite = Bag[ID].GetComponent<UnityEngine.UI.Image>().sprite;
                                    Equipar[ID_equipar].GetComponent<UnityEngine.UI.Image>().enabled = true;
                                    Bag[ID].GetComponent<UnityEngine.UI.Image>().sprite = null;
                                    Bag[ID].GetComponent<UnityEngine.UI.Image>().enabled = false;
                                }
                                else
                                {
                                    Sprite obj = Bag[ID].GetComponent<UnityEngine.UI.Image>().sprite;
                                    Bag[ID].GetComponent<UnityEngine.UI.Image>().sprite = Equipar[ID_equipar].GetComponent<UnityEngine.UI.Image>().sprite;
                                    Equipar[ID_equipar].GetComponent<UnityEngine.UI.Image>().sprite = obj;
                                }

                                Fase_Inventario = 0;
                            }
                        }
                        break;

                    case 1:
                        seleccion[0].sprite = Seleccion_Sprite[0];
                        seleccion[1].sprite = Seleccion_Sprite[1];

                        if (Input.GetKeyDown(KeyCode.F) && ID >= 0 && ID < Bag.Count)
                        {
                            // 🔴 Verificamos que haya un sprite válido
                            Sprite icono = Bag[ID].GetComponent<UnityEngine.UI.Image>().sprite;

                            if (icono != null)
                            {
                                inventarioGlobal.QuitarItem(icono); // ✅ Esto elimina del inventario global real
                            }

                            // 🔁 Limpiamos visualmente el slot
                            Bag[ID].GetComponent<UnityEngine.UI.Image>().sprite = null;
                            Bag[ID].GetComponent<UnityEngine.UI.Image>().enabled = false;

                            Fase_Inventario = 1;
                        }
                        break;
                }
                break;
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        navegar();

        if (activar_inventario)
        {
            inventario[0].SetActive(true);
            Time.timeScale = 0f;
            Movimiento movimiento = GetComponent<Movimiento>();
            movimiento.enabled = false;
        }
        else
        {
            Fase_Inventario = 0;
            inventario[0].SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            activar_inventario = !activar_inventario;
            Time.timeScale = 1f;

            Movimiento movimiento = GetComponent<Movimiento>();
            movimiento.enabled = true;
        }
    }
}
