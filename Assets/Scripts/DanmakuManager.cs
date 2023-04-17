using System.Collections;
using UnityEngine;

public class DanmakuManager : SingletonManager<DanmakuManager>
{
    public GameObject bulletPrefab;
    public GameObject wheel;
    public GameObject playerObject;

    public float initSpawnRate;
    public float spawnRateIncrease; // unit: second
    public float maxInitVelocity;
    public float minInitVelocity;
    public float maxVelocityIncrease; 
    public float minVelocityIncrease;
    public float minScale;
    public float maxScale;

    private float minVelocity;
    private float maxVelocity;
    private Player player;

    protected override void Awake()
    {
        base.Awake();
        player=playerObject.GetComponent<Player>();
    }

    private void Start()
    {
        StartCoroutine(SpawnBullets());
    }

    private IEnumerator SpawnBullets()
    {
        while (true)
        {
            SpawnBullet();
            float spawnRate = initSpawnRate + spawnRateIncrease * player.SurviveTime;
            yield return new WaitForSeconds(1f / spawnRate);
        }
    }

    private void SpawnBullet()
    {
        minVelocity=minInitVelocity+minVelocityIncrease * player.SurviveTime;
        maxVelocity=maxInitVelocity+maxVelocityIncrease * player.SurviveTime;

        Vector2 direction = Random.insideUnitCircle.normalized;
        Vector2 velocity = direction * Random.Range(minVelocity, maxVelocity);
        Vector3 scale = Vector3.one * Random.Range(minScale, maxScale);

        GameObject bullet = Instantiate(bulletPrefab, wheel.transform.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Velocity = velocity;
        bulletScript.Scale = scale;
        bulletScript.MyColor = (MyColor)Random.Range(0, 2);
        bulletScript.Init();

    }
}
