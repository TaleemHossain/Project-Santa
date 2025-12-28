using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakingPlatform: MonoBehaviour
{
    [SerializeField] private float breakDelay = 2f;

    private Tilemap tilemap;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // Check each contact point
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Only break tiles the player lands ON (from above)
            if (contact.normal.y < 0.5f)
                continue;

            Vector3 worldPoint = contact.point;
            Vector3Int cellPos = tilemap.WorldToCell(worldPoint);

            if (tilemap.HasTile(cellPos))
            {
                StartCoroutine(BreakTile(cellPos));
            }
        }
    }

    System.Collections.IEnumerator BreakTile(Vector3Int cellPos)
    {
        yield return new WaitForSeconds(breakDelay);

        if (tilemap.HasTile(cellPos))
        {
            tilemap.SetTile(cellPos, null);
        }
    }
}
