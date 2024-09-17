using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolpeArea : MonoBehaviour
{
    public float da�o = 10f;  // Da�o que inflige el golpe
    public List<string> tagsIgnorados;  // Lista de Tags que no recibir�n da�o
    private Transform personaje;  // Referencia al transform del personaje que ejecuta el golpe

    void Start()
    {
        // Obtener el transform del personaje (padre del �rea de golpe)
        personaje = transform.root;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto colisionado tiene el script de salud
        Salud saludEnemigo = other.GetComponent<Salud>();

        // Si el objeto tiene un tag que debe ser ignorado o es el mismo personaje que ejecuta el golpe, no aplicar da�o
        if (saludEnemigo != null && other.transform != personaje && !tagsIgnorados.Contains(other.tag))
        {
            // Aplicar da�o al enemigo
            saludEnemigo.RecibirDa�o(da�o);
        }
    }
}