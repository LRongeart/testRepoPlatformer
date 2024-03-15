using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private Collider2D col;
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetAxisRaw("Vertical") < 0)
        {
            StartCoroutine(ChangeColliderState());
        }
    }

    private IEnumerator ChangeColliderState()
    {
        col.enabled = false;
        yield return new WaitForSeconds(1f);
        col.enabled = true;
    }
}
