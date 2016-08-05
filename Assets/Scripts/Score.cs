using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {

    public PlayerSpawner spawner;
    public Text text;

    void OnGUI()
    {
        text.text = "Score: " + spawner.currentPoints
        + "\nBest Score: " + spawner.pointsMax
        + "\nTotal Score: " + spawner.pointsSum;
    }

}
