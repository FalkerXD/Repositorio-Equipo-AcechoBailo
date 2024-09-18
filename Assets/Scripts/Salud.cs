using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salud : MonoBehaviour
{
    public float saludMaxima = 100f;  // Salud m�xima del enemigo
    public float saludActual;         // Salud actual del enemigo, visible en el Inspector
    public GameObject efectoMuerte;   // Efecto visual o de part�culas para la muerte
    public bool desactivarEnemigo = true;  // Opci�n para desactivar el GameObject en lugar de destruirlo

    void Start()
    {
        // Inicializa la salud actual con la salud m�xima
        saludActual = saludMaxima;
    }

    // M�todo para aplicar da�o
    public void RecibirDa�o(float cantidad)
    {
        saludActual -= cantidad;  // Reducir la salud actual por la cantidad de da�o recibido
        saludActual = Mathf.Clamp(saludActual, 0f, saludMaxima);  // Asegurarse de que la salud no sea menor que 0

        // Verificar si la salud ha llegado a 0 o menos
        if (saludActual <= 0f)
        {
            Morir();
        }

        // Aqu� podr�as agregar otras l�gicas relacionadas con la muerte o efectos visuales
        Debug.Log($"Salud actual: {saludActual}"); // Opcional: Log para verificar en la consola
    }

    // M�todo para manejar la muerte del enemigo
    void Morir()
    {
        // Reproducir efecto de muerte si se ha asignado uno
        if (efectoMuerte != null)
        {
            Instantiate(efectoMuerte, transform.position, transform.rotation);
        }

        // Desactivar el GameObject si la opci�n est� habilitada
        if (desactivarEnemigo)
        {
            gameObject.SetActive(false);
        }
        else
        {
            // Alternativamente, destruir el GameObject
            Destroy(gameObject);
        }
    }
}