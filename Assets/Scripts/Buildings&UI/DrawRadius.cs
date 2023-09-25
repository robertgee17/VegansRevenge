using UnityEngine;
using System.Collections;

public class DrawRadius : MonoBehaviour
{
    public float thetaScale = 0.01f;
    private ComponentAttack attackComponent;
    private int Size;
    private LineRenderer LineDrawer;
    private float Theta = 0f;
    private float initTheta = 1.43f;

    void Start()
    {
        LineDrawer = GetComponent<LineRenderer>();
        LineDrawer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        LineDrawer.material.color = Color.magenta;
        attackComponent = GetComponent<ComponentAttack>();
        Size = (int)((1f / thetaScale) + 2f);
        LineDrawer.SetVertexCount(2*Size);
        
    }
    private void Update()
    {
        drawCircle(attackComponent.getAggroRadius(), attackComponent.getRange());
    }
    void drawCircle(float aggroRadius, float range)
    {
        Theta = initTheta;
        for (int i = 0; i < Size; i++)
        {
            Theta += (2.0f * Mathf.PI * thetaScale);
            float x = aggroRadius * Mathf.Cos(Theta);
            float z = aggroRadius * Mathf.Sin(Theta);
            LineDrawer.SetPosition(i, new Vector3(x + transform.position.x, 1, z + transform.position.z));
            x = range * Mathf.Cos(Theta);
            z = range * Mathf.Sin(Theta);
            LineDrawer.SetPosition(i+Size, new Vector3(x + transform.position.x, 1, z + transform.position.z));
        }
    }

}