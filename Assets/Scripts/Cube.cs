using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cube : MonoBehaviour
{
    [SerializeField] private int _lifeTimeMin = 2;
    [SerializeField] private int _lifeTimeMax = 5;
    [SerializeField] private Color _baseColor = Color.white;

    private Spawner _spawner;
    private Material _material;
    private bool _isTouched = false;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(1);

    private void Awake()
    {
        _spawner = FindObjectOfType<Spawner>();
        _material = gameObject.GetComponent<MeshRenderer>().material;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_isTouched == true)
            return;

        if (other.gameObject.TryGetComponent<Platform>(out Platform _) == false)
            return;

        _isTouched = true;
        _material.color = Random.ColorHSV();
        StartCoroutine(LifeTiming(Random.Range(_lifeTimeMin, _lifeTimeMax + 1)));
    }

    private void ResetState()
    {
        _isTouched = false;
        _material.color = _baseColor;
    }

    private IEnumerator LifeTiming(int lifeTime)
    {
        for (int i = 0; i < lifeTime; i++)
        {
            yield return _waitForSeconds;
        }

        _spawner.DestroyCube(this);
        ResetState();
    }
}