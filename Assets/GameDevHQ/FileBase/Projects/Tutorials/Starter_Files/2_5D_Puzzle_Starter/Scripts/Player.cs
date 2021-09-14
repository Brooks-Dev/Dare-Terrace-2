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
    [SerializeField]
    private bool _canWallJump;
    private Vector3 _hitNormal;
    private float _pushPower = 2f;

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
            _xVelocity = 0f;
        }

        if (_controller.isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            _yVelocity = _jumpHeight;
            _canDoubleJump = true;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && _canDoubleJump == true && _canWallJump == false)
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = false;
            }
            else if (_canWallJump == true & Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Hit normal " + _hitNormal);
                _xVelocity = _jumpHeight * _hitNormal.x;
                _yVelocity = _jumpHeight;
                _canWallJump = false;
                _hitNormal = Vector3.zero;
            }
            else
            {
                _yVelocity -= _gravity * Time.deltaTime;
            }
        }
 
        if (_controller.isGrounded == true)
        {
            _xVelocity = Input.GetAxis("Horizontal") * _speed;
        }

        _playerVelocity = new Vector3(_xVelocity, _yVelocity, 0f);
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (_controller.isGrounded == false && hit.transform.CompareTag("Wall"))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.blue);
            _canWallJump = true;
            _hitNormal = hit.normal;
        }

        if (body == null || body.isKinematic) return;
        if (hit.transform.CompareTag("MovableBox") && hit.moveDirection.y >= -0.3f)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0f, 0f);
            body.velocity = pushDir * _pushPower;
        }
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

    public int CoinCount()
    {
        return _coins;
    }
}
