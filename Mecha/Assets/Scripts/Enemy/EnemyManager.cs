using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Enemy enemyRegularPrefab;
    [SerializeField] private Enemy[] enemySpecialPrefabs;
    public Transform player;
    public new Camera camera;
    [SerializeField] private Transform enemyDeathVisual;
    public float minSpawnHeight;
    public float sideOfScreenMaxOffset;
    public float spawnRangeUp;
    public float spawnRangeDown;
    public float spawnRangeHorizontal;
    public float lowSpawnRangeHeight;
    public int enemiesInRangeLow;
    public float firingRange;

    private DifficultySettings _difficultySettings;
    private Enemy[] _enemies;
    private static Dictionary<Collider2D, Enemy> _enemyOfCollider = new Dictionary<Collider2D, Enemy>();
    private float _timePlaying;
    private float _currentGeneralTier;
    private float _timeLastSpawn;
    private Vector2 _screenWorldSize;
    private Vector2 _position;
    private int _direction;
    private int _sideOfScreen;
    private Queue<Enemy> _enemiesOutOfRange = new Queue<Enemy>();
    private int _currentEnemiesInRangeLow;
    private int _tierMin;
    private static Enemy _enemy;

    void Awake()
    {
        Player.Attack.AddListener(OnAttack);
        Player.FinishAttack.AddListener(OnFinishAttack);

        _difficultySettings = GameManager.I.GetDifficultySettings();
        _screenWorldSize = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) - 
                           camera.ScreenToWorldPoint(Vector2.zero);
    }

    void Start()
    {
        _enemies = CreateEnemies();
        
        for (int i = 0; i < _enemies.Length; i++)
        {
            SpawnEnemy(_enemies[i]);
        }
    }
    
    void Update()
    {
        _timePlaying += Time.deltaTime;
        _currentGeneralTier = -0.5f + _timePlaying / _difficultySettings.TimeToMaxTier * _difficultySettings.AmountTiers;

        _currentEnemiesInRangeLow = 0;
        foreach (Enemy enemy in _enemies)
        {
            if (IsInSpawnRange(enemy.Transform.position))
            {
                enemy.SetFiringEnabled(IsInFiringRange(enemy.Transform.position.y));
                
                _currentEnemiesInRangeLow += IsInSpawnRangeLow(enemy.Transform.position) ? 1 : 0;

                continue;
            }

            _enemiesOutOfRange.Enqueue(enemy);
        }

        for (int i = _enemiesOutOfRange.Count - 1; i >= 0; i--)
        {
            SpawnEnemy(_enemiesOutOfRange.Dequeue());
        }
    }

    public static Enemy GetEnemy(Collider2D collider)
    {
        _enemyOfCollider.TryGetValue(collider, out _enemy);
        return _enemy;
    }
    
    private void OnAttack(Enemy enemy)
    {
        enemyDeathVisual.gameObject.SetActive(true);
        enemyDeathVisual.position = enemy.transform.position;
        StartCoroutine(RespawnEnemy(enemy));
    }
    
    private void OnFinishAttack()
    {
        enemyDeathVisual.gameObject.SetActive(false);
    }
    
    private void SpawnEnemy(Enemy enemy)
    {
        if (IsPlayerNearGround() && _currentEnemiesInRangeLow < enemiesInRangeLow)
        {
            _position = GetPositionInLowSpawnRange();
            _currentEnemiesInRangeLow++;
        }
        else
        {
            _position = GetPositionInSpawnRange();
        }
        
        _direction = Random.Range(0, 1f) > 0.5f ? 1 : -1;
        if (IsOnScreen(_position))
        {
            _sideOfScreen = _position.x - player.position.x < 0 ? -1 : 1;
            _position = new Vector2(GetEnterScreenPosition(_sideOfScreen, enemy.SpriteRenderer), _position.y);
            _direction = -_sideOfScreen;
        }

        enemy.Transform.position = _position;
        enemy.SetHidden(_position.y < minSpawnHeight);
        enemy.SetTier(GenerateTier());
        enemy.SetDirection(_direction);
    }

    private IEnumerator RespawnEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        yield return null;
        enemy.gameObject.SetActive(true);
        SpawnEnemy(enemy);
    }

    private Enemy[] CreateEnemies()
    {
        Enemy[] enemies = new Enemy[_difficultySettings.AmountExistEnemy];
        Enemy enemy;
        int indexEnemySpecial;
        for (int i = 0; i < enemies.Length; i++)
        {
            indexEnemySpecial = i / _difficultySettings.AmountExistPerEnemySpecial;
            enemy = Instantiate(indexEnemySpecial < enemySpecialPrefabs.Length ? 
                enemySpecialPrefabs[indexEnemySpecial] : 
                enemyRegularPrefab);
            
            enemies[i] = enemy;
            _enemyOfCollider[enemy.GetCollider()] = enemy;
        }

        return enemies;
    }
    
    private int GenerateTier()
    {
        _tierMin = Mathf.Max(Mathf.FloorToInt(_currentGeneralTier), 0);
        return Mathf.Min(_tierMin + (Random.Range(0, 1f) < _currentGeneralTier - _tierMin ? 
            1 : 0), _difficultySettings.AmountTiers - 1);
    }

    private bool IsInSpawnRange(Vector2 position)
    {
        return position.y > player.position.y - spawnRangeDown &&
               position.y < player.position.y + spawnRangeUp &&
               position.x > player.position.x - spawnRangeHorizontal &&
               position.x < player.position.x + spawnRangeHorizontal;
    }

    private bool IsInSpawnRangeLow(Vector2 position)
    {
        return position.y < minSpawnHeight + lowSpawnRangeHeight && 
            position.y > minSpawnHeight;
    }

    private bool IsOnScreen(Vector2 position)
    {
        return position.y > camera.transform.position.y - _screenWorldSize.y / 2 &&
               position.y < camera.transform.position.y + _screenWorldSize.y / 2 &&
               position.x > camera.transform.position.x - _screenWorldSize.x / 2 &&
               position.x < camera.transform.position.x + _screenWorldSize.x / 2;
    }
    
    private bool IsInFiringRange(float height)
    {
        return height < player.position.y + firingRange &&
               height > player.position.y - firingRange;
    }

    private Vector2 GetPositionInSpawnRange()
    {
        return new Vector2(player.position.x + Random.Range(-spawnRangeHorizontal, spawnRangeHorizontal), 
            player.position.y + Random.Range(-spawnRangeDown, spawnRangeUp));
    }

    private Vector2 GetPositionInLowSpawnRange()
    {
        return new Vector2(player.position.x + Random.Range(-spawnRangeHorizontal, spawnRangeHorizontal),
            minSpawnHeight + Random.Range(0, lowSpawnRangeHeight));
    }

    private float GetEnterScreenPosition(int sideOfScreen, SpriteRenderer enemyRenderer)
    {
        return camera.transform.position.x + (_screenWorldSize.x / 2 + enemyRenderer.bounds.size.x / 2 + 
               Random.Range(0, sideOfScreenMaxOffset)) * sideOfScreen;
    }

    private bool IsPlayerNearGround()
    {
        return player.position.y < minSpawnHeight;
    }
}