using UnityEngine;
using TMPro;

public class ComprarYDesbloquearObjeto : MonoBehaviour
{
    [Header("Referencias")]
    public Eventos eventos;

    [Header("Compra")]
    public int precio = 10;
    public string nombreCompra = "Ingrediente";

    [Header("Objeto a desbloquear")]
    public ObjectGrabbable objetoParaDesbloquear;

    [Header("Busqueda opcional para respawn")]
    public Transform puntoBusqueda;
    public float radioBusqueda = 1f;
    public LayerMask capasIngredientes = ~0;

    [Header("UI opcional")]
    public TextMeshProUGUI textoBoton;

    private void Start()
    {
        ActualizarTexto();
    }

    private void OnMouseDown()
    {
        Comprar();
    }

    public void Comprar()
    {
        if (eventos == null)
        {
            Debug.LogError("Falta asignar Eventos.");
            return;
        }

        ObjectGrabbable objetivo = ObtenerObjetoBloqueado();

        if (objetivo == null)
        {
            eventos.MostrarMensajeEntrega(
                "No hay " + nombreCompra + " bloqueado para comprar.",
                false
            );

            return;
        }

        if (eventos.dineroActual < precio)
        {
            eventos.MostrarMensajeEntrega(
                "No tienes suficiente dinero.",
                false
            );

            return;
        }

        eventos.QuitarDinero(precio);

        objetivo.DesbloquearObjeto();

        eventos.MostrarMensajeEntrega(
            "Compraste " + nombreCompra + " -$" + precio,
            true
        );

        Debug.Log("Objeto desbloqueado: " + objetivo.name);
    }

    private ObjectGrabbable ObtenerObjetoBloqueado()
    {
        if (objetoParaDesbloquear != null &&
            objetoParaDesbloquear.bloqueado)
        {
            return objetoParaDesbloquear;
        }

        if (puntoBusqueda == null)
            return null;

        Collider[] colliders = Physics.OverlapSphere(
            puntoBusqueda.position,
            radioBusqueda,
            capasIngredientes,
            QueryTriggerInteraction.Ignore
        );

        foreach (Collider col in colliders)
        {
            ObjectGrabbable grabbable =
                col.GetComponentInParent<ObjectGrabbable>();

            if (grabbable != null && grabbable.bloqueado)
            {
                objetoParaDesbloquear = grabbable;
                return grabbable;
            }
        }

        return null;
    }

    private void ActualizarTexto()
    {
        if (textoBoton != null)
        {
            textoBoton.text = nombreCompra + " $" + precio;
        }
    }
}