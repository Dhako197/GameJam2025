using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MenuPerdiste : MonoBehaviour
{
    [FormerlySerializedAs("perdisteMenuUI")] public GameObject perdisteMenuUI;

    void Update()
    {
        
    }
    
    public void ShowMenu()
    {
        perdisteMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        perdisteMenuUI.SetActive(false);
        Time.timeScale = 1f;
        
    }


    void Pause()
    {
        perdisteMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }


    public void Renaudar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); //Se pone primer escena del juego
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}