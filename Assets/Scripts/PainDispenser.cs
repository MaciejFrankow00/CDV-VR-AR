using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainDispenser : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Pain")]
    [SerializeField] private GameObject deliveryObject;
    [SerializeField] private float deliverySpeed = 3f;
    [SerializeField] private float deliveryInterval = 2f;
    [SerializeField] [Tooltip("Delivery Object will be destroyed at this distance from target point")]
    private float destroyDistance = 3f;

    [Header("Automated processes")]
    [SerializeField]
    [Tooltip("Do not add elements manually!")]
    private List<Transform> startPoints = new List<Transform>();
    [SerializeField]
    [Tooltip("Do not add elements manually!")]
    private List<Transform> targetPoints = new List<Transform>();

    private Coroutine attackRoutine;

    private void Awake()
    {
        GameObject[] startObjs = GameObject.FindGameObjectsWithTag("PainSpawner");
        GameObject[] targetObjs = GameObject.FindGameObjectsWithTag("HitMe");

        startPoints.Clear();
        targetPoints.Clear();

        foreach (GameObject obj in startObjs)
            startPoints.Add(obj.transform);

        foreach (GameObject obj in targetObjs)
            targetPoints.Add(obj.transform);
    }

    private void Start()
    {
        if (playerHealth == null)
        {
            Debug.LogWarning("Pain Dispenser needs reference to Player Health!");
            return;
        }

        attackRoutine = StartCoroutine(LoopDeliverPain());
    }

    private IEnumerator LoopDeliverPain()
    {
        while (playerHealth.CurrentHealth > 0)
        {
            DeliverPain();
            yield return new WaitForSeconds(deliveryInterval);
        }
    }

    //Set random start and target points from lists
    private void DeliverPain()
    {
        if (startPoints.Count == 0 || targetPoints.Count == 0)
        {
            Debug.LogWarning("Start or target point list is empty");
            return;
        }

        Vector3 startPoint = startPoints[Random.Range(0, startPoints.Count)].position;
        Vector3 targetPoint = targetPoints[Random.Range(0, targetPoints.Count)].position;

        if (targetPoint.y <= 0f)
        {
            Debug.Log("Skipping delivery – target point too low");
            return;
        }

        StartCoroutine(PainInDelivery(startPoint, targetPoint));
    }

    //Start executing delivery animation
    private IEnumerator PainInDelivery(Vector3 start, Vector3 target)
    {
        GameObject obj = Instantiate(deliveryObject, start, Quaternion.identity);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        Vector3 direction = (target - start).normalized;

        obj.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        rb.velocity = direction * deliverySpeed;

        while (obj != null)
        {
            float distance = Vector3.Distance(obj.transform.position, target);

            if (distance > destroyDistance)
            {
                Destroy(obj);
                yield break;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pain"))
        {
            playerHealth.TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}