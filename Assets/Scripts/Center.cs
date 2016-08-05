using UnityEngine;

public class Center : MonoBehaviour {

    public Transform cam;
    public float minCamDist = 30f;
    public float maxCamDist = 100f;
    public GameObject[] players;
	
	void Update () {
        Vector3 sum = Vector3.zero;
        Vector3 pos;
        foreach(GameObject player in players)
            sum += player.transform.position;
        if (players.Length > 0)
            pos = sum / players.Length;
        else
            pos = sum;
        transform.position = pos;
        float max = minCamDist;
        foreach (GameObject player in players)
            max = Mathf.Max(max, (pos - player.transform.position).magnitude);
        Vector3 camPos = cam.localPosition;
        camPos.y = Mathf.Min(maxCamDist, max);
        cam.localPosition = camPos;
	}
}
