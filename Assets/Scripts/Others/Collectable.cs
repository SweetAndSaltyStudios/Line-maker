using UnityEngine;

public class Collectable : MonoBehaviour
{
    private Effect shatterEffect;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        shatterEffect = Instantiate(ResourceManager.Instance.ShatterEffect, transform.position, Quaternion.identity, transform);
        shatterEffect.ChangeColor(Color.yellow);
        shatterEffect.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(8))
        {
            AudioManager.Instance.PlayAudioClipAtPosition("Collectable", transform.position);

            LevelManager.Instance.AddCollectable(1);

            shatterEffect.transform.SetParent(null);
            shatterEffect.gameObject.SetActive(true);

            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
