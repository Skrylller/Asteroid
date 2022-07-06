using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableSceneObject : MonoBehaviour
{
    private Vector2 _screenSize;

    private void Start()
    {
        _screenSize = new Vector2(Camera.main.orthographicSize * Screen.width / Screen.height, Camera.main.orthographicSize);
    }

    private void Update()
    {
        CheckOutOfScreen();   
    }

    private void CheckOutOfScreen()
    {
        if (Mathf.Abs(transform.position.x) > _screenSize.x)
            transform.position = new Vector2(_screenSize.x  * Mathf.Sign(-transform.position.x), transform.position.y);

        if (Mathf.Abs(transform.position.y) > _screenSize.y)
            transform.position = new Vector2(transform.position.x, _screenSize.y * Mathf.Sign(-transform.position.y));
    }
}
