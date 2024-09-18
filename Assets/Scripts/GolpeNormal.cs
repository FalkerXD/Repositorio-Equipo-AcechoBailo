using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolpeNormal : MonoBehaviour
{
    public float da�oGolpeNormal = 10f; // Da�o del golpe normal
    public float tiempoDeDa�o = 0.5f; // Tiempo que el golpe normal debe aplicar da�o

    private bool golpeActivo = false;
    private float tiempoDeActivacion;

    private void OnTriggerEnter(Collider other)
    {
        if (golpeActivo && other.CompareTag("Enemigo")) // Ajusta el tag seg�n sea necesario
        {
            Salud saludEnemigo = other.GetComponent<Salud>();
            if (saludEnemigo != null)
            {
                saludEnemigo.RecibirDa�o(da�oGolpeNormal);
            }
        }
    }

    public void ActivarGolpeNormal()
    {
        golpeActivo = true;
        tiempoDeActivacion = Time.time;
        StartCoroutine(EsperarYAplicarDa�o());
    }

    private IEnumerator EsperarYAplicarDa�o()
    {
        while (golpeActivo)
        {
            yield return new WaitForSeconds(tiempoDeDa�o);
            // Aplica el da�o mientras el golpe est� activo
            Collider[] coliders = Physics.OverlapSphere(transform.position, 1f); // Ajusta el tama�o del �rea seg�n sea necesario
            foreach (var collider in coliders)
            {
                if (collider.CompareTag("Enemigo"))
                {
                    Salud saludEnemigo = collider.GetComponent<Salud>();
                    if (saludEnemigo != null)
                    {
                        saludEnemigo.RecibirDa�o(da�oGolpeNormal);
                    }
                }
            }
        }
    }

    public void DesactivarGolpeNormal()
    {
        golpeActivo = false;
    }
}