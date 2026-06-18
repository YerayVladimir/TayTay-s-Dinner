using System.Collections.Generic;
using UnityEngine;

public class UIPool : MonoBehaviour
{
    public GameObject prefab;
    public Transform contenedor;
    public int tamañoInicial = 5;

    private List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        InicializarPool();
    }

    void InicializarPool()
    {
        for (int i = 0; i < tamañoInicial; i++)
        {
            GameObject obj = Instantiate(prefab, contenedor);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject Obtener()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // Si no hay disponibles, crea uno nuevo
        GameObject nuevo = Instantiate(prefab, contenedor);
        nuevo.SetActive(false);
        pool.Add(nuevo);

        return nuevo;
    }

    public void Devolver(GameObject obj)
    {
        obj.SetActive(false);
    }
}