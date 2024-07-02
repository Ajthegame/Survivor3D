using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    Rigidbody body;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // body.Move(transform.up * speed * Time.deltaTime,Quaternion.identity);
        body.AddForce(transform.up*speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
