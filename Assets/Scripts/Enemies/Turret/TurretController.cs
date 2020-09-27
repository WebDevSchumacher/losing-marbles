using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public GameObject barrel;
    public GameObject bulletObj;
    public float fireRate;

    List<GameObject> firedBullets = new List<GameObject>();
    float reloadTimer = 0.0f;
    bool active;
    GameObject player;

    void Start()
    {
        active = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().holdGame.AddListener(Shutdown);
        GameObject.Find("Switch").GetComponent<SwitchController>().pressed.AddListener(Deactivate);
        GameObject.Find("Switch").GetComponent<SwitchController>().released.AddListener(Activate);
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (!player)
        {
            player = GameObject.Find("Player");
        }
        else
        {
            reloadTimer += Time.deltaTime;
            transform.Find("Cupola").transform.LookAt(player.transform);
            if (active && reloadTimer >= fireRate)
            {
                FireBullet();
                reloadTimer = 0.0f;
            }
        }
    }

    void FireBullet()
    {
        if (!bulletObj)
        {
            return;
        }

        firedBullets.Clear();
        if (barrel)
        {
            GameObject bullet = Instantiate(bulletObj, barrel.transform.position,
                Quaternion.Euler(barrel.transform.forward));
            bullet.GetComponent<BulletController>().FireBullet(barrel, player);
            firedBullets.Add(bullet);
        }
    }

    void Deactivate()
    {
        active = false;
    }

    void Activate()
    {
        active = true;
    }

    void Shutdown()
    {
        Deactivate();
        foreach (GameObject bullet in firedBullets)
        {
            Destroy(bullet);
        }
    }
}