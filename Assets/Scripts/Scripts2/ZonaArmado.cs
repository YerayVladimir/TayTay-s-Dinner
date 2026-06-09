using UnityEngine;

public class ZonaArmado : MonoBehaviour
{
    public HamburguesaActual hamburguesa;

    public PuntoApilado apilado;

    private void OnTriggerEnter(
        Collider other)
    {
        Ingrediente ingrediente =
            other.GetComponent<Ingrediente>();

        if (ingrediente == null)
            return;

        int id =
            ConvertirIngrediente(
                ingrediente);

        hamburguesa.AgregarIngrediente(id);

        apilado.AgregarIngrediente(
            ingrediente.gameObject);
    }

    int ConvertirIngrediente(
        Ingrediente ingrediente)
    {
        switch (ingrediente.tipo)
        {
            case Ingrediente.TipoIngrediente.PanAbajo:
                return 0;

            case Ingrediente.TipoIngrediente.Carne:

                if (ingrediente.estado ==
                    Ingrediente.EstadoCoccion.Cocinado)
                    return 1;

                if (ingrediente.estado ==
                    Ingrediente.EstadoCoccion.Quemado)
                    return 11;

                return 10;

            case Ingrediente.TipoIngrediente.Queso:
                return 2;

            case Ingrediente.TipoIngrediente.Lechuga:
                return 3;

            case Ingrediente.TipoIngrediente.Tomate:
                return 4;

            case Ingrediente.TipoIngrediente.Cebolla:
                return 5;

            case Ingrediente.TipoIngrediente.Pepinillo:
                return 6;

            case Ingrediente.TipoIngrediente.Tocino:

                if (ingrediente.estado ==
                    Ingrediente.EstadoCoccion.Cocinado)
                    return 7;

                if (ingrediente.estado ==
                    Ingrediente.EstadoCoccion.Quemado)
                    return 13;

                return 12;

            case Ingrediente.TipoIngrediente.Papas:
                return 8;

            case Ingrediente.TipoIngrediente.PanArriba:
                return 9;
        }

        return -1;
    }
}