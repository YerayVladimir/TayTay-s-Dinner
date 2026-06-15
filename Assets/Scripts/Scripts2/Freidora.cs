using UnityEngine;

public class Freidora : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(
            "Algo entró a la freidora. Collider detectado: " +
            other.name);

        Ingrediente ingrediente =
            other.GetComponentInParent<Ingrediente>();

        if (ingrediente == null)
        {
            Debug.Log(
                other.name +
                " entró a la freidora, pero no tiene Ingrediente.cs.");

            return;
        }

        Debug.Log(
            ingrediente.name +
            " entró a la freidora. Tipo detectado: " +
            ingrediente.tipo);

        if (!ingrediente.requiereCoccion)
        {
            Debug.Log(
                ingrediente.name +
                " no requiere cocción, no puede freírse.");

            return;
        }

        if (ingrediente.tipo != Ingrediente.TipoIngrediente.Papas &&
            ingrediente.tipo != Ingrediente.TipoIngrediente.ArosCebolla)
        {
            Debug.Log(
                ingrediente.name +
                " no puede cocinarse en la freidora. Tipo: " +
                ingrediente.tipo);

            return;
        }

        ingrediente.IniciarCoccion();

        Debug.Log(
            ingrediente.name +
            " se colocó en la freidora y comenzó a cocinarse.");
    }

    private void OnTriggerExit(Collider other)
    {
        Ingrediente ingrediente =
            other.GetComponentInParent<Ingrediente>();

        if (ingrediente == null)
            return;

        if (ingrediente.tipo != Ingrediente.TipoIngrediente.Papas &&
            ingrediente.tipo != Ingrediente.TipoIngrediente.ArosCebolla)
            return;

        ingrediente.DetenerCoccion();

        Debug.Log(
            ingrediente.name +
            " se retiró de la freidora. Estado actual: " +
            ingrediente.estado);
    }
}