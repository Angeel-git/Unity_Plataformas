using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject ObjetoMenuPausa;
    private bool Pausa = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Presionar "Esc" para pausar
        {
            if (!Pausa)
            {
                ObjetoMenuPausa.SetActive(true);
                Pausa = true;
                Time.timeScale = 0f;    //Para dejar el juego en pausa
            }else{
                Resumir();  //Si el juego ya esta en pausa el ESC puede salir del menu pausa 
            }
        }
    }

    public void Resumir()
    {
        ObjetoMenuPausa.SetActive(false);
        Pausa = false;
        Time.timeScale = 1f;    //Para quitar pa pausa del juego
    }

    public void irMenu(string nombreMenu)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreMenu);
    }

    public void SalirJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
