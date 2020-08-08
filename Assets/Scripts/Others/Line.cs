using System.Collections;
using UnityEngine;

public class Line : MonoBehaviour
{
    #region VARIABLES

    private readonly float lifeTime = 10f;
    private readonly float flickeringTime = 0.15f;

    private Coroutine iLifeTime;
    private Coroutine iFlickeringEffect;

    private LineRenderer lineRenderer;
    private readonly Vector2[] lineRendererEndPoints = new Vector2[2];
    private Vector2 lineStartPoint;

    private Effect shatterEffect;

    private Color defaultColor;
    private Color validPlacementColor = new Color(0, 1, 0, 0.6f);
    private Color invalidPlacementColor = new Color(1, 0, 0, 0.6f);

    #endregion VARIABLES

    #region PROPERTIES

    public float CurrentLenght
    {
        get
        {
            return Vector2.Distance(lineStartPoint, lineRenderer.GetPosition(1));
        }
    }
    public EdgeCollider2D EdgeCollider2D
    {
        get;
        private set;
    }
    public Vector2 LineCenterPoint
    {
        get
        {
            return (lineStartPoint + lineRendererEndPoints[1]) / 2;
        }
    }

    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        EdgeCollider2D = GetComponent<EdgeCollider2D>();
        defaultColor = lineRenderer.endColor;      
    }

    private void Start()
    {
        SetLineRendererPoint(0, InputManager.Instance.CurrentTouchPoint);
        EdgeCollider2D.isTrigger = true;
        lineStartPoint = lineRenderer.GetPosition(0);

        shatterEffect = Instantiate(ResourceManager.Instance.ShatterEffect, transform);
        shatterEffect.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        iLifeTime = null;
        iFlickeringEffect = null;
    }

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

    public void UpdateLine(Vector2 newPoint)
    {
        SetLineRendererPoint(1, newPoint);
        UpdateCollision();
    }

    public void PlaceLine()
    {
        EdgeCollider2D.isTrigger = false;
        SetLineColor(defaultColor);

        StartLifeTime();
    }

    public void DestroyLine()
    {
        shatterEffect.transform.position = LineCenterPoint;
        shatterEffect.transform.SetParent(null);
        shatterEffect.gameObject.SetActive(true);

        AudioManager.Instance.PlayAudioClipAtPosition("BallDestroy", transform.position);

        Destroy(gameObject);
    }

    private void SetLineColor(Color newColor)
    {
        lineRenderer.startColor = newColor;
        lineRenderer.endColor = newColor;
    }

    public void CanPlace(bool canPlace)
    {
        SetLineColor(canPlace ? validPlacementColor : invalidPlacementColor);

        if(shatterEffect != null)
        shatterEffect.ChangeColor(canPlace ? defaultColor : invalidPlacementColor);
    }

    private void SetLineRendererPoint(int index, Vector2 newPoint)
    {
        lineRenderer.SetPosition(index, newPoint);
        lineRendererEndPoints[index] = newPoint;
    }

    private void UpdateCollision()
    {
        EdgeCollider2D.points = lineRendererEndPoints;
    }

    private void StartLifeTime()
    {
        if(iLifeTime == null)
        {
            iLifeTime = StartCoroutine(ILifeTime(lifeTime));
        }
    }

    private IEnumerator ILifeTime(float lifeTime)
    {
        var currentLifeTime = lifeTime;

        while (currentLifeTime > 0f)
        {
            currentLifeTime -= Time.deltaTime;

            if(currentLifeTime < 2f && iFlickeringEffect == null)
            {
                iFlickeringEffect = StartCoroutine(FlickeringEffect(flickeringTime));
            }

            yield return null;
        }

        DestroyLine();
    }

    private IEnumerator FlickeringEffect(float flickeringTime)
    {
        while (true)
        {
            lineRenderer.enabled = !lineRenderer.enabled;
            yield return new WaitForSeconds(flickeringTime);
        }      
    }

    #endregion CUSTOM_FUNCTIONS
}
