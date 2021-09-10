using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _gravity = 9.8f;
    [SerializeField]
    private float _jumpHeight = 3.0f;
    private float _xVelocity;
    private float _yVelocity;
    private Vector3 _playerVelocity;
    private bool _canDoubleJump = false;
    [SerializeField]
    private int _coins;
    private UIManager _uiManager;
    [SerializeField]
    private int _lives = 3;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL."); 
        }

        _uiManager.UpdateLivesDisplay(_lives);
    }

    // Update is called once per frame
    void Update()
    {
        if (_controller.isGrounded == true && _yVelocity < 0)
        {
            _yVelocity = 0f;
        }

        if (_controller.isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            _yVelocity = _jumpHeight;
            _canDoubleJump = true;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && _canDoubleJump == true)
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = false;
            }
            else
            {
                _yVelocity -= _gravity * Time.deltaTime;
            }
        }
 
        _xVelocity = Input.GetAxis("Horizontal") * _speed;
        _playerVelocity = new Vector3(_xVelocity, _yVelocity, 0f);
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    public void AddCoins()
    {
        _coins++;

        _uiManager.UpdateCoinDisplay(_coins);
    }

    public void Damage()
    {
        _lives--;

        _uiManager.UpdateLivesDisplay(_lives);

        if (_lives < 1)
        {
            SceneManager.LoadScene(0);
        }
    }
}
