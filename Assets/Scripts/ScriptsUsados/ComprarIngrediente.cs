using UnityEngine;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
  [Header("Referencias")]
    public Eventos eventos;
    public SpawnerIngrediente spawnerIngrediente;

    [Header("Datos de compra")]
    public int idIngrediente;
    public int precio = 20;

    [Header("Estado")]
    public bool comprado = false;

    [Header("UI opcional")]
    public TextMeshProUGUI textoCompra;

    private void Start()
    {
        ActualizarTexto();

        if (!comprado)
        {
            if (spawnerIngrediente != null)
            {
                spawnerIngrediente.Desactivar();
            }
        }
        else
        {
            DesbloquearSinCobrar();
        }
    }

    public void Comprar()
    {
        if (eventos == null)
        {
            Debug.LogError("Falta asignar Eventos.");
            return;
        }

        if (spawnerIngrediente == null)
        {
            Debug.LogError("Falta asignar SpawnerIngrediente.");
            return;
        }

        if (comprado)
        {
            eventos.MostrarMensajeEntrega("Este ingrediente ya fue comprado.", false);
            return;
        }

        if (!eventos.TieneDineroSuficiente(precio))
        {
            eventos.MostrarMensajeEntrega("No tienes suficiente dinero.", false);
            return;
        }

        bool agregado =
            eventos.AgregarIngredienteDisponible(idIngrediente);

        if (!agregado)
            return;

        eventos.GastarDinero(precio);

        spawnerIngrediente.Activar();

        comprado = true;

        eventos.MostrarMensajeEntrega(
            "Compraste " + eventos.ObtenerNombreIngrediente(idIngrediente),
            true
        );

        ActualizarTexto();
    }

    private void DesbloquearSinCobrar()
    {
        if (eventos != null)
        {
            if (!eventos.IngredienteYaDisponible(idIngrediente))
            {
                eventos.AgregarIngredienteDisponible(idIngrediente);
            }
        }

        if (spawnerIngrediente != null)
        {
            spawnerIngrediente.Activar();
        }

        comprado = true;

        ActualizarTexto();
    }

    private void ActualizarTexto()
    {
        if (textoCompra == null)
            return;

        if (comprado)
        {
            textoCompra.text = "Comprado";
        }
        else
        {
            textoCompra.text = "Comprar $" + precio;
        }
    }
}