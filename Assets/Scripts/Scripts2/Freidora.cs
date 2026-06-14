using UnityEngine;

public class Freidora : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Ingrediente ingrediente =
            other.GetComponent<Ingrediente>();

        if (ingrediente == null)
            return;

        if (!ingrediente.requiereCoccion)
            return;

        if (ingrediente.tipo !=
            Ingrediente.TipoIngrediente.Papas &&
            ingrediente.tipo !=
            Ingrediente.TipoIngrediente.ArosCebolla)
            return;

        ingrediente.cocinandose = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Ingrediente ingrediente =
            other.GetComponent<Ingrediente>();

        if (ingrediente == null)
            return;

        ingrediente.cocinandose = false;
    }
}