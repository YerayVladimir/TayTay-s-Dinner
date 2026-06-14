using UnityEngine;

public class BotonIngrediente : MonoBehaviour
{
    public int idIngrediente;
    public HamburguesaActual hamburguesa;

    public void ActivarIngrediente()
    {
        hamburguesa.AgregarIngrediente(idIngrediente);
    }
}
