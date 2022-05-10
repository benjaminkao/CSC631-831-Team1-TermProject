using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemyMovement : MonoBehaviour
{
    private const float WALK_RANGE = 5.0f;
    private float _speed;
    private float _currentDistance;
    private Vector3 _direction;

    void Start()
    {
        _direction = transform.forward;
        _speed = 2.0f;
        _currentDistance = 0.0f;
    }

    void Update()
    {
        float step = _speed * Time.deltaTime;
        _currentDistance += step;
        transform.position += _direction * step;

        if (_currentDistance >= WALK_RANGE)
        {
            SwitchTarget();
            _currentDistance = 0.0f;
        }
    }

    private void SwitchTarget()
    {
        if (_direction == transform.forward) _direction = -transform.forward;
        else _direction = transform.forward;
    }
}
