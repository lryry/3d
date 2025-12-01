using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Sprite OpenSprite;
    public Sprite ClosedSprite;
    public bool CollisionToggle;

    public enum TOGGLE
    {
        OPEN = 0,
        CLOSED = 1,
    }
    public TOGGLE Toggle;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (Toggle == TOGGLE.CLOSED)
        {
            spriteRenderer.sprite = ClosedSprite;
            if (CollisionToggle) { gameObject.GetComponent<Collider2D>().enabled = true; }
        }
        else if (Toggle == TOGGLE.OPEN)
        {
            spriteRenderer.sprite = OpenSprite;
            if (CollisionToggle) { gameObject.GetComponent<Collider2D>().enabled = false; }
        }
    }

    public void ToggleObject()
    {
        if (Toggle == TOGGLE.OPEN)
        {
            Toggle = TOGGLE.CLOSED;

            spriteRenderer.sprite = ClosedSprite;
            if (CollisionToggle) { gameObject.GetComponent<Collider2D>().enabled = true; }
        }
        else if (Toggle == TOGGLE.CLOSED)
        {
            Toggle = TOGGLE.OPEN;

            spriteRenderer.sprite = OpenSprite;
            if (CollisionToggle) { gameObject.GetComponent<Collider2D>().enabled = false; }
        }
    }
}