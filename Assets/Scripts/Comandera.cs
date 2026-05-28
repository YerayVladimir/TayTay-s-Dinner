using UnityEngine;
using UnityEngine.InputSystem;

public class Comandera : MonoBehaviour
{
    public GameObject prefabComanda;
    public Transform puntoSpawn;

    private int contadorOrdenes = 1;

    void Update()
    {
        // Detecta Space o Flecha Abajo
        if ((Keyboard.current?.spaceKey.wasPressedThisFrame ?? false) ||
            (Keyboard.current?.downArrowKey.wasPressedThisFrame ?? false))
        {
            GenerarComanda();
        }
    }

    public void GenerarComanda()
    {
        if (prefabComanda == null || puntoSpawn == null)
        {
            Debug.LogError("Faltan referencias en Comandera");
            return;
        }

        GameObject obj = Instantiate(prefabComanda, puntoSpawn.position, Quaternion.identity, puntoSpawn);

        Comanda comanda = obj.GetComponent<Comanda>();

        if (comanda == null)
        {
            Debug.LogError("El prefab no tiene script Comanda");
            return;
        }

        comanda.numeroOrden = contadorOrdenes++;
    }
}