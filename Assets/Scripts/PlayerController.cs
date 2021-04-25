using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("[Movement Speed]")]
	[SerializeField] private float moveSpeed = 20f;
	[SerializeField] private float maxSpeed = 3f;
	[SerializeField] private float jumpForce = 6f;
	[SerializeField] private int jumpInputCount = 10;
	
	[Header("[Mouse and Camera Control]")]
	[SerializeField] private float mouseSensitivity = 2f;
	[SerializeField] private float mouseSmoothing = 8f;
	[SerializeField] private float minAngleY = -90f;
	[SerializeField] private float maxAngleY = 90f;
	
	private float verticalInput;
	private float horizontalInput;
	private int willJump;
	private Vector3 zeroY = new Vector3(1, 0, 1);
	private Transform mainCamera;
	private Rigidbody rb;
	
	private float mouseX, mouseY;
	private Vector2 mouseLook, smoothV;
	
    void Start()
    {
        rb = GetComponent<Rigidbody>();
		mainCamera = Camera.main.transform;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }

    void Update()
    {
		GetInput();
		MoveCamera();
    }
	
	void FixedUpdate()
	{
		MovePlayer();
	}
	
	private void GetInput()
	{
		verticalInput = Input.GetAxisRaw("Vertical");		
		horizontalInput = Input.GetAxisRaw("Horizontal");
		
		mouseX = Input.GetAxis("Mouse X");
		mouseY = Input.GetAxis("Mouse Y");
		
		if (Input.GetKeyDown("space"))
        {
			willJump = jumpInputCount;
        }
	}
	
	private void MoveCamera()
	{
		float mouseMovementX = mouseX * mouseSensitivity * mouseSmoothing;
		float mouseMovementY = mouseY * mouseSensitivity * mouseSmoothing;
		
		smoothV = new Vector2(Mathf.Lerp(smoothV.x, mouseMovementX, mouseSmoothing * Time.deltaTime), Mathf.Lerp(smoothV.y, mouseMovementY, mouseSmoothing * Time.deltaTime));
        mouseLook += smoothV;
        mouseLook.y = Mathf.Clamp(mouseLook.y, minAngleY, maxAngleY);

        mainCamera.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        transform.rotation = Quaternion.AngleAxis(mouseLook.x, transform.up);
	}
	
	private void MovePlayer()
	{
		Vector3 verticalVector = Vector3.Scale(mainCamera.forward, zeroY) * verticalInput;
		Vector3 horizontalVector = Vector3.Scale(mainCamera.right, zeroY) * horizontalInput;
		
		rb.AddForce((verticalVector + horizontalVector).normalized * moveSpeed);
		
		if (verticalInput != 0 || horizontalInput != 0)
		{
			if (Vector3.Scale(rb.velocity, zeroY).sqrMagnitude > maxSpeed)
			{
				Vector3 clampSpeed = Vector3.Scale(rb.velocity, zeroY).normalized * maxSpeed;
				rb.velocity = new Vector3(clampSpeed.x, rb.velocity.y, clampSpeed.z);
			}
		}
		
		else
		{
			rb.velocity = new Vector3(0, rb.velocity.y, 0);
		}
		
		if (willJump > 0)
        {
			RaycastHit hit;
			
			if (Physics.SphereCast(transform.position, 0.4f, -Vector3.up, out hit, 0.15f))
			{
				rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
				willJump = 0;
			}
			
			else
			{
				willJump --;
			}
        }
	}
}
