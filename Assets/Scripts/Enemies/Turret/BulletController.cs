using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    bool isFired;
    Vector3 aimDirection;

    public int speed;

    void Update()
    {
        if(isFired){
            transform.position += aimDirection * speed * Time.deltaTime;
        }     
    }

    public void FireBullet(GameObject turret, GameObject target){
        if(turret && target){
            aimDirection = (target.transform.position - turret.transform.position).normalized;
            isFired = true;
        }
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player"){
            GameObject player = other.gameObject;
            player.GetComponent<PlayerHitController>().hit.Invoke();
            Destroy(this.gameObject);
        }
    }
}
