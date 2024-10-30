using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private int _lifeTimeMin = 2;
    [SerializeField] private int _lifeTimeMax = 5;
    [SerializeField] private Color _baseColor = Color.white;

    private Spawner _spawner;
    private Material _material;
    private bool _isTouchedPlatform;
    private WaitForSeconds _waitTime;

    private void Awake()
    {
        _isTouchedPlatform = false;
        _waitTime = new WaitForSeconds(1);
        _material = GetComponent<MeshRenderer>().material;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_isTouchedPlatform == true)
            return;

        if (other.gameObject.TryGetComponent<Platform>(out Platform _) == false)
            return;

        _isTouchedPlatform = true;
        _material.color = Random.ColorHSV();
        StartCoroutine(WaitToDestroying(Random.Range(_lifeTimeMin, _lifeTimeMax + 1)));
    }

    public void SetSpawner(Spawner spawner)
    {
        _spawner = spawner;
    }

    public void ResetCube(Vector3 startPosition)
    {
        _isTouchedPlatform = false;
        _material.color = _baseColor;
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        enabled = true;
    }

    private IEnumerator WaitToDestroying(int lifeTime)
    {
        for (int i = 0; i < lifeTime; i++)
        {
            yield return _waitTime;
        }

        _spawner.DestroyCube(this);
    }
}