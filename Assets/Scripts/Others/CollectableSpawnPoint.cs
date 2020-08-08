using System.Collections;
using UnityEngine;

public class CollectableSpawnPoint : MonoBehaviour
{
    private Collectable ownedCollectable;

    private Coroutine iSpawnCollectable_Coroutine;

    private readonly int respawnTime = 10;

    private void Awake()
    {

    }

    private void Start()
    {
        SpawnCollectable();

        if (iSpawnCollectable_Coroutine == null)
        {
            iSpawnCollectable_Coroutine = StartCoroutine(ISpawnCollectable());
        }
    }

    public Vector2 Position
    {
        get 
        {
            return transform.position;
        }
    }

    private void SpawnCollectable()
    {
        ownedCollectable = Instantiate(ResourceManager.Instance.Collectable, Position, Quaternion.identity, transform);
    }

    private IEnumerator ISpawnCollectable()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitUntil(() => ownedCollectable == null);

            yield return new WaitForSeconds(respawnTime);

            SpawnCollectable();
        }

        iSpawnCollectable_Coroutine = null;
    }
}
