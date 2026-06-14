using UnityEngine;

public class Parrilla : MonoBehaviour
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
            Ingrediente.TipoIngrediente.Carne &&
            ingrediente.tipo !=
            Ingrediente.TipoIngrediente.Tocino)
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