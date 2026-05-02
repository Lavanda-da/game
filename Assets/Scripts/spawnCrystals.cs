using UnityEngine;

public class spawnCrystals : MonoBehaviour
{
    [Header("Область спавна")]
    [SerializeField] public Vector3 center = new Vector3(-12.5f, 20f, -7f);
    [SerializeField] public float width = 25f;
    [SerializeField] public float length = 15f;

    [Header("Настройки спавна")]
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] public int cubeCount = 5;
    [SerializeField] public float size = 0.25f;
    [SerializeField] public float yOffset = 0.25f;

    void Start()
    {
        SpawnCubes();
    }

    void SpawnCubes()
    {
        for (int i = 0; i < cubeCount; ++i)
        {
            float x = center.x + Random.Range(-width / 2, width / 2);
            float z = center.z + Random.Range(-length / 2, length / 2);

            RaycastHit hit;
            float y = 0f;
            if (Physics.Raycast(new Vector3(x, 9f, z), Vector3.down, out hit, 200f))
            {
                y = hit.point.y;
                Vector3 spawnPosition = new Vector3(x, y + yOffset, z);
                Quaternion originalRotation = cubePrefab.transform.rotation;
                GameObject cube = Instantiate(cubePrefab, spawnPosition, originalRotation);
                cube.transform.localScale = Vector3.one * size;
            } 
            else
            {
                Debug.Log($"{center.x} {width} {x} ; {center.z} {length} {z}");
            }
        }
    }
}
