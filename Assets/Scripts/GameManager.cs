using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager _UI_Manager;
    [SerializeField] private Player _Player;
    [SerializeField] private Enemy _Enemy;
    [SerializeField] private float _SpawnEveryThisSeconds = 3f;
    [SerializeField] private float _SpawnsToDoubleSpeed = 20;
    private float _Timer = 0f;

    [SerializeField] private Collider _Collider;

    private Edges _edges;

    void Awake()
    {
        if (_Collider == null) _Collider = gameObject.GetComponent<Collider>();
        if (_Player == null) _Player = Resources.FindObjectsOfTypeAll<Player>()[0];
        if (_Enemy == null) _Enemy = Resources.FindObjectsOfTypeAll<Enemy>()[0];
        if (_UI_Manager == null) _UI_Manager = GameObject.FindObjectOfType<UIManager>();
    }

    void Start()
    {
        float horizontalDistance = _Collider.bounds.extents.z;
        float verticalDistance = _Collider.bounds.extents.y;
        _edges = new Edges(
            Top: _Collider.bounds.center.y + verticalDistance,
            Right: _Collider.bounds.center.z + horizontalDistance,
            Bottom: _Collider.bounds.center.y - verticalDistance,
            Left: _Collider.bounds.center.z - horizontalDistance,
            Middle: _Collider.bounds.center.x
        );
        Vector3 playerStartPosition = new Vector3(_edges.Middle, _edges.Bottom, _Collider.bounds.center.z);
        Instantiate(_Player, playerStartPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        _Timer += Time.deltaTime;
        if (_Timer >= _SpawnEveryThisSeconds)
        {
            _Timer = 0f;
            SpawnEnemy();
            Time.timeScale += 1f / _SpawnsToDoubleSpeed;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(_edges.Middle, _edges.Top, Random.Range(_edges.Left, _edges.Right));
        Enemy enemy = Instantiate(_Enemy, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
        enemy.SetDestroyAt(_edges.Bottom);
        enemy.SetBottomCallback(EnemyReachedBottom);
        enemy.SetDeathCallback(EnemyWasDestroyed);
    }

    private void EnemyReachedBottom(bool hitPlayer = false)
    {
        _UI_Manager.DecreaseHealth(hitPlayer);
    }

    private void EnemyWasDestroyed()
    {
        _UI_Manager.IncreaseScore();
    }

    public void OnGameReset()
    {
        Time.timeScale = 1.0f;
        foreach (Enemy enemy in GameObject.FindObjectsOfType<Enemy>())
        {
            Destroy(enemy.gameObject);
        }
    }

    private struct Edges
    {
        public Edges(float Top, float Right, float Bottom, float Left, float Middle)
        {
            this.Top = Top;
            this.Right = Right;
            this.Bottom = Bottom;
            this.Left = Left;
            this.Middle = Middle;
        }

        public float Top { get; }
        public float Right { get; }
        public float Bottom { get; }
        public float Left { get; }
        public float Middle { get; }
    }
}
