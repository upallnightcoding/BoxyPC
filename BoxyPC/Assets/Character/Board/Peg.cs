using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peg : MonoBehaviour
{
    public Sprite pegOpen;
    public Sprite pegSelected;
    public Sprite pegClosed;
    public Sprite pegIllegal;

    public float X
    {
        get { return gameObject.transform.position.x; }
    }

    public float Y
    {
        get { return gameObject.transform.position.y; }
    }

    public Vector3 GetPosition
    {
        get { return gameObject.transform.position; }
    }

    public bool North { get; set; }
    public bool South { get; set; }
    public bool West { get; set; }
    public bool East { get; set; }

    public int MaxLinks { get; set; }

    //private Renderer pegRenderer;
    private SpriteRenderer spriteRenderer;

    private bool selected;

    // Start is called before the first frame update
    void Start()
    {
        selected = false;

        North = false;
        South = false;
        East = false;
        West = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        //pegRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Selected()
    {
        spriteRenderer.sprite = pegSelected;
        selected = true;
    }

    public void Reset()
    {
        if (IsOpen())
        {
            spriteRenderer.sprite = pegOpen;
        }
        else
        {
            spriteRenderer.sprite = pegClosed;
        }
    }

    public void Illegal()
    {
        if (!selected) { 
            spriteRenderer.sprite = pegIllegal;
        }
    }

    public void OnMouseExit()
    {
        if (!selected)
        {
            Reset();
        }

        selected = false;
    }

    public void DisplayCount()
    {
        int count = 0;

        if (North) count++;
        if (South) count++;
        if (East) count++;
        if (West) count++;

        //Debug.Log($"X,Y: {X},{Y} {North}/{South}/{East}/{West} Count: {count}:{MaxLinks}");
    }

    public bool IsOpen()
    {
        int count = 0;

        if (North) count++;
        if (South) count++;
        if (East) count++;
        if (West) count++;

        return (count < MaxLinks);
    }
}
