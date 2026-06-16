using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PedidoUIManager : MonoBehaviour
{
    [Header("UI de pedidos")]
    public GameObject prefabPedidoUI;
    public Transform contenedorPedidos;

    [Header("Límite de pedidos")]
    public int maxPedidosEnPantalla = 3;

    private Dictionary<Cliente, GameObject> pedidosUI =
        new Dictionary<Cliente, GameObject>();

    public bool PuedeCrearPedido()
    {
        return pedidosUI.Count < maxPedidosEnPantalla;
    }

    public int CantidadPedidosActivos()
    {
        return pedidosUI.Count;
    }

    public void CrearPedidoUI(Cliente cliente, List<int> pedido, Eventos eventos)
    {
        if (cliente == null)
        {
            Debug.LogError("No se puede crear UI porque el cliente es null.");
            return;
        }

        if (pedido == null)
        {
            Debug.LogError("No se puede crear UI porque el pedido es null.");
            return;
        }

        if (prefabPedidoUI == null)
        {
            Debug.LogError("Falta asignar Prefab Pedido UI en PedidoUIManager.");
            return;
        }

        if (contenedorPedidos == null)
        {
            Debug.LogError("Falta asignar Contenedor Pedidos en PedidoUIManager.");
            return;
        }

        if (pedidosUI.ContainsKey(cliente))
        {
            Debug.LogWarning("Este cliente ya tiene UI de pedido.");
            return;
        }

        if (!PuedeCrearPedido())
        {
            Debug.LogWarning("Ya hay máximo de pedidos en pantalla.");
            return;
        }

        GameObject nuevoPedidoUI = Instantiate(
            prefabPedidoUI,
            contenedorPedidos
        );

        TextMeshProUGUI texto =
            nuevoPedidoUI.GetComponentInChildren<TextMeshProUGUI>();

        if (texto != null)
        {
            texto.text = ConstruirTextoPedido(pedido, eventos);
        }
        else
        {
            Debug.LogWarning("El prefab de pedido no tiene TextMeshProUGUI en hijos.");
        }

        pedidosUI.Add(cliente, nuevoPedidoUI);
    }

    public void QuitarPedidoUI(Cliente cliente)
    {
        if (cliente == null)
            return;

        if (!pedidosUI.ContainsKey(cliente))
            return;

        GameObject pedidoUI = pedidosUI[cliente];

        if (pedidoUI != null)
        {
            Destroy(pedidoUI);
        }

        pedidosUI.Remove(cliente);
    }

    private string ConstruirTextoPedido(List<int> pedido, Eventos eventos)
    {
        string texto = "Pedido:\n";

        for (int i = 0; i < pedido.Count; i++)
        {
            int id = pedido[i];

            string nombre = eventos != null
                ? eventos.ObtenerNombreIngrediente(id)
                : "ID " + id;

            if (nombre == "Aros de Cebolla")
                nombre = "Aros cebolla";

            texto += "• " + nombre + "\n";
        }

        return texto;
    }
}