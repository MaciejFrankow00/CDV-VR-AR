using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainDispenser : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Pain")]
    [SerializeField] private GameObject deliveryObject;
    [SerializeField] private float deliveryTime = 1.5f;
    [SerializeField] private float curveHeight = 2f;
    [SerializeField] private float deliveryInterval = 2f;

    [Header("Automated processes")]
    [SerializeField] [Tooltip("Do not add elements manually!")] 
    private List<Transform> startPoints = new List<Transform>();
    [SerializeField] [Tooltip("Do not add elements manually!")] 
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
        float time = 0f;
        GameObject obj = Instantiate(deliveryObject, start, Quaternion.identity);

        while (time < deliveryTime)
        {
            float t = time / deliveryTime;
            Vector3 pos = Vector3.Lerp(start, target, t);
            
            //Fancy curve trajectory
            float arc = 4 * curveHeight * t * (1 - t);
            pos.y += arc;

            obj.transform.position = pos;
            time += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = target;
        Destroy(obj);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pain"))
        {
            playerHealth.TakeDamage(1);
        }
    }
}