using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SimpleFPS : MonoBehaviour
{
    public float walkSpeed = 5f;        // Speed of walking
    public float runSpeed = 10f;        // Speed of running
    public float jumpForce = 5f;        // Force of jumping
    public Transform cameraTransform;   // Reference to the camera transform
    public float mouseSensitivity = 2f; // Mouse sensitivity for camera control

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isJumping;
    public AudioSource land;
    private bool Hactivo;
    private bool Vactivo;
    public float mouseX;
    public float mouseY;
    public float AxisMouseX;
    public float AxisMouseY;

    [Header("Camera")]
    [SerializeField] Transform target;
    [SerializeField] Transform Camera;
    [SerializeField] Material materialCamera;
    [SerializeField] Camera nightVision;
    [SerializeField] Camera withoutnightVision;
    bool isActiveCamera;
    bool NocturneVision;
    public bool isOnAreaToPickUp;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        PickUpObject();
        Movement();
        CameraMode();
    }
    private void CameraMode()
    {
        if (isActiveCamera)
        {
            if (Input.GetKeyDown(KeyCode.F)) 
            {
                if(NocturneVision==false)
                {
                    nightVision.gameObject.SetActive(true);
                    withoutnightVision.gameObject.SetActive(true);
                    NocturneVision = true;
                }else if (NocturneVision)
                {
                    nightVision.gameObject.SetActive(false);
                    withoutnightVision.gameObject.SetActive(true);
                    NocturneVision = false;
                }
            }
        }
    }
    private void PickUpObject()
    {
        if (isOnAreaToPickUp == true)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                // Establece el nuevo padre
                Camera.transform.SetParent(target.transform);

                // Reinicia la posición y rotación local a cero respecto al nuevo padre
                Camera.transform.localPosition = Vector3.zero;
                Camera.transform.localRotation = Quaternion.identity;
                isActiveCamera = true;
            }
        }
    }
    private void Movement()
    {
        // Player movement
        float moveSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        controller.Move(move * moveSpeed * Time.deltaTime);
        if (Input.GetButtonDown("Horizontal"))
        {
            if (Vactivo == false)
            {
                Hactivo = true;
                land.Play();
            }
        }
        if (Input.GetButtonDown("Vertical"))
        {
            if (Hactivo == false)
            {
                Vactivo = true;
                land.Play();
            }
        }
        if (Input.GetButtonUp("Horizontal"))
        {
            Hactivo = false;
            if (Vactivo == false)
            {
                land.Pause();
            }
        }
        if (Input.GetButtonUp("Vertical"))
        {
            Vactivo = false;
            if (Hactivo == false)
            {
                land.Pause();
            }
        }
        // Player jumping
        if (controller.isGrounded)
        {
            playerVelocity.y = 0f;
            isJumping = false;
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            playerVelocity.y += Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            isJumping = true;
        }

        // Apply gravity to the player
        playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Player camera control
        AxisMouseX = Input.GetAxis("Mouse X");
        AxisMouseY = Input.GetAxis("Mouse Y");
        mouseX = AxisMouseX * mouseSensitivity;
        mouseY = AxisMouseY * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically
        Vector3 currentRotation = cameraTransform.rotation.eulerAngles;
        float desiredRotationX = currentRotation.x - mouseY;
        if (desiredRotationX > 180)
            desiredRotationX -= 360;
        desiredRotationX = Mathf.Clamp(desiredRotationX, -90f, 90f);
        cameraTransform.rotation = Quaternion.Euler(desiredRotationX, currentRotation.y, currentRotation.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ObjectPickUp"))
        {
            isOnAreaToPickUp = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ObjectPickUp"))
        {
            isOnAreaToPickUp = false;
        }
    }
}
