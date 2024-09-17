using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float speed = 5f;         // Velocidad de movimiento horizontal
    public float jumpHeight = 2f;    // Altura del salto
    public float gravity = 9.81f;    // Magnitud de la gravedad
    public float rotationSpeed = 700f; // Velocidad de rotaci�n
    public GameObject misilObject;   // Referencia al objeto que contiene el misil

    private CharacterController controller;
    private Animator animator;
    private Misil misilScript;       // Referencia al script del misil
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        // Obt�n el componente CharacterController
        controller = GetComponent<CharacterController>();
        // Obt�n el componente Animator
        animator = GetComponent<Animator>();
        // Obt�n el script del misil si existe
        if (misilObject != null)
        {
            misilScript = misilObject.GetComponent<Misil>();
        }
    }

    void Update()
    {
        // Verifica si el jugador est� en el suelo
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Un valor peque�o para mantener el personaje en el suelo
        }

        // Verifica el movimiento del jugador
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Verifica si el jugador est� movi�ndose
        bool isMoving = move != Vector3.zero;
        animator.SetBool("correr", isMoving);

        // Maneja el salto y la animaci�n de salto
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Aplica una velocidad vertical para el salto
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
            animator.SetTrigger("salto");
        }

        // Aplica la gravedad
        velocity.y -= gravity * Time.deltaTime;

        // Mueve al jugador
        controller.Move(move * speed * Time.deltaTime + velocity * Time.deltaTime);

        // Calcula la rotaci�n deseada en funci�n del movimiento
        if (isMoving)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Controles de combate
        HandleCombat();

        // Lanza el misil cuando se presiona la tecla "E"
        if (Input.GetKeyDown(KeyCode.E) && misilScript != null)
        {
            misilScript.LanzarMisil();
        }

        // Activa la animaci�n "Lanzo" cuando se presiona la tecla "Q"
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("Lanzo");
        }
    }

    void HandleCombat()
    {
        // Golpe normal (click izquierdo)
        if (Input.GetMouseButtonDown(0)) // Click izquierdo
        {
            animator.SetBool("golpear", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("golpear", false);
        }

        // Golpe especial (click derecho)
        if (Input.GetMouseButtonDown(1)) // Click derecho
        {
            animator.SetTrigger("preparando golpe");
        }
    }
}