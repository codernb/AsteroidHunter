using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour {

    private int Life;

	void Start () {
        Life = 20;
	}

    public void Hit(Player origin)
    {
        Life -= 1;
        if (Life < 1)
            StartCoroutine(Respawn(origin));
    }

    private IEnumerator Respawn(Player origin)
    {
        origin.givePoints(10, false);
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(10);
        Life = 20;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
    }

}
