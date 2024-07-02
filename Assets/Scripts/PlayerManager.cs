using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    public Image healthBar;
    float _health = 100;
    public float health
    {  get 
        { return _health; }
        set 
        {
            _health = value;
            healthBar.fillAmount = _health/100;
            if(health <= 0 )
            {
                GameManager.gameInstance.GameOver();
            }
        }
    }
    int coins;
    [SerializeField]float coolTime;
    float coolDownPeriod;

    [SerializeField] GameObject projectilePrefab;

    /// <summary>
    /// the radius to detect enemies and take a shot
    /// </summary>
    [SerializeField] float _proximityRadius;

    public float proximityRadius
    {
        get 
        { return _proximityRadius; }
        set 
        { _proximityRadius = value; }
    }
    
    /// <summary>
    /// all enemies which entered the player radius
    /// </summary>
    public List<GameObject> enemyList;

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < projectilePoolCount; i++)
        //{
        //    projectilePool.Add(GameObject.Instantiate(projectilePrefab));
        //    projectilePool[i].SetActive(false);
        //}

        if (GetComponent<SphereCollider>())
            GetComponent<SphereCollider>().radius = proximityRadius;
    }

    // Update is called once per frame
    void Update()
    {
        coolDownPeriod = coolDownPeriod > 0?coolDownPeriod-Time.deltaTime:0;   
        if (coolDownPeriod <= 0 && enemyList.Count()>0 && GameManager.gameInstance.projectilePool.Count<5)
        {
            GameObject obj = Instantiate(projectilePrefab);
            GameManager.gameInstance.projectilePool.Add(obj);
            ShootProjectie(ClosestEnemy(), obj);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
            enemyList.Add(other.GetComponent<Collider>().gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if(Vector3.Distance(other.gameObject.transform.position,transform.position) > proximityRadius)
        enemyList.Remove(other.GetComponent<Collider>().gameObject);
    }

    void ShootProjectie(GameObject closest, GameObject projectile)
    {
        coolDownPeriod += coolTime;
        if (closest)
        {
            // Debug.DrawRay(transform.position, projectile.transform.position,Color.green,10f);
            projectile.gameObject.transform.position = this.transform.position;
            if (projectile.GetComponentInChildren<Projectile>())
            {
                //projectile.GetComponent<Projectile>().SetDirection(DirectionbwCColliders(closest.GetComponentInChildren<Collider>(), projectile.gameObject.GetComponentInChildren<Collider>()), this.GetComponent<Rigidbody>().velocity);
                projectile.GetComponentInChildren<Projectile>().LockTarget(closest, this.GetComponent<Rigidbody>().velocity);
            }
        }
    }

    /// <summary>
    /// dont think this level of detail is required if i have time i will do it later
    /// </summary>
    /// <returns></returns>
    GameObject ClosestEnemy()
    {
        if (enemyList.Count > 0)
        {
            GameObject closest = null;
            int index = 0;
            float minDist = proximityRadius;
            for(int i=0;i<enemyList.Count;i++)
            {
                if (!enemyList[i]) continue;

                float currentDist = (enemyList[i].transform.position - transform.position).magnitude;
                if (currentDist < minDist)
                {
                    minDist = currentDist;
                    index = i;
                }
            }
            closest = enemyList[index];
            //enemyList.Remove(first);
            return closest;
        }
        else return null;
    }

    //GameObject FindAvailableProjectile()
    //{
    //    for (int i = 0; i < projectilePool.Count(); i++)
    //    {
    //        if (!projectilePool[i].activeSelf)
    //        {
    //            GameObject curObject = projectilePool[i];
    //            curObject.SetActive(true);
    //            curObject.GetComponent<Projectile>().rb.isKinematic = false;
    //            curObject.GetComponent<Projectile>().rb.velocity = Vector3.zero;
    //            curObject.GetComponent<Projectile>().rb.angularVelocity = Vector3.zero;
    //            curObject.GetComponent<Projectile>().rb.MovePosition(transform.position);
    //            return projectilePool[i];
    //        }
    //    }
    //    return null;
    //}

    Vector3 DirectionbwCColliders(Collider from , Collider to)
    {
        Vector3 dir = (from.bounds.center - to.bounds.center);
        dir = dir / dir.magnitude;
        return dir;
    }

    public void DealDamage(int damage)
    {
        health -= damage;
    }
}
