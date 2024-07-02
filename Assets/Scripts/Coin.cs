using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    float lifeTime = 10f;

    float currentAge;

    [SerializeField]
    float rotationAngle = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAge < lifeTime)
        {
            currentAge += Time.deltaTime;
            transform.Rotate(Vector3.up, rotationAngle);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInChildren<PlayerManager>() && other.GetType() == typeof(BoxCollider))
        {
            GameManager.gameInstance.coinsCollected++;
            Destroy(gameObject);
        }
    }
}
