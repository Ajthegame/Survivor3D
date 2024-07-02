using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    PlayerManager _playerManager;
    [SerializeField]
    public PlayerManager playerManager
    {  get 
        {
            if (_playerManager == null)
                _playerManager = FindObjectOfType<PlayerManager>();
            return _playerManager;
        }
        set 
        {
            _playerManager = value;   
        }
    }

    [SerializeField]
    GameObject enemyPrefab;

    int _coinsCollected;
    public int coinsCollected
    {
        get
        {
            return _coinsCollected;
        }

        set
        {
            _coinsCollected = value;
            UIManager.uiManager.UpdatePlayerStats(_coinsCollected);
        }
    }
    int _enemiesKilled;
    public int enemiesKilled
    {
        get
        {
            return _enemiesKilled;
        }

        set
        {
            _enemiesKilled = value;
            UIManager.uiManager.UpdatePlayerStats(-1,_enemiesKilled);
        }
    }
    
    public GameObject coinPrefab;

    /// <summary>
    /// playtime to increase difficulty or seed
    /// </summary>
    [SerializeField]
    float playTime;

    [SerializeField]
    List<GameObject> activeEnemyList;
    [SerializeField]
    List<GameObject> passiveEnemyList;

    public List<GameObject> projectilePool;
    [SerializeField] int projectilePoolCount = 5;

    public static GameManager gameInstance;
    // Start is called before the first frame update
    void Awake()
    {
        if (gameInstance == null)
        {
            gameInstance = this;
        }

        _playerManager = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        playTime += Time.deltaTime;
        if(Random.Range(0,100) == 3 && activeEnemyList.Count < 100)
        EnemySpawner(Random.Range(0, 7));
    }

    void EnemySpawner(int seed)
    {
        GameObject obj;
        for (int i = 0; i < seed; i++)
        {
            if (passiveEnemyList.Count > 0)
            {
                obj = passiveEnemyList.First<GameObject>();
                if (obj != null)
                {
                    obj.SetActive(true);
                    passiveEnemyList.Remove(obj);
                }
            }
            else
            {
                obj = Instantiate(enemyPrefab);
            }
                activeEnemyList.Add(obj);
            obj.GetComponent<Enemy>().SetInitialParameters((EnemyType)Random.Range(0, 2), GetAvailablePositionForSpawn());
        }
    }

    Vector3 GetAvailablePositionForSpawn()
    {
        float radius = playerManager.proximityRadius;
        float maxRadius = radius * 2f;
        float x = Random.Range(-1f,1f);
        float z = Random.Range(-1f,1f);

        Vector2 pos = new Vector2(x, z);
        pos.Normalize();
        pos*= Random.Range(radius, maxRadius);

        return new Vector3(pos.x, 0, pos.y);
    }

    public void DeactivateEnemy(GameObject enemyObj)
    {
        if(activeEnemyList.Contains(enemyObj))
        {
            activeEnemyList.Remove(enemyObj);
            passiveEnemyList.Add(enemyObj);
        }
        if(playerManager.enemyList.Contains(enemyObj))
        {
            playerManager.enemyList.Remove(enemyObj);
        }
    }

    public void ResetGame()
    {
        coinsCollected = 0;
        enemiesKilled = 0;

        for (int i = 0; i < activeEnemyList.Count; i++)
        {
            Destroy(activeEnemyList[i]);
            activeEnemyList.RemoveAt(i);
        }

        for (int i = 0; i < passiveEnemyList.Count; i++)
        {
            Destroy(passiveEnemyList[i]);
            passiveEnemyList.RemoveAt(i);
        }

        playerManager.health = 100;
    }

    public void GameOver()
    {
        UIManager.uiManager.DisplayPopUp(UIManager.uiManager.gameOverPanel);
        ResetGame();
        playTime = 0;
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        UIManager.uiManager.DisplayPopUp();
    }
}
