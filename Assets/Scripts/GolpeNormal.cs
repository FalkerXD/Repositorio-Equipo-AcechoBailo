using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolpeNormal : MonoBehaviour
{
    public float da�oGolpeNormal = 10f;   // Da�o del golpe normal
    public float intervaloDeDa�o = 0.5f;  // Intervalo de tiempo entre aplicaciones de da�o
    private bool golpeActivo = false;     // Indica si el golpe est� activo
    private List<Collider> enemigosDentroDelArea = new List<Collider>(); // Lista de enemigos en el �rea

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo")) // Verifica si el objeto es un enemigo
        {
            enemigosDentroDelArea.Add(other); // A�ade el enemigo a la lista
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemigo")) // Cuando el enemigo sale del �rea
        {
            enemigosDentroDelArea.Remove(other); // Elimina el enemigo de la lista
        }
    }

    public void ActivarGolpeNormal()
    {
        golpeActivo = true;
        StartCoroutine(AplicarDa�oContinuo());
    }

    private IEnumerator AplicarDa�oContinuo()
    {
        // Mientras el golpe est� activo, aplica da�o cada intervalo de tiempo
        while (golpeActivo)
        {
            foreach (Collider enemigo in enemigosDentroDelArea)
            {
                Salud saludEnemigo = enemigo.GetComponent<Salud>();
                if (saludEnemigo != null)
                {
                    saludEnemigo.RecibirDa�o(da�oGolpeNormal); // Aplica el da�o al enemigo
                }
            }
            // Espera antes de aplicar el da�o de nuevo
            yield return new WaitForSeconds(intervaloDeDa�o);
        }
    }

    public void DesactivarGolpeNormal()
    {
        golpeActivo = false; // Desactiva el golpe
        StopCoroutine(AplicarDa�oContinuo()); // Det�n el da�o continuo
    }
}