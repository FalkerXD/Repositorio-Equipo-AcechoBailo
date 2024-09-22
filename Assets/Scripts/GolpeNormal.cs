using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolpeNormal : MonoBehaviour
{
    public float dañoGolpeNormal = 10f; // Daño del golpe normal
    public float tiempoDeDaño = 0.5f; // Tiempo que el golpe normal debe aplicar daño

    private bool golpeActivo = false;
    private float tiempoDeActivacion;

    private void OnTriggerEnter(Collider other)
    {
        if (golpeActivo && other.CompareTag("Enemigo")) // Ajusta el tag según sea necesario
        {
            Salud saludEnemigo = other.GetComponent<Salud>();
            if (saludEnemigo != null)
            {
                saludEnemigo.RecibirDaño(dañoGolpeNormal);
            }
        }
    }

    public void ActivarGolpeNormal()
    {
        golpeActivo = true;
        tiempoDeActivacion = Time.time;
        StartCoroutine(EsperarYAplicarDaño());
    }

    private IEnumerator EsperarYAplicarDaño()
    {
        while (golpeActivo)
        {
            yield return new WaitForSeconds(tiempoDeDaño);
            // Aplica el daño mientras el golpe está activo
            Collider[] coliders = Physics.OverlapSphere(transform.position, 1f); // Ajusta el tamaño del área según sea necesario
            foreach (var collider in coliders)
            {
                if (collider.CompareTag("Enemigo"))
                {
                    Salud saludEnemigo = collider.GetComponent<Salud>();
                    if (saludEnemigo != null)
                    {
                        saludEnemigo.RecibirDaño(dañoGolpeNormal);
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