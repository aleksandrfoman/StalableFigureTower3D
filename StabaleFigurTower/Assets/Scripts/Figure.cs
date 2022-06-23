using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure : MonoBehaviour
{
    [SerializeField]
    private new Rigidbody rigidbody;
    [SerializeField]
    private Collider[] colliders;
    [SerializeField]
    private Renderer[] renderers;
    [SerializeField]
    private Material figureMaterial;
    public Material FigureMaterial => figureMaterial;
    [SerializeField]
    private bool isSelectableObject;
    public bool IsSelectableObject => isSelectableObject;

    public bool IsCollEnter()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.forward, out hit,Mathf.Infinity);

        // 7f волшебное число высота уровня в будующем нужно получать откуда-то можно получать текщей самой высокой фигуры или т.п

        if (hit.transform == null && transform.position.y >= 7f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool IsStabilityFiguree()
    {
        if(Mathf.Abs(rigidbody.velocity.magnitude)<0.005f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChangeSelectable(bool value)
    {
        isSelectableObject = value;
    }
    public void DisableObject()
    {
        AcitavateColliders(false);
    }
    public void ActiavateObject()
    {
        AcitavateColliders(true);
        rigidbody.isKinematic = false;
    }

    private void AcitavateColliders(bool value)
    {
        foreach (var col in colliders)
        {
            col.enabled = value;
        }
    }

    public void ChangeRendereMat(Material curMat)
    {
        foreach (var rend in renderers)
        {
            rend.material = curMat;
        }
    }
}
