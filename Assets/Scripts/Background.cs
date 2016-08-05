using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

    public GameObject tile;
    public int rows = 2;
    public int cols = 2;

	void Start () {
        for (int i = -rows; i <= rows; i++)
        {
            for (int j = -cols; j <= cols; j++)
            {
                Vector3 pos = transform.position;
                pos.x += tile.transform.localScale.x * i;
                pos.z += tile.transform.localScale.y * j;
                    GameObject tileObject = (GameObject) Instantiate(tile, pos, transform.rotation);
                    tileObject.transform.SetParent(transform);
            }
        }
	}
}
