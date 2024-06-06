using System.Collections;
using UnityEngine;
using TMPro;

public class LifecycleManager : MonoBehaviour
{
    public GameObject eggPrefab;
    public GameObject chickPrefab;
    public GameObject henPrefab;
    public GameObject roosterPrefab;

    public TMP_Text eggCountText;
    public TMP_Text chickCountText;
    public TMP_Text henCountText;
    public TMP_Text roosterCountText;

    private int eggCount = 0;
    private int chickCount = 0;
    private int henCount = 0;
    private int roosterCount = 0;

    public Vector2 spawnAreaMin = new Vector2(-50, -50);
    public Vector2 spawnAreaMax = new Vector2(50, 50);
    private Vector3 lastSpawnPosition;

    private float elapsedTime = 0f;
    private bool stopSpawning = false;
    private const float timeLimit = 4 * 60; // 4 minutes in seconds

    void Start()
    {
        Debug.Log("Starting Lifecycle Manager");
        UpdateCountText();
        StartCoroutine(StartInitialLifecycle());
    }

    void Update()
    {
        if (!stopSpawning)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= timeLimit)
            {
                stopSpawning = true;
                Debug.Log("Time limit reached. Stopping spawning.");
            }
        }
    }

    IEnumerator StartInitialLifecycle()
    {
        if (stopSpawning) yield break;
        SpawnEgg();
        yield return new WaitForSeconds(10);
        SpawnChick();
    }

    IEnumerator EggLifecycle()
    {
        if (stopSpawning) yield break;
        yield return new WaitForSeconds(10);
        SpawnChick();
    }

    void SpawnEgg()
    {
        if (stopSpawning) return;
        eggCount++;
        UpdateCountText();
        Instantiate(eggPrefab, GetFarSpawnPosition(), Quaternion.identity);
        Debug.Log($"Egg Count: {eggCount}");
        StartCoroutine(EggLifecycle());
    }

    void SpawnChick()
    {
        if (stopSpawning) return;
        // eggCount--; Commented out to prevent decrementing egg count prematurely
        chickCount++;
        UpdateCountText();
        Instantiate(chickPrefab, GetFarSpawnPosition(), Quaternion.identity);
        Debug.Log($"Chick Count: {chickCount}");
        StartCoroutine(ChickLifecycle());
    }

    IEnumerator ChickLifecycle()
    {
        if (stopSpawning) yield break;
        yield return new WaitForSeconds(10);
        if (Random.Range(0, 2) == 0)
        {
            SpawnHen();
        }
        else
        {
            SpawnRooster();
        }
    }

    void SpawnHen()
    {
        if (stopSpawning) return;
        chickCount--;
        henCount++;
        UpdateCountText();
        GameObject hen = Instantiate(henPrefab, GetFarSpawnPosition(), Quaternion.identity);
        Debug.Log($"Hen Count: {henCount}");
        StartCoroutine(HenLifecycle(hen));
    }

    void SpawnRooster()
    {
        if (stopSpawning) return;
        chickCount--;
        roosterCount++;
        UpdateCountText();
        GameObject rooster = Instantiate(roosterPrefab, GetFarSpawnPosition(), Quaternion.identity);
        Debug.Log($"Rooster Count: {roosterCount}");
        StartCoroutine(RoosterLifecycle(rooster));
    }

    IEnumerator HenLifecycle(GameObject hen)
    {
        if (stopSpawning) yield break;
        yield return new WaitForSeconds(30);
        int eggsLaid = Random.Range(2, 11);
        Debug.Log($"Hen Laying {eggsLaid} Eggs");
        for (int i = 0; i < eggsLaid; i++)
        {
            SpawnEgg();
        }

        yield return new WaitForSeconds(40); // 30s + 40s = 70s total hen lifespan
        henCount--;
        UpdateCountText();
        Debug.Log($"Hen Count: {henCount}");
        Destroy(hen);
    }

    IEnumerator RoosterLifecycle(GameObject rooster)
    {
        if (stopSpawning) yield break;
        yield return new WaitForSeconds(40);
        roosterCount--;
        UpdateCountText();
        Debug.Log($"Rooster Count: {roosterCount}");
        Destroy(rooster);
    }

    Vector3 GetFarSpawnPosition()
    {
        Vector3 spawnPosition;
        float y = 1f; // Adjust this value to set the height above ground level

        do
        {
            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float z = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            spawnPosition = new Vector3(x, y, z);
        } while (Vector3.Distance(spawnPosition, lastSpawnPosition) < 10);

        lastSpawnPosition = spawnPosition;
        return spawnPosition;
    }

    void UpdateCountText()
    {
        if (eggCountText != null)
            eggCountText.text = $"Eggs: {eggCount}";
        if (chickCountText != null)
            chickCountText.text = $"Chicks: {chickCount}";
        if (henCountText != null)
            henCountText.text = $"Hens: {henCount}";
        if (roosterCountText != null)
            roosterCountText.text = $"Roosters: {roosterCount}";
    }
}






























