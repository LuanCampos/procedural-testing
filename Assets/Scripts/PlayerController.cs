using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("[Movement Speed]")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float jumpHeight = 5f;
	
	[Header("[Mouse and Camera Control]")]
	[SerializeField] private float mouseSensitivity = 2f;
	[SerializeField] private float mouseSmoothing = 8f;
	[SerializeField] private float minAngleY = -90f;
	[SerializeField] private float maxAngleY = 90f;
	
	private float verticalInput;
	private float horizontalInput;
	private bool willJump;
	private Vector3 zeroY = new Vector3(1, 0, 1);
	private Vector3 justY = new Vector3(0, 1, 0);
	private Transform mainCamera;
	private Rigidbody rb;
	
	private float mouseX, mouseY;
	private Vector2 mouseLook, smoothV;
	
    void Start()
    {
        rb = GetComponent<Rigidbody>();
		mainCamera = Camera.main.transform;
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
			RaycastHit hit;
			
			Debug.DrawRay(transform.position, -Vector3.up * 0.6f, Color.green, 2f);
			
			if (Physics.Raycast(transform.position, -Vector3.up, out hit, 0.6f))
			{
				willJump = true;
			}
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
		float yVelocity = rb.velocity.y;
		
		if (willJump)
		{
			yVelocity += jumpHeight;
			willJump = false;
		}
		
		rb.velocity = ((verticalVector + horizontalVector).normalized * moveSpeed) + justY * yVelocity;
	}
}
