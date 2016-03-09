using UnityEngine;

using UnityEngine.EventSystems;


public class Menu : MonoBehaviour{

    public GameObject startMenu; 
    public GameObject menu;
    private bool isClicked = false;

    void start()
    {
        
        menu.SetActive(false);
    } 

    public void gameStart()
    {
        isClicked = true;
        // Application.LoadLevel("test");
        if (isClicked == true)
        {
            startMenu.SetActive(false);
            menu.SetActive(true);

        }

    }

    public void practice()
    {
        Application.LoadLevel("test");
    }

  public void quitGame()
    {
        Application.Quit();
    }

    public void clicked()
    {
        this.isClicked = true;
    }

}
