using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Walkers,
    lazy
};


public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyType enemyType;
    [SerializeField]
    GameObject projectilePrefab;
    [SerializeField]
    float maxCoolTime = 2f;    
    [SerializeField]
    float tmesinceLastProjectile;
    [SerializeField]
    List<Material> materials = new List<Material>();
    Color oldColor;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    float speed = 3;
    [SerializeField]
    RuntimeAnimatorController idleController;
    [SerializeField]
    RuntimeAnimatorController walkingController;
    BoxCollider playerRefrence;
    Collider selfRefrence;

    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        playerRefrence = GameManager.gameInstance.playerManager.gameObject.GetComponentInChildren<BoxCollider>();
        selfRefrence = GetComponent<Collider>();
        GetComponentInChildren<SkinnedMeshRenderer>().GetMaterials(materials);
    }

    // Update is called once per frame
    void Update()
    {
        rb.MoveRotation(Quaternion.LookRotation(playerRefrence.bounds.center - selfRefrence.bounds.center).normalized);//.LookAt(playerRefrence.bounds.center);
        if (enemyType == EnemyType.Walkers)
        {
            rb.velocity = playerRefrence ? ((playerRefrence.bounds.center - selfRefrence.bounds.center).normalized * speed):Vector3.forward;
        }
        else if(enemyType == EnemyType.lazy)
        {
            if(tmesinceLastProjectile>maxCoolTime)
            {
                GameObject obj = Instantiate(projectilePrefab);
                obj.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
                ShootProjectie(obj);
            }
            else
            tmesinceLastProjectile += Time.deltaTime;
        }
    }

    public void SetInitialParameters(EnemyType _enemyType,Vector3 position)
    {
        enemyType = _enemyType;
        switch (enemyType)
        {
            case EnemyType.Walkers:
                GetComponentInChildren<Animator>().runtimeAnimatorController = walkingController;
                rb.velocity = Vector3.forward * speed;
                materials[0].SetColor("_Color", Color.white);
                rb.isKinematic = false;
                break;

            case EnemyType.lazy:
                GetComponentInChildren<Animator>().runtimeAnimatorController = idleController;
                GetComponentInChildren<SkinnedMeshRenderer>().GetMaterials(materials);
                materials[0].SetColor("_Color",Color.green);
                rb.isKinematic = true;
                break;
        }
        transform.position = position;
    }

    void MarkAsClosest()
    {
        //materials[0].color = Color.red;
        //isClosest = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Projectile>()!= null)
        {
            {
                GameManager.gameInstance.projectilePool.Remove(other.transform.root.gameObject);
                Destroy(other.transform.root.gameObject);//.GetComponent<Projectile>().DeActivate();
                Instantiate(GameManager.gameInstance.coinPrefab,GetComponent<Collider>().bounds.center,Quaternion.identity);
                GameManager.gameInstance.DeactivateEnemy(gameObject);
                GameManager.gameInstance.enemiesKilled++;
                this.gameObject.SetActive(false);
            }
        }
        else if(other.gameObject.GetComponentInChildren<PlayerManager>() && other.GetType() == typeof(BoxCollider))
        {
            other.gameObject.GetComponentInChildren<PlayerManager>().DealDamage(10);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.collider.gameObject.SetActive(false);
    }

    void ShootProjectie(GameObject projectile)
    {
        tmesinceLastProjectile = 0;
        if (playerRefrence)
        {
            projectile.gameObject.transform.position = this.transform.position;
            if (projectile.GetComponentInChildren<Projectile>())
            {
                //projectile.GetComponent<Projectile>().SetDirection(DirectionbwCColliders(closest.GetComponentInChildren<Collider>(), projectile.gameObject.GetComponentInChildren<Collider>()), this.GetComponent<Rigidbody>().velocity);
                projectile.GetComponentInChildren<Projectile>().LockTarget(playerRefrence.gameObject, Vector3.zero);
            }
        }
    }
}
