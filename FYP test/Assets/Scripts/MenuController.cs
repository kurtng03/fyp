using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {
    public GameObject StartButton;
    public GameObject MultiplayerButton;
    public GameObject PracticeButton;

	// Use this for initialization
	void Awake () {
        MultiplayerButton.SetActive(false);
        PracticeButton.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

  public   void Destroy()
    {
        Destroy(StartButton);
        MultiplayerButton.SetActive(true);
        PracticeButton.SetActive(true);
    }

    public void Practice()
    {
        Application.LoadLevel("test");
    }
}
