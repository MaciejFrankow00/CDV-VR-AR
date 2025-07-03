using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsaber : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerHealth playerHealth;

    private bool playerIsAlive = true;

    [Header("Saber animation")]
    [SerializeField] private GameObject saber;
    [SerializeField] private float duration = 0.5f;
    
    private bool startAnimation = true;
    
    [Header("Trail renderer")]
    [SerializeField] private GameObject tipPoint;
    [SerializeField] private GameObject basePoint;
    [SerializeField] private GameObject meshParent;
    [SerializeField] private int trailFrameLength = 3;
    [SerializeField] private AudioSource audioSource;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private int frameCount;
    private Vector3 previousTipPosition;
    private Vector3 previousBasePosition;
    private const int NUM_VERTICES = 12;
    private AudioClip audioClip;
    private bool playingSwoosh = true;

    void Start()
    {
        StartCoroutine(TurnOnAnimation());
        
        mesh = new Mesh();
        mesh.name = "TrailMesh";
        meshParent.GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[trailFrameLength * NUM_VERTICES];
        triangles = new int[vertices.Length];

        previousTipPosition = tipPoint.transform.position;
        previousBasePosition = basePoint.transform.position;

        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 100f);
    }

    void Update()
    {
        if(playerHealth.CurrentHealth <= 0 && playerIsAlive)
        {
            playerIsAlive = false;
            StartCoroutine(TurnOffAnimation());
        }
    }

    void LateUpdate()
    {
        if (frameCount == (trailFrameLength * NUM_VERTICES))
        {
            frameCount = 0;
        }

        Vector3 localTip = meshParent.transform.InverseTransformPoint(tipPoint.transform.position);
        Vector3 localBase = meshParent.transform.InverseTransformPoint(basePoint.transform.position);
        Vector3 localPrevTip = meshParent.transform.InverseTransformPoint(previousTipPosition);
        Vector3 localPrevBase = meshParent.transform.InverseTransformPoint(previousBasePosition);

        //Movement detection based on local position of tipPoint and basePoint
        float movementThreshold = 0.5f;
        bool tipMoved = Vector3.Distance(localTip, localPrevTip) > movementThreshold;
        bool baseMoved = Vector3.Distance(localBase,localPrevBase) > movementThreshold;
        bool isTrailVisible = tipMoved || baseMoved;

        //Playing swoosh sound logic
        if (!startAnimation && isTrailVisible)
        {
            if (!playingSwoosh)
            {
                playingSwoosh = true;
                
                SoundFXManager.instance.ChooseRandom(SoundType.SWOOSH, audioSource);
                audioClip = audioSource.clip;

                if (audioClip != null)
                {
                    audioSource.Play();
                }
            }
        }
        else
        {
            playingSwoosh = false;
            audioClip = null;
        }

        //Rendering trail
        vertices[frameCount] = localBase;
        vertices[frameCount + 1] = localTip;
        vertices[frameCount + 2] = localPrevTip;
        vertices[frameCount + 3] = localBase;
        vertices[frameCount + 4] = localPrevTip;
        vertices[frameCount + 5] = localTip;
        vertices[frameCount + 6] = localPrevTip;
        vertices[frameCount + 7] = localBase;
        vertices[frameCount + 8] = localPrevBase;
        vertices[frameCount + 9] = localPrevTip;
        vertices[frameCount + 10] = localPrevBase;
        vertices[frameCount + 11] = localBase;

        for (int i = 0; i < NUM_VERTICES; i++)
        {
            triangles[frameCount + i] = frameCount + i;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 100f);

        previousTipPosition = tipPoint.transform.position;
        previousBasePosition = basePoint.transform.position;
        frameCount += NUM_VERTICES;
    }

    public IEnumerator TurnOnAnimation()
    {
        Vector3 scale = saber.transform.localScale;
        float finalScaleZ = scale.z;

        scale.z = 0f;
        saber.transform.localScale = scale;

        yield return new WaitForSeconds(0.5f);
        SoundFXManager.instance.PlaySound3D(SoundType.SABER_ACTIVATION, transform, 0.6f);

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            scale.z = Mathf.Lerp(0f, finalScaleZ, normalizedTime);
            saber.transform.localScale = scale;
            yield return null;
        }

        scale.z = finalScaleZ;
        saber.transform.localScale = scale;

        yield return new WaitForSeconds(0.001f);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        startAnimation = false;
    }

    public IEnumerator TurnOffAnimation()
    {
        Vector3 scale = saber.transform.localScale;
        float initialScaleZ = scale.z;
        
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();

        yield return new WaitForSeconds(0.5f);
        SoundFXManager.instance.PlaySound3D(SoundType.SABER_ACTIVATION, transform, 0.6f);

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            scale.z = Mathf.Lerp(initialScaleZ, 0f, normalizedTime);
            saber.transform.localScale = scale;
            yield return null;
        }

        scale.z = 0f;
        saber.transform.localScale = scale;
    }
}