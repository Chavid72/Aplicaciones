using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class APowerUp : MonoBehaviour
{
    private float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
        if (transform.position.y > 6f)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float newspeed)
    {
        speed = newspeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Interaction();
        }
    }

    public virtual void Interaction()
    {
        AudioManager.PlaySound(SoundType.PowerUp, 1f);
        Destroy(gameObject);
    }
}
