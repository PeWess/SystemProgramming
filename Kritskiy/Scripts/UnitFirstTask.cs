using System.Collections;
using UnityEngine;

public class UnitFirstTask : MonoBehaviour
{
    [SerializeField] [Range(1, 100)] private int _health;

    private Coroutine _healingCoroutine;

    private void Start()
    {
        if (_health != 100)
        {
            _healingCoroutine = StartCoroutine(HealthRegen());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && _health != 100)
        {
            ReceiveHealing();
        }
    }

    public void ReceiveHealing()
    {
        StopCoroutine(_healingCoroutine);
        _healingCoroutine = StartCoroutine(HealthRegen());
    }

    IEnumerator HealthRegen()
    {
        Debug.Log("Start healing...");
        float regenTime = 3f;
        while (_health < 100 && regenTime != 0)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            regenTime -= 0.5f;
            _health += 5;
            if (_health >= 100) _health = 100;
            Debug.Log($"Current health is {_health}");
        }
    }
}