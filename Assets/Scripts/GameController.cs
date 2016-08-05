using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject[] spawners = new GameObject[4];
    public GameObject[] compasses = new GameObject[4];
    public GameObject[] gates = new GameObject[4];
    public Text[] names = new Text[4];
    public InputField[] nameInputs = new InputField[4];
    public ScoreBorad scoreBoard;
    public GameObject orb;
    public Center center;
    public float gameTime = 180f;
    public Text time;
    public InputField input;
    public GameObject menu;

    private GameObject[] activeSpawners;
    private float endTime;
    private bool gameOver = true;

    void Start()
    {
        int nActive = 0;

        for (int i = 0; i < 4; i++)
        {
            if (GamePad.GetState((PlayerIndex) i).IsConnected)
            {
                spawners[i].SetActive(true);
                compasses[i].SetActive(true);
                gates[i].SetActive(true);
                nameInputs[i].gameObject.SetActive(true);
                nActive++;
            }
        }

        activeSpawners = new GameObject[nActive];

        int j = 0;

        foreach (GameObject spawner in spawners)
        {
            if (spawner.activeSelf)
            {
                activeSpawners[j++] = spawner.gameObject;
            }
        }


    }

    public void Go()
    {
        gameTime = float.Parse(input.text) * 60f;
        endTime = Time.time + gameTime;
        center.players = activeSpawners;
        scoreBoard.Go(spawners);
        var i = 0;
        foreach (GameObject spawner in activeSpawners)
        {
            spawner.GetComponent<PlayerSpawner>().Go();
            names[i].text = nameInputs[i++].text;
        }
        gameOver = false;
    }

    public void Stop()
    {
        foreach (GameObject spawner in spawners)
        {
            spawner.GetComponent<PlayerSpawner>().Stop();
        }
        menu.SetActive(true);
    }

    void OnGUI()
    {
        int delta = (int) (endTime - Time.time);
        if (delta < 0)
        {
            if (!gameOver)
            {
                gameOver = true;
                Stop();
            }
            time.text = "0:0";
            return;
        }
        int minutes = delta / 60;
        int seconds = delta % 60;
        time.text = minutes + ":" + seconds;
    }

}
