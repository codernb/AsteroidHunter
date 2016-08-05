using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

    public Transform cam;

    private static Vector3 scale = new Vector3(10f, 1f, 10f);

    void FixedUpdate()
    {
        transform.localScale = scale * cam.position.y / 200f;
    }

}
