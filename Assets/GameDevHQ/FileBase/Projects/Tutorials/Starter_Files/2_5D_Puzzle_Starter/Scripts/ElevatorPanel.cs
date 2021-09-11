using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPanel : MonoBehaviour
{
    private bool _atElevator;
    [SerializeField]
    private int _requiredCoins = 8;
    private int _coinsCollected;
    private Elevator _elevator;
    private MeshRenderer _button;

    private void Start()
    {
        _elevator = GameObject.Find("Elevator").GetComponent<Elevator>();
        if (_elevator == null)
        {
            Debug.LogError("Elevator is null in elevator panel.");
        }
        _button = transform.Find("Call_Button").GetComponent<MeshRenderer>();
        if (_button == null)
        {
            Debug.LogError("Button is null in elevator panel.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _atElevator == true && _coinsCollected >= _requiredCoins)
        {
            _elevator.CallElevator();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _atElevator = true;
            _coinsCollected = other.GetComponent<Player>().CoinCount();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _atElevator = false;
        }
    }

    public void ButtonPressed()
    {
        if (_button.material.color == Color.green)
        {
            _button.material.color = Color.red;
        }
        else
        {
            _button.material.color = Color.green;
        }
    }
}
