using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



using UnityEngine;

public class MenuFinPartida : MonoBehaviour
{
    public GameObject menuFinPartida;
    void Start()
    {
        menuFinPartida.SetActive(false);
    }


    public void MostrarMenu()
    {
        menuFinPartida.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   // Reinicia el nivel actual
    }

    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicial");
    }
}
