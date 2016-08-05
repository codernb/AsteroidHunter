using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreBorad : MonoBehaviour {

    public GameObject[] scores;
    public Text[] names;

    public void Go(GameObject[] playerSpawners)
    {
        for (int i = 0; i < playerSpawners.Length; i++)
            if (playerSpawners[i].activeSelf)
            {
                scores[i].SetActive(true);
                names[i].text = playerSpawners[i].GetComponent<PlayerSpawner>().playerName;
            }
    }

}
