using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] Vector3 direction;
    public Rigidbody rb;
    float maxActivationTime = 2f;
    float activationTime = 0f;
    [SerializeField] GameObject target;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        activationTime += Time.deltaTime;
        if (activationTime < maxActivationTime && target!=null)
        {
            //rb.AddForce(direction * Speed * Time.deltaTime, ForceMode.Acceleration);
            //transform.Translate(Direction * Speed * Time.deltaTime);
            transform.LookAt(target.GetComponent<Collider>().bounds.center);
            rb.AddForce(transform.forward*Speed*Time.deltaTime);
        }
        else
        {
           DeActivate();
        }
    }

    public void SetDirection(Vector3 _direction,Vector3 playerVelocity)
    {
        rb.isKinematic = false;
       // rb.velocity = playerVelocity;
        this.direction = _direction;
        //chnagind direction height wise is not really needed
        direction.y = 0f;
        transform.LookAt(direction);
    }

    public void LockTarget(GameObject _target,Vector3 initialVelocity)
    {
        rb.velocity = initialVelocity;
        target = _target;
    }

    public void DeActivate()
    {
        activationTime = 0f;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        GameManager.gameInstance.projectilePool.Remove(transform.root.gameObject);
        Destroy(transform.root.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponentInChildren<PlayerManager>())
        {
            GameManager.gameInstance.playerManager.DealDamage(25);
        }
    }
}
