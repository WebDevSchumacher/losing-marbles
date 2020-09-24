using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchController : MonoBehaviour
{
    public UnityEvent pressed;
    public UnityEvent released;
    public Material inactiveMaterial;
    public Material activeMaterial;

    Vector3 indentation;
    bool pressDown = false;
    bool pressUp = false;
    bool active = false;
    public float duration;
    float activeTimer = 0.0f;
    void Awake()
    {
        pressed = new UnityEvent();
        released = new UnityEvent();
        indentation = new Vector3(-10f * Time.deltaTime, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(pressDown){
            gameObject.transform.localScale += indentation;
            gameObject.transform.position += indentation / 105;
            if(gameObject.transform.localScale.x <= 1){
                pressDown = false;
            }
        }
        if(pressUp){
            gameObject.transform.localScale += -indentation;
            gameObject.transform.position += -indentation / 105;
            if(gameObject.transform.localScale.x >= 3){
                pressUp = false;
            }
        }
        if(active){
            activeTimer += Time.deltaTime;
            if(activeTimer >= duration){
                active = false;
                pressUp = true;
                gameObject.GetComponent<Renderer>().material = inactiveMaterial;
                released.Invoke();
            }
        }
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player"){
            pressDown = true;
            active = true;
            activeTimer = 0.0f;
            gameObject.GetComponent<Renderer>().material = activeMaterial;
            pressed.Invoke();
        }
    }
}
