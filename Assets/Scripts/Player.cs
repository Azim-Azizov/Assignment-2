using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    CharacterController characterController;
    [SerializeField]
    Transform cameraTransform;
    [SerializeField]
    GameObject[] coinPrefabs;
    [SerializeField]
    TextMeshProUGUI coinText;
    [SerializeField]
    AudioSource coinAudioSource;
    [SerializeField]
    float speed = 10f;
    [SerializeField]
    float sensetivity = 1;
    [SerializeField]
    float additionalForce = 10f;

    float h;
    float v;
    Vector3 forward;
    Vector3 right;
    int coins = 0;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SpawnCoin();
    }

    void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        // Calculate the movement direction relative to the camera
        forward = cameraTransform.forward;
        right = cameraTransform.right;
    }

    void FixedUpdate()
    {
        // Rotate the character and camera with mouse input
        RotateWithMouse();

        // Move the character based on input relative to the camera direction
        MoveCharacter();
    }

    void RotateWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X");

        // Adjust the rotation based on mouse movement for both character and camera
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += mouseX * sensetivity;
        transform.rotation = Quaternion.Euler(rotation);

        // Rotate the camera around the player
        float cameraRotationY = cameraTransform.rotation.eulerAngles.y + mouseX * sensetivity;
        Vector3 offset = Quaternion.Euler(0, mouseX * sensetivity, 0) * (cameraTransform.position - transform.position);
        cameraTransform.position = transform.position + offset;
        cameraTransform.LookAt(transform.position);
    }

    void MoveCharacter()
    {
        forward.y = 0f; // Ignore vertical component
        right.y = 0f;

        Vector3 moveDirection = (forward.normalized * v + right.normalized * h).normalized;
        if (transform.position.y > 1f)
        {
            moveDirection.y = -9.81f * Time.fixedDeltaTime * 20;
        }

        // Move the character using CharacterController
        characterController.Move(moveDirection * speed * Time.fixedDeltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the collision is with a ball
        if (hit.gameObject.CompareTag("Ball"))
        {
            // Calculate the direction from the capsule to the ball
            Vector3 directionToBall = hit.gameObject.transform.position - transform.position;

            // Apply additional force to the ball in the direction from the capsule to the ball
            Rigidbody ballRigidbody = hit.gameObject.GetComponent<Rigidbody>();
            if (ballRigidbody != null)
            {

                // Apply the force instantly using ForceMode.Impulse
                ballRigidbody.AddForce(directionToBall.normalized * additionalForce, ForceMode.Impulse);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coinAudioSource.Play();
            coins += other.gameObject.GetComponent<Coin>().value;
            other.gameObject.SetActive(false);
            coinText.text = coins.ToString();
            SpawnCoin();
        }
    }

    void SpawnCoin()
    {
        Instantiate(
            coinPrefabs[Random.Range(0, 3)],
            new Vector3(Random.Range(-9.5f, 9.5f), 0.5f, Random.Range(-14.5f, 14.5f)),
            Quaternion.identity);
    }
}
