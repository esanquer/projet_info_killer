using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Transform team_orange;
    public Transform team_bleue;
    public Transform objectCreator;
    public Transform camera;

    public RectTransform menuTeamBlue;
    public RectTransform menuTeamOrange;

    public RectTransform playButton;
    public RectTransform stopButton;
    public RectTransform quitButton;
    public RectTransform gameOverPanel;
    public RectTransform winnerBleu;
    public RectTransform winnerOrange;
    bool play = false;
    // Use this for initialization
    void Start () {
        stopButton.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        play = false;
        camera.GetComponent<cameraControl>().Active = false;
    }


	public void LancerJeu()
    {
        camera.GetComponent<cameraControl>().Active = true;
        Cursor.visible = false;
        menuTeamBlue.gameObject.SetActive(false);
        menuTeamOrange.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(true);
        gameOverPanel.gameObject.SetActive(false);
        winnerOrange.gameObject.SetActive(false);
        winnerBleu.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        team_bleue.gameObject.GetComponent<TeamCreator>().Demarrer();
        team_orange.gameObject.GetComponent<TeamCreator>().Demarrer();
        objectCreator.gameObject.GetComponent<RandomObjectsCreator>().demarrer();

        play = true;
    }
    public void arreterJeu()
    {
        play = false;
        camera.GetComponent<cameraControl>().Active = false;
        Cursor.visible = true;
        team_bleue.gameObject.GetComponent<TeamCreator>().arreter();
        team_orange.gameObject.GetComponent<TeamCreator>().arreter();
        objectCreator.gameObject.GetComponent<RandomObjectsCreator>().arreter();
        menuTeamBlue.gameObject.SetActive(true);
        menuTeamOrange.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        winnerOrange.gameObject.SetActive(false);
        winnerBleu.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(true);


    }
    // Update is called once per frame
    void Update() {
        if (play) {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                arreterJeu();
            }
            if (team_bleue.childCount == 0 && team_bleue.gameObject.GetComponent<TeamCreator>().temmatesList.Count == 0)
            {
                arreterJeu();
                gameOverPanel.gameObject.SetActive(true);
                winnerOrange.gameObject.SetActive(true);
            }
            if (team_orange.childCount == 0 && team_orange.gameObject.GetComponent<TeamCreator>().temmatesList.Count == 0)
            {
                arreterJeu();
                gameOverPanel.gameObject.SetActive(true);
                winnerBleu.gameObject.SetActive(true);
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
    public void quit()
    {
        Application.Quit();
    }
}
