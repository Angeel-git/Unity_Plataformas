using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad; 
    public float velocidadMax;

    private Rigidbody2D rPlayer;
    private float h;

    private bool miraDerecha = true;

    void Start(){
        rPlayer = GetComponent<Rigidbody2D>();
    }

    void Update(){
        giraPlayer(h); 
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
