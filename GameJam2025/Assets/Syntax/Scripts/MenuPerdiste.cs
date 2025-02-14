using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MenuPerdiste : MonoBehaviour
{
    [FormerlySerializedAs("perdisteMenuUI")] public GameObject perdisteMenuUI;


    private void Start()
    {
        //perdisteMenuUI.SetActive(true);
        AudioController.Instance.SetVolume(0.3f);
    }
    
    public void ShowMenu()
    {
        perdisteMenuUI.SetActive(true);
    }

    public void Renaudar()
    {
        AudioController.Instance.SetVolume();
        SceneManager.LoadScene("FinalFinal"); //Se pone primer escena del juego
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuInicial");
    }
}