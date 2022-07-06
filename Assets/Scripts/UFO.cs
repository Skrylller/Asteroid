using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    private int _moveDirectional;

    [SerializeField] private Vector2 _shootDelay;
    private bool _canShoot;

    [SerializeField] private int _scoreForDestroy;
    [SerializeField] private AudioClip _destroyClip;
    [SerializeField] private AudioClip _shootClip;


    private void OnEnable()
    {
        _canShoot = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet")
        {
            collision.GetComponent<Bullet>().Deactivate();
            DeactivateObject();
        }

        if (collision.tag == "Asteroid" || collision.tag == "Player")
        {
            DeactivateObject();
        }
    }

    private void Update()
    {
        Shoot();
    }
    private void FixedUpdate()
    {
        Move();
    }

    public void SetDirectional(int directional)
    {
        _moveDirectional = directional >= 0 ? 1 : -1;
    }

    public void DeactivateObject()
    {
        GameplayController.main.PlusScore(_scoreForDestroy);
        UIController.main.UpdateText();

        Generator.main.GenerateUFO();

        gameObject.SetActive(false);
        SoundController.main.PlayClip(_destroyClip);
    }


    private void Move()
    {
        transform.Translate(_moveSpeed * _moveDirectional * Time.fixedDeltaTime, 0, 0, Space.World);
    }
    private void Shoot()
    {
        if (_canShoot == false || PlayerController.main == null)
            return;

        Vector2 aim = PlayerController.main.transform.localPosition;
        float atan = (aim.x - transform.localPosition.x) / (aim.y - transform.localPosition.y);
        float angle = -Mathf.Atan(atan) * Mathf.Rad2Deg;
        if (aim.y - transform.position.y <= 0)
            angle -= 180;

        ObjectsPull.main.UFOBullets.ActivateObject(transform.position, angle);
        SoundController.main.PlayClip(_shootClip);

        StartCoroutine(ShootTimer());
    }

    private IEnumerator ShootTimer()
    {
        _canShoot = false;
        yield return new WaitForSeconds(Random.Range(_shootDelay.x, _shootDelay.y));
        _canShoot = true;
    }
}
