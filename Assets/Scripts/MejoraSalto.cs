using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MejoraSalto : MonoBehaviour
{
    public float saltoLargo = 1.5f; // Multiplicador para aumentar la gravedad cuando el jugador cae
    public float saltoCorto = 1f;   // Multiplicador para aumentar la gravedad si suelta el botón de salto antes de tiempo

    Rigidbody2D rb;

    // Se ejecuta al iniciar el script
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtiene el componente Rigidbody2D del jugador
    }

    // Se ejecuta una vez por fotograma
    void Update()
    {
        // Si el jugador está cayendo (velocidad negativa en Y)
        if (rb.linearVelocity.y < 0)
        {
            // Aumenta la gravedad para caer más rápido
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * saltoLargo * Time.deltaTime;
        } 
        // Si el jugador está subiendo pero suelta el botón de salto
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            // Aumenta la gravedad para que el salto sea más corto
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * saltoCorto * Time.deltaTime;
        }
    }
}
