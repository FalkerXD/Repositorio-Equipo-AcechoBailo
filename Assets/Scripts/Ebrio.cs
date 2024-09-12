using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ebrio : MonoBehaviour
{
    public float radioDeteccion = 10f;   // Radio para detectar al jugador
    public float radioAtaque = 2f;       // Radio para atacar al jugador
    public float velocidad = 2f;         // Velocidad del enemigo
    public float tiempoTransicion = 1f;  // Tiempo de transici�n antes de cambiar de "Golpear" a "Correr"

    public Transform jugador;            // Referencia al transform del jugador

    private Animator anim;
    private Rigidbody rb;
    private bool persiguiendo = false;
    private bool atacando = false;
    private bool enTransicion = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Si no se ha asignado el jugador manualmente, lo busca por su tag
        if (jugador == null)
        {
            jugador = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Configuraci�n del Rigidbody para evitar empujones no deseados
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.drag = 5f;
        rb.angularDrag = 5f;
    }

    void Update()
    {
        if (jugador == null) return;

        float distanciaJugador = Vector3.Distance(transform.position, jugador.position);

        if (distanciaJugador <= radioAtaque)
        {
            AtacarJugador();
        }
        else if (distanciaJugador <= radioDeteccion)
        {
            if (atacando && !enTransicion)
            {
                StartCoroutine(TransicionDeAtaqueACorrer());
            }
            else if (!atacando)
            {
                PerseguirJugador();
            }
        }
        else
        {
            DejarDePerseguirYAtacar();
        }

        MirarJugador();
    }

    void PerseguirJugador()
    {
        // Si no est� atacando, activa la animaci�n de correr
        if (!atacando)
        {
            anim.SetBool("Quieto", false);
            anim.SetTrigger("Correr");
            persiguiendo = true;
        }

        // Mover hacia el jugador
        Vector3 direccion = (jugador.position - transform.position).normalized;
        rb.MovePosition(transform.position + direccion * velocidad * Time.deltaTime);
    }

    void AtacarJugador()
    {
        // Mientras est� en el radio de ataque, mantiene la animaci�n de ataque activa
        if (!atacando)
        {
            anim.ResetTrigger("Correr");
            anim.SetTrigger("Golpear");
            atacando = true;
            persiguiendo = false;
        }

        // Movimiento hacia el jugador mientras ataca
        Vector3 direccion = (jugador.position - transform.position).normalized;
        rb.MovePosition(transform.position + direccion * velocidad * Time.deltaTime);
    }

    IEnumerator TransicionDeAtaqueACorrer()
    {
        enTransicion = true;
        yield return new WaitForSeconds(tiempoTransicion);

        // Despu�s de la transici�n, activa la animaci�n de correr y desactiva la de golpear
        if (!JugadorEnRadioAtaque())
        {
            anim.ResetTrigger("Golpear");
            anim.SetTrigger("Correr");
            atacando = false;
        }
        enTransicion = false;
    }

    bool JugadorEnRadioAtaque()
    {
        // Comprueba si el jugador sigue dentro del radio de ataque
        return Vector3.Distance(transform.position, jugador.position) <= radioAtaque;
    }

    void DejarDePerseguirYAtacar()
    {
        // Detiene la animaci�n de ataque y correr si sale de todos los radios
        if (atacando || persiguiendo)
        {
            anim.ResetTrigger("Golpear");
            anim.ResetTrigger("Correr");
            anim.SetBool("Quieto", true);
            atacando = false;
            persiguiendo = false;
        }
    }

    void MirarJugador()
    {
        if (jugador != null && (atacando || persiguiendo))
        {
            // Calcula la direcci�n hacia el jugador
            Vector3 direccion = (jugador.position - transform.position).normalized;
            direccion.y = 0; // Mantiene la rotaci�n en el plano horizontal

            // Rotaci�n instant�nea hacia el jugador
            transform.rotation = Quaternion.LookRotation(direccion);
        }
    }

    // Visualizaci�n en el Editor de los radios de detecci�n y ataque
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioAtaque);
    }
}