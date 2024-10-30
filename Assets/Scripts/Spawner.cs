using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private BoxCollider _platform;
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _spawnPositionY = 25;
    [SerializeField] private int _maxCubeCount = 10;

    private ObjectPool<Cube> _objectPool;

    public void DestroyCube(Cube cube)
    {
        _objectPool.Release(cube);
    }
    
    private void Awake()
    {
        _objectPool = new ObjectPool<Cube>(
            CreateCube,
            ActionOnGet,
            (cube) => cube.enabled = false,
            Destroy,
            true,
            _maxCubeCount,
            _maxCubeCount
        );
    }

    private void Update()
    {
        if (_objectPool.CountActive < _maxCubeCount)
            _objectPool.Get();
    }

    private void ActionOnGet(Cube cube)
    {
        Vector3 boundsMin = _platform.bounds.min;
        Vector3 boundsMax = _platform.bounds.max;
        Vector3 startPosition = new Vector3(
            Random.Range(boundsMin.x, boundsMax.x),
            _spawnPositionY,
            Random.Range(boundsMin.z, boundsMax.z)
        );
        cube.ResetCube(startPosition);
    }

    private Cube CreateCube()
    {
        Cube cube = Instantiate(_cubePrefab);
        cube.SetSpawner(this);
        cube.enabled = false;
        return cube;
    }
}