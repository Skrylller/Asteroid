using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController main;

    [Header("Movement")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _accelerationSpeed;
    [SerializeField] private float _rotationSpeed;
    private Vector2 _moveDirection;
    private bool isAcceleration;

    [Header("Shooting")]
    [SerializeField] private int _shootsPerSecond;
    private bool _canShoot;

    [Header("Image Settings")]
    [SerializeField] private GameObject _image;
    [SerializeField] private float _immortalDuration;
    private bool isImmortal;

    [Header("Sounds")]
    [SerializeField] private AudioSource _accelerationSound;
    [SerializeField] private AudioClip _shootClip;
    [SerializeField] private AudioClip _dammageClip;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        ResetPlayer();
    }

    private void Update()
    {
        if (isImmortal)
            ImageFlashing();

        AccelerationSound();
    }

    private void FixedUpdate()
    {        
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "UFOBullet")
        {
            collision.GetComponent<Bullet>().Deactivate();
            Dammage();
        }
        if(isImmortal == false && (collision.tag == "Asteroid" || collision.tag == "UFO"))
        {
            Dammage();
        }
    }

    public void Acceleration()
    {
        float xSpeed = _moveDirection.x + _accelerationSpeed * Mathf.Sin(transform.eulerAngles.z * -1 * Mathf.Deg2Rad) * Time.deltaTime;
        float ySpeed = _moveDirection.y + _accelerationSpeed * Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad) * Time.deltaTime;

        if (Mathf.Sqrt(Mathf.Pow(xSpeed, 2) + Mathf.Pow(ySpeed, 2)) > _maxSpeed)
        {
            float forceAngle = -Mathf.Atan(xSpeed / ySpeed) * Mathf.Rad2Deg;
            if (ySpeed <= 0)
                forceAngle -= 180;

            xSpeed = _maxSpeed * Mathf.Sin(forceAngle * -1 * Mathf.Deg2Rad);
            ySpeed = _maxSpeed * Mathf.Cos(forceAngle * Mathf.Deg2Rad);
        }
        _moveDirection = new Vector2(xSpeed, ySpeed);
        isAcceleration = true;
    }

    public void RightRotationPlayer()
    {
        RotationPlayer(-1);
    }

    public void LeftRotationPlayer()
    {
        RotationPlayer(1);
    }

    public void RotationToPoint(Vector2 aim)
    {
        float atan = (aim.x - transform.localPosition.x) / (aim.y - transform.localPosition.y);
        float angle = -Mathf.Atan(atan) * Mathf.Rad2Deg;

        if (aim.y - transform.position.y <= 0)
            angle += 180;
        else if (aim.x - transform.position.x >= 0)
            angle += 360;

        float firstSideAngle = Mathf.Abs(angle - transform.eulerAngles.z);
        float secondSideAngle = angle > transform.eulerAngles.z ? Mathf.Abs((360 - angle) + transform.eulerAngles.z) : Mathf.Abs(angle + (360 - transform.eulerAngles.z));
        if (firstSideAngle > secondSideAngle)
        {
            angle += angle > transform.eulerAngles.z ? -360 : 360;
        }

        //Debug.Log($"{angle}, {firstSideAngle}, {secondSideAngle}");

        RotationPlayer(angle - transform.eulerAngles.z);
    }

    public void Shoot()
    {
        if (_canShoot == false || Time.timeScale == 0)
            return;

        ObjectsPull.main.bullets.ActivateObject(transform.position, transform.eulerAngles.z);
        SoundController.main.PlayClip(_shootClip);
        StartCoroutine(ShootTimer());
    }

    public void ResetPlayer()
    {
        _canShoot = true;
        _moveDirection = new Vector2(0, 0);
        transform.position = new Vector2(0, 0);
        transform.rotation = Quaternion.identity;
        _accelerationSound.Stop();
    }

    private void RotationPlayer(float rotateDirection)
    {
        rotateDirection = rotateDirection > 1 ? 1 : rotateDirection;
        rotateDirection = rotateDirection < -1 ? -1 : rotateDirection;

        transform.eulerAngles += new Vector3(0, 0 , _rotationSpeed * rotateDirection * Time.deltaTime);
    }

    private void Dammage()
    {
        ResetPlayer();
        SoundController.main.PlayClip(_dammageClip);
        if (GameplayController.main.MinusHealth())
        {
            StartImmortal();
        }
        else
        {
            _canShoot = false;
        }
    }

    private void Move()
    {
        transform.Translate(_moveDirection.x * Time.fixedDeltaTime, _moveDirection.y * Time.fixedDeltaTime, 0, Space.World);
    }

    private void ImageFlashing()
    {
        if(Time.time % 0.5f > 0.25f)
            _image.SetActive(false);
        else
            _image.SetActive(true);
    }

    private void StartImmortal()
    {
        isImmortal = true;

        StartCoroutine(ImmortalTimer());
    }

    private void AccelerationSound()
    {
        if (isAcceleration)
        {
            if(_accelerationSound.isPlaying == false)
                _accelerationSound.Play();

            isAcceleration = false;
        }
        else if (!isAcceleration && _accelerationSound.isPlaying == true)
        {
            _accelerationSound.Stop();
        }
    }

    private IEnumerator ImmortalTimer()
    {
        yield return new WaitForSeconds(_immortalDuration);

        isImmortal = false;
        _image.SetActive(true);
    }
    private IEnumerator ShootTimer()
    {
        _canShoot = false;

        yield return new WaitForSeconds(1f / _shootsPerSecond);

        _canShoot = true;
    }
}
