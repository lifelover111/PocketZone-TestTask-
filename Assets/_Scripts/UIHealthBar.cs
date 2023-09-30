using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] bool fade = true;
    [SerializeField] float fadeTime = 5;

    IHealth target;
    GameObject healthLine;
    SpriteRenderer sRend;
    SpriteRenderer hsRend;
    float memorizedHealth;
    float memorizeTime;
    void Start()
    {
        target = transform.parent.GetComponent<IHealth>();
        healthLine = transform.GetChild(0).gameObject;
        sRend = GetComponent<SpriteRenderer>();
        hsRend = healthLine.GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (fade)
            Fade();
        healthLine.transform.localScale = Vector3.Lerp(healthLine.transform.localScale, new Vector3(target.health / target.maxHealth, 1, 1), 10 * Time.deltaTime);
        healthLine.transform.localPosition = Vector3.Lerp(healthLine.transform.localPosition, new Vector3(-((target.maxHealth - target.health) / target.maxHealth)/2, healthLine.transform.localPosition.y, healthLine.transform.localPosition.z), 10 * Time.deltaTime);
    }

    void Fade()
    {
        if (Time.time - memorizeTime >= fadeTime)
        {
            sRend.color = Color.Lerp(sRend.color, new Color(sRend.color.r, sRend.color.g, sRend.color.b, 0), Time.deltaTime);
            hsRend.color = Color.Lerp(hsRend.color, new Color(hsRend.color.r, hsRend.color.g, hsRend.color.b, 0), Time.deltaTime);
        }
        else
        {
            sRend.color = new Color(sRend.color.r, sRend.color.g, sRend.color.b, 1);
            hsRend.color = new Color(hsRend.color.r, hsRend.color.g, hsRend.color.b, 1);
        }
        if (memorizedHealth != target.health)
        {
            memorizedHealth = target.health;
            memorizeTime = Time.time;
        }
    }
}
