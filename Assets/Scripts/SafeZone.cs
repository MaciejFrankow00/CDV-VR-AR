using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafeZone : MonoBehaviour
{
    [Header("Player and Filter")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private CanvasGroup damageFilter;

    [Header("Visual")]
    [SerializeField] private Material zoneMaterial;
    [SerializeField] private float radius = 1.5f;

    private GameObject ring;
    private float damageTimer = 0f;

    public Vector3 Center => transform.position;
    public float Radius => radius;

    private void Start()
    {
        CreateVisual();
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(
            new Vector2(player.position.x, player.position.z),
            new Vector2(Center.x, Center.z)
        );

        bool isOutside = distance > Radius;

        if (isOutside && playerHealth != null)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= 1f)
            {
                playerHealth.TakeDamage(1);
                damageTimer = 0f;
            }

            if (damageFilter != null)
                damageFilter.alpha = 1f;
        }
        else
        {
            damageTimer = 0f;

            if (damageFilter != null)
                damageFilter.alpha = 0f;
        }
    }

    private void CreateVisual()
    {
        ring = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        ring.name = "VisualRing";
        ring.transform.SetParent(transform, false);

        float height = 0.05f;
        ring.transform.localScale = new Vector3(radius * 2f, height, radius * 2f);
        ring.transform.localPosition = Vector3.up * height;

        Destroy(ring.GetComponent<Collider>());

        if (zoneMaterial != null)
            ring.GetComponent<Renderer>().sharedMaterial = zoneMaterial;
    }
}