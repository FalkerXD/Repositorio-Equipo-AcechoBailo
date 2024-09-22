using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolpeNormal : MonoBehaviour
{
    public float dañoGolpeNormal = 10f;   // Daño del golpe normal
    public float intervaloDeDaño = 0.5f;  // Intervalo de tiempo entre aplicaciones de daño
    private bool golpeActivo = false;     // Indica si el golpe está activo
    private List<Collider> enemigosDentroDelArea = new List<Collider>(); // Lista de enemigos en el área

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo")) // Verifica si el objeto es un enemigo
        {
            enemigosDentroDelArea.Add(other); // Añade el enemigo a la lista
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemigo")) // Cuando el enemigo sale del área
        {
            enemigosDentroDelArea.Remove(other); // Elimina el enemigo de la lista
        }
    }

    public void ActivarGolpeNormal()
    {
        golpeActivo = true;
        StartCoroutine(AplicarDañoContinuo());
    }

    private IEnumerator AplicarDañoContinuo()
    {
        // Mientras el golpe esté activo, aplica daño cada intervalo de tiempo
        while (golpeActivo)
        {
            foreach (Collider enemigo in enemigosDentroDelArea)
            {
                Salud saludEnemigo = enemigo.GetComponent<Salud>();
                if (saludEnemigo != null)
                {
                    saludEnemigo.RecibirDaño(dañoGolpeNormal); // Aplica el daño al enemigo
                }
            }
            // Espera antes de aplicar el daño de nuevo
            yield return new WaitForSeconds(intervaloDeDaño);
        }
    }

    public void DesactivarGolpeNormal()
    {
        golpeActivo = false; // Desactiva el golpe
        StopCoroutine(AplicarDañoContinuo()); // Detén el daño continuo
    }
}