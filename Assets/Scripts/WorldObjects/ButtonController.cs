using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    public UnityEvent buttonPressed;
    public Material activeMaterial;

    Vector3 indentation;
    Vector3 raise;
    bool pressDown = false;
    bool raiseGate = false;
    private Transform gate;

    void Awake()
    {
        gate = gameObject.transform.parent.Find("WallFront");
        buttonPressed = new UnityEvent();
        indentation = new Vector3(-0.2f * Time.deltaTime, 0, 0);
        raise = new Vector3(0, -0.6f * Time.deltaTime, 0);
    }

    void Update()
    {
        if (pressDown)
        {
            gameObject.transform.localScale += indentation;
            gameObject.transform.position -= indentation / 5;
            if (gameObject.transform.localScale.x <= 0.1f)
            {
                pressDown = false;
            }
        }

        if (raiseGate)
        {
            gate.localScale += raise;
            gate.position -= raise;
            if (gate.position.y >= 2.8f)
            {
                raiseGate = false;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            pressDown = true;
            raiseGate = true;
            gameObject.GetComponent<Renderer>().material = activeMaterial;
            buttonPressed.Invoke();
        }
    }
}