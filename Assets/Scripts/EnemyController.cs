using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform hero;
    private Steering steeringScript;
    private bool isHeroInside = false;

    private Vector3 roomCenter = new Vector3(80.02592f, 2.480645f, 62.14454f);
    private Vector3 roomSize = new Vector3(40.97554f, 5.96129f, 28.51953f);

    void Start()
    {
        steeringScript = GetComponent<Steering>();

        if (steeringScript == null)
        {
            Debug.LogError($"[EnemyController] {gameObject.name} - Missing Steering component");
        }
    }

    void Update()
    {
        if (hero == null)
        {
            Debug.LogError("[EnemyController] Hero reference not set");
            return;
        }

        isHeroInside = IsHeroInsideRoom(hero.position);

        if (isHeroInside)
        {
            EnableSteering();
        }
        else
        {
            DisableSteering();
        }

        PreventLeavingRoom();
    }

    bool IsHeroInsideRoom(Vector3 heroPos)
    {
        float minX = roomCenter.x - (roomSize.x / 2);
        float maxX = roomCenter.x + (roomSize.x / 2);
        float minZ = roomCenter.z - (roomSize.z / 2);
        float maxZ = roomCenter.z + (roomSize.z / 2);

        return (heroPos.x >= minX && heroPos.x <= maxX &&
                heroPos.z >= minZ && heroPos.z <= maxZ);
    }

    void PreventLeavingRoom()
    {
        if (steeringScript == null)
            return;

        float minX = roomCenter.x - (roomSize.x / 2);
        float maxX = roomCenter.x + (roomSize.x / 2);
        float minZ = roomCenter.z - (roomSize.z / 2);
        float maxZ = roomCenter.z + (roomSize.z / 2);

        Vector3 newPosition = transform.position;
        bool outOfBounds = false;

        if (newPosition.x < minX) { newPosition.x = minX; outOfBounds = true; }
        if (newPosition.x > maxX) { newPosition.x = maxX; outOfBounds = true; }
        if (newPosition.z < minZ) { newPosition.z = minZ; outOfBounds = true; }
        if (newPosition.z > maxZ) { newPosition.z = maxZ; outOfBounds = true; }

        if (outOfBounds)
        {
            Debug.Log($"[EnemyController] {gameObject.name} repositioned inside: {newPosition}");
        }

        transform.position = newPosition;
    }

    void EnableSteering()
    {
        if (steeringScript != null && !steeringScript.enabled)
        {
            steeringScript.enabled = true;
        }
    }

    void DisableSteering()
    {
        if (steeringScript != null && steeringScript.enabled)
        {
            steeringScript.enabled = false;
        }
    }
}
