using UnityEngine;
using System.Collections.Generic;

public class PuntoApilado : MonoBehaviour
{
    public Transform puntoBase;

    public float alturaPorIngrediente = 0.15f;

    private List<GameObject> ingredientesApilados =
        new List<GameObject>();

    public void AgregarIngrediente(
        GameObject ingrediente)
    {
        float alturaActual =
            ingredientesApilados.Count *
            alturaPorIngrediente;

        ingrediente.transform.SetParent(
            transform);

        ingrediente.transform.position =
            puntoBase.position +
            Vector3.up * alturaActual;

        ingrediente.transform.rotation =
            puntoBase.rotation;

        Rigidbody rb =
            ingrediente.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        ingredientesApilados.Add(
            ingrediente);
    }
}