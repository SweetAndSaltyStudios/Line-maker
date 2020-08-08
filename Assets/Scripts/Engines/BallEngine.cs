using System.Collections;
using UnityEngine;

public class BallEngine : MonoBehaviour
{
    private readonly float lifeTime = 15f;
    private readonly float flickeringTime = 0.15f;
    private readonly float maxImpactVelocity = 2f;

    private Effect shatterEffect;

    private Coroutine iLifeTime;
    private Coroutine iFlickeringEffect;

    private new Rigidbody2D rigidbody2D;
    private SpriteRenderer lineRenderer;

    public bool InPlayArea
    {
        get
        {
            return transform.position.y < CameraEngine.Instance.KillZone;
        }
    }
    public float CurrentVelocity
    {
        get
        {
            return rigidbody2D.velocity.magnitude;
        }
    }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponentInChildren<SpriteRenderer>();
        shatterEffect = Instantiate(ResourceManager.Instance.ShatterEffect, transform.position, Quaternion.identity, transform);
        shatterEffect.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartLifeTime();
    }

    private void OnDestroy()
    {
        iLifeTime = null;
        iFlickeringEffect = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var layerIndex = collision.gameObject.layer;

        CheckCollisionLayer(layerIndex, collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    private void StartLifeTime()
    {
        if (iLifeTime == null)
        {
            iLifeTime = StartCoroutine(ILifeTime(lifeTime));
        }
    }

    private void CheckCollisionLayer(int layerIndex, Collision2D collision)
    {
        switch (layerIndex)
        {
            case 8:

                // We should use somekind of callback system instead of getcomponent...
                if(CurrentVelocity >= maxImpactVelocity)
                {
                    AudioManager.Instance.PlayAudioClipAtPosition("BallDestroy", transform.position);
                    //collision.gameObject.GetComponent<BallEngine>().DestroyBall();
                    DestroyBall();
                }

                break;

            case 9:

                AudioManager.Instance.PlayAudioClipAtPosition("Goal", transform.position);

                LevelManager.Instance.AddEnergy(20);

                DestroyBall();

                break;

            case 12:

                AudioManager.Instance.PlayAudioClipAtPosition("BallDestroy", transform.position);

                DestroyBall();

                LevelManager.Instance.AddEnergy(-10);

                break;

            default:

                break;
        }
    }

    private void DestroyBall()
    {
        shatterEffect.transform.SetParent(null);
        shatterEffect.gameObject.SetActive(true);

        Destroy(gameObject);
    }

    private IEnumerator ILifeTime(float lifeTime)
    {
        var currentLifeTime = lifeTime;

        while (currentLifeTime > 0f)
        {
            currentLifeTime -= Time.deltaTime;

            if (currentLifeTime < 2f && iFlickeringEffect == null)
            {
                iFlickeringEffect = StartCoroutine(FlickeringEffect(flickeringTime));
            }

            yield return null;
        }

        DestroyBall();
    }

    private IEnumerator FlickeringEffect(float flickeringTime)
    {
        while (true)
        {
            lineRenderer.enabled = !lineRenderer.enabled;
            yield return new WaitForSeconds(flickeringTime);
        }
    }
}
