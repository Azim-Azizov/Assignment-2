using TMPro;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    TextMeshProUGUI textGoals;
    [SerializeField]
    AudioSource audioSource;

    int goals = 0;

    void FixedUpdate()
    {
        if (transform.position.y < 0)
            resetBall();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            audioSource.Play();
            textGoals.text = (++goals).ToString();
            resetBall();
        }
    }

    void resetBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = new Vector3(0, 0.5f, 0);
    }
}
