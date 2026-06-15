using UnityEngine;

public class ZonaArmado : MonoBehaviour
{
    public HamburguesaActual hamburguesa;

    public PuntoApilado apilado;

    private void OnTriggerEnter(Collider other)
    {
        Ingrediente ingrediente =
            other.GetComponentInParent<Ingrediente>();

        if (ingrediente == null)
            return;

        int id = ingrediente.ObtenerIDActual();

        if (id == -1)
        {
            Debug.Log("Este ingrediente no tiene un ID válido para agregarse.");
            return;
        }

        hamburguesa.AgregarIngrediente(id);

        ingrediente.DetenerCoccion();

        apilado.AgregarIngrediente(
            ingrediente.gameObject);
    }
}