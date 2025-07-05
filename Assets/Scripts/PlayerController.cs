using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed = 5.0f;
    private bool _isSpeedBoosted = false;
    private int _doubleJumpCount = 0;

    float _horizontal;
    float _vertical;
    bool _jumpPressed;

    GroundCheck _groundCheck;
    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _groundCheck = GetComponent<GroundCheck>();
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        _jumpPressed = Input.GetButtonDown("Jump");

        SpeedBoosting();
        Movement();
    }

    private void Movement()
    {
        Vector3 move = new Vector3(_horizontal, 0, _vertical).normalized;
        _rb.MovePosition(_rb.position + move * _speed * Time.deltaTime);

        if (move != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, toRotation, 720 * Time.deltaTime));
        }

        if (_jumpPressed)
        {
            if (_groundCheck.IsGrounded())
            {
                _doubleJumpCount = 0;
                Jump();
            }
            else if (_doubleJumpCount < 1)
            {
                _doubleJumpCount++;
                Jump();
            }
            else
            {
                Debug.Log("You can't jump anymore!");
            }

            Debug.Log("Double Jump Count: " + _doubleJumpCount);
        }
    }

    private void SpeedBoosting()
    {
        if (Input.GetButtonDown("Fire3") && !_isSpeedBoosted)
        {
            _speed *= 2.0f;
            _isSpeedBoosted = true;
        }
        if (Input.GetButtonUp("Fire3") && _isSpeedBoosted)
        {
            _speed /= 2.0f;
            _isSpeedBoosted = false;
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z); // reset vertical speed
        _rb.AddForce(Vector3.up * 5.0f, ForceMode.Impulse);
    }
}
