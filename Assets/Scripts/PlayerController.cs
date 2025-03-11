using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad; 
    public float velocidadMax;
    public float fuerzaSalto;
    public bool colPies = false;

    private Rigidbody2D rPlayer;
    private float h;

    private bool miraDerecha = true;
    private bool colPlayer = false;

    void Start(){
        rPlayer = GetComponent<Rigidbody2D>();
    }

    void Update(){
        giraPlayer(h); 

        //Configuración del salto
        colPies = CheckGround.colPies;
        if (Input.GetButtonDown("Jump") && colPies)
        {
            Debug.Log("Está saltando/Pulsó el espacio");
            rPlayer.linearVelocity = new Vector2(rPlayer.linearVelocity.x, 0f);
            rPlayer.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        h = Input.GetAxisRaw("Horizontal");
        rPlayer.AddForce(Vector2.right * velocidad * h);
        float limiteVel = Mathf.Clamp(rPlayer.linearVelocity.x, -velocidadMax, velocidadMax);
        rPlayer.linearVelocity = new Vector2(limiteVel, rPlayer.linearVelocity.y);
    }

    void giraPlayer(float horizontal){
        if ((horizontal < 0 && miraDerecha) || (horizontal > 0 && !miraDerecha))
        {
            miraDerecha = !miraDerecha;
            Vector3 escalaGiro = transform.localScale;
            escalaGiro.x *= -1;
            transform.localScale = escalaGiro;
        }
    }
}
