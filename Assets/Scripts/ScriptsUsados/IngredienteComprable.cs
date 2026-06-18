using UnityEngine;

public class IngredienteComprable : MonoBehaviour
{
    [Header("Grupo de compra")]
    public CompraGrupoIngredientes grupoCompra;

    public bool PuedeTomarseComprado()
    {
        if (grupoCompra == null)
        {
            Debug.LogError("Falta asignar Grupo Compra en " + gameObject.name);
            return false;
        }

        return grupoCompra.TieneCompraDisponible();
    }

    public bool IntentarUsarCompra()
    {
        if (grupoCompra == null)
        {
            Debug.LogError("Falta asignar Grupo Compra en " + gameObject.name);
            return false;
        }

        return grupoCompra.UsarCompra();
    }
}