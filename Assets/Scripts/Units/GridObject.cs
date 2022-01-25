using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GridObject : MonoBehaviour
{
    public virtual void Hover()
    {
        //throw new System.NotImplementedException();
        Debug.Log($"Hover {name}");
    }
    public virtual void Unhover()
    {
        //throw new System.NotImplementedException();
        Debug.Log($"Unhover {name}");
    }

    public virtual void Select()
    {
        //throw new System.NotImplementedException();
        Debug.Log($"Select {name}");
    }
    public virtual void Deselect()
    {
        //throw new System.NotImplementedException();
        Debug.Log($"Deselect {name}");
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}