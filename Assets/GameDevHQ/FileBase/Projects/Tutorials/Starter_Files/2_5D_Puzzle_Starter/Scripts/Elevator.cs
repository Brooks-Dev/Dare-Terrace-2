using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private Transform _pointA, _pointB;
    private Transform _target;
    private float _speed = 3.0f;
    private bool _onElevator;
    private ElevatorPanel _elevatorPanel;

    private void Start()
    {
        _elevatorPanel = GameObject.Find("Elevator_Panel").GetComponent<ElevatorPanel>();
        if (_elevatorPanel == null)
        {
            Debug.LogError("Elevator panel is null on elevator.");
        }
        _target = _pointA;
    }
    void Update()
    {
        if (_onElevator && Input.GetKeyDown(KeyCode.E))
        {
            CallElevator();
        }
    }

    void FixedUpdate()
    {
        if (_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _target.transform.position) < 0.05f)
            {
                _target = null;
            }
        }
    }

    public void CallElevator()
    {
        if (_target == null)
        {
            _elevatorPanel.ButtonPressed();
        }
        if (Vector3.Distance(transform.position, _pointA.transform.position) < 0.05f)
        {
            _target = _pointB;
        }
        else if (Vector3.Distance(transform.position, _pointB.transform.position) < 0.05f)
        {
            _target = _pointA;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = this.transform;
        _onElevator = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _onElevator = false;
        other.transform.parent = null;
    }
}
