using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;

    private void OnEnable()
    {
        StartCoroutine(DeactivateCourotine());
    }

    private void FixedUpdate()
    {
        Move(); 
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void Move()
    {
        transform.Translate(0, _speed * Time.fixedDeltaTime, 0, Space.Self);
    }

    private IEnumerator DeactivateCourotine()
    {
        yield return new WaitForSeconds(_lifeTime);
        Deactivate();
    }

}
