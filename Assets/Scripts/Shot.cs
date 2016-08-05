using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

    public float speed = 20f;
    public float lifeTime = 2f;
    public Vector3 initSpeed;
    public float spread = 5f;
    public float damage = 5f;
    public Player origin;

	void Start () {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += Random.Range(-spread, spread);
        transform.rotation = Quaternion.Euler(rotation);
        GetComponent<Rigidbody>().velocity = transform.up * speed + initSpeed;
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shot")
            return;
        if (other.tag == "Player")
        { 
            other.GetComponent<PlayerCollider>().DealDamage(damage, origin);
            Destroy(gameObject);
        }
        if (other.tag == "Obstacle")
            Destroy(gameObject);
    }

}
