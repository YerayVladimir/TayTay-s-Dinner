using UnityEngine;

public class AgarrarObjetos : MonoBehaviour
{
    public Camera camara;

    public float distanciaMaxima = 4f;

    public Transform puntoSujecion;

    private ObjetoMovible objetoActual;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            IntentarTomar();
        }

        if (Input.GetMouseButtonUp(0))
        {
            SoltarObjeto();
        }

        if (objetoActual != null)
        {
            objetoActual.transform.position =
                Vector3.Lerp(
                    objetoActual.transform.position,
                    puntoSujecion.position,
                    15f * Time.deltaTime);
        }
    }

    void IntentarTomar()
    {
        if (objetoActual != null)
            return;

        Ray ray =
            camara.ScreenPointToRay(
                Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(
            ray,
            out hit,
            distanciaMaxima))
        {
            ObjetoMovible objeto = hit.collider.GetComponentInParent<ObjetoMovible>();

            if (objeto == null)
                return;

            objetoActual = objeto;

            objeto.Tomar();
        }
    }

    void SoltarObjeto()
    {
        if (objetoActual == null)
            return;

        objetoActual.Soltar();

        objetoActual = null;
    }
}