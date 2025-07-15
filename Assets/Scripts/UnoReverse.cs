using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoReverse : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private float reverseSpeed;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pain"))
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            Vector3 currentVelocity = rb.velocity;

            Vector3 horizontalDirection = new Vector3(currentVelocity.x, 0f, currentVelocity.z).normalized;
            Vector3 reversedDirection = -horizontalDirection * reverseSpeed;

            rb.velocity = reversedDirection;
            scoreManager.AddPoint();
            SoundFXManager.instance.PlaySound2D(SoundType.SCORE_INCREASED, transform, 0.5f);
        }
    }
}
