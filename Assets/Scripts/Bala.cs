using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float velocidad = 20f;          // Velocidad de la bala
    public float tiempoVida = 5f;          // Tiempo de vida de la bala antes de ser destruida
    public float retrasoActivacion = 0.5f; // Tiempo de espera antes de que la bala comience a moverse
    public float da�o = 20f;               // Cantidad de da�o que la bala inflige al enemigo
    public int maxColisiones = 2;          // N�mero m�ximo de colisiones antes de que la bala se destruya
    public string tagIgnorado;             // Tag al que no debe hacer da�o

    private Rigidbody rb;
    private bool activada = false;
    private float tiempoDeActivacion;
    private int colisiones = 0;            // Contador de colisiones

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tiempoDeActivacion = Time.time + retrasoActivacion;
        rb.velocity = Vector3.zero; // Inicialmente la velocidad es cero
    }

    void Update()
    {
        // Activar la bala despu�s del retraso de activaci�n
        if (!activada && Time.time >= tiempoDeActivacion)
        {
            activada = true;
            rb.velocity = transform.forward * velocidad;
        }

        // Destruye la bala despu�s de un tiempo si no ha colisionado
        Destroy(gameObject, tiempoVida);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verificar si el objeto tiene el tag que debe ser ignorado
        if (collision.gameObject.CompareTag(tagIgnorado))
        {
            return; // No hacer nada si el tag es el que se debe ignorar
        }

        // Verificar si la bala ha colisionado con un objeto que tiene el script de "Salud"
        Salud salud = collision.gameObject.GetComponent<Salud>();
        if (salud != null)
        {
            // Aplicar da�o al enemigo
            salud.RecibirDa�o(da�o);
        }

        // Incrementar el contador de colisiones y destruir la bala si ha colisionado el n�mero m�ximo de veces
        colisiones++;
        if (colisiones >= maxColisiones)
        {
            Destroy(gameObject);
        }
    }
}