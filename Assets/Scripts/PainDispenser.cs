using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainDispenser : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Droid")]
    [SerializeField] private float duration = 1f;
    [SerializeField] private Animator droidsAnimator;

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
        //Delay first delivery
        yield return new WaitForSeconds(2f);

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

        Transform startTransform = startPoints[Random.Range(0, startPoints.Count)];
        Transform droid = startTransform.parent;
        Vector3 targetPoint = targetPoints[Random.Range(0, targetPoints.Count)].position;

        if (targetPoint.y <= 0f)
        {
            Debug.Log("Skipping delivery – target point too low");
            return;
        }

        droidsAnimator.speed = 0f;
        StartCoroutine(DroidAnimation(startTransform, targetPoint, droid));
    }

    //Play befor executing delivery animation
    private IEnumerator DroidAnimation(Transform startTransform, Vector3 target, Transform droid)
    {
        Vector3 start = startTransform.position;
        Quaternion initialRotation = droid.rotation;
        Vector3 directionToTarget = (target - start).normalized;
        Quaternion finalRotation = Quaternion.LookRotation(directionToTarget);
        SoundFXManager.instance.PlaySound3D(SoundType.ALERT, startTransform);

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            droid.rotation = Quaternion.Slerp(initialRotation, finalRotation, normalizedTime);
            yield return null;
        }

        droid.rotation = finalRotation;
        Vector3 fixedStart = startTransform.position;
        StartCoroutine(PainInDelivery(fixedStart, target));
    }

    //Start executing delivery animation
    private IEnumerator PainInDelivery(Vector3 start, Vector3 target)
    {
        GameObject obj = Instantiate(deliveryObject, start, Quaternion.identity);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        Vector3 direction = (target - start).normalized;

        SoundFXManager.instance.PlaySound3D(SoundType.BLASTER, obj.transform);
        obj.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        rb.velocity = direction * deliverySpeed;
        droidsAnimator.speed = 1f;

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
            SoundFXManager.instance.PlaySound2D(SoundType.PAIN, transform, 0.6f);
            Destroy(other.gameObject);
        }
    }
}