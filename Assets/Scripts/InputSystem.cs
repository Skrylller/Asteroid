using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputSystem : MonoBehaviour
{
    public static InputSystem main;

    [SerializeField] private UnityEvent _actionW, _actionA, _actionD, _actionSpace, _actionEsc, _actionMouse0;

    private bool _isFirstControl;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        _isFirstControl = true;
    }

    private void Update()
    {
        if (_isFirstControl)
        {
            CheckFirstControl();
        }
        else
        {
            CheckSecondControl();
        }
    }

    public void ChangeControlType()
    {
        _isFirstControl = !_isFirstControl;
    }

    private void CheckFirstControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _actionW.Invoke();
        }

        if (Input.GetKey(KeyCode.A))
        {
            _actionA.Invoke();
        }

        if (Input.GetKey(KeyCode.D))
        {
            _actionD.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _actionSpace.Invoke();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            _actionEsc.Invoke();
        }
    }

    private void CheckSecondControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _actionW.Invoke();
        }

        PlayerController.main.RotationToPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _actionMouse0.Invoke();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            _actionEsc.Invoke();
        }
    }

}
