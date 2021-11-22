using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{   

    Mesh mesh;
    public Vector3[] vertices;
    int[] triangles;
    private int nCubos = 0;
    private int[] indicesAux = new int[8];
    private float anchoLlantas = 0.15f;
    private float anchoCarro = 0.5f;
    private float largoCarro = 1.2f;
    private float radioLlantas = 0.3f;
    private float espacioLlantas = 0.05f;
    private int cubos = 5;
    public int n;
    private int angle;
    private float radioPivote = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        angle = 0;

        CreateShape();
        UpdateMesh();
    }


    void CreateShape()
    {
        vertices = new Vector3[cubos * 8];
        triangles = new int[cubos * 36];
        n = cubos * 8;
        
        CreateCubo(-(anchoCarro/2) - anchoLlantas, 0, -(largoCarro/2) + espacioLlantas, anchoLlantas, radioLlantas, radioLlantas);
        CreateCubo(-(anchoCarro/2) - anchoLlantas, 0, (largoCarro/2) - espacioLlantas - radioLlantas, anchoLlantas, radioLlantas, radioLlantas);
        CreateCubo((anchoCarro/2), 0, -(largoCarro/2) + espacioLlantas, anchoLlantas, radioLlantas, radioLlantas);
        CreateCubo((anchoCarro/2), 0, (largoCarro/2) - espacioLlantas - radioLlantas, anchoLlantas, radioLlantas, radioLlantas);
        CreateCubo(-(anchoCarro/2), 0.1f, -(largoCarro/2), anchoCarro, largoCarro, anchoCarro);

    }

    void CreateCubo(float origenX, float origenY, float origenZ, float ancho, float largo, float alto)
    {
        int aux = nCubos * 8;

        for(int i = 0; i < 8; i++){
            indicesAux[i] = aux + i;
        }

        vertices[indicesAux[0]] = new Vector3(origenX, origenY, origenZ);
        vertices[indicesAux[1]] = new Vector3(origenX, origenY + alto, origenZ);
        vertices[indicesAux[2]] = new Vector3(origenX + ancho, origenY + alto, origenZ);
        vertices[indicesAux[3]] = new Vector3(origenX + ancho, origenY, origenZ);
        vertices[indicesAux[4]] = new Vector3(origenX, origenY, origenZ + largo);
        vertices[indicesAux[5]] = new Vector3(origenX, origenY + alto, origenZ + largo);
        vertices[indicesAux[6]] = new Vector3(origenX + ancho, origenY + alto, origenZ + largo);
        vertices[indicesAux[7]] = new Vector3(origenX + ancho, origenY, origenZ + largo);

        aux = nCubos * 36;
        triangles[aux] = indicesAux[0];
        triangles[aux + 1] = indicesAux[1];
        triangles[aux + 2] = indicesAux[2];
        triangles[aux + 3] = indicesAux[2];
        triangles[aux + 4] = indicesAux[3];
        triangles[aux + 5] = indicesAux[0];
        triangles[aux + 6] = indicesAux[4];
        triangles[aux + 7] = indicesAux[5];
        triangles[aux + 8] = indicesAux[1];
        triangles[aux + 9] = indicesAux[1];
        triangles[aux + 10] = indicesAux[0];
        triangles[aux + 11] = indicesAux[4];
        triangles[aux + 12] = indicesAux[1];
        triangles[aux + 13] = indicesAux[5];
        triangles[aux + 14] = indicesAux[6];
        triangles[aux + 15] = indicesAux[6];
        triangles[aux + 16] = indicesAux[2];
        triangles[aux + 17] = indicesAux[1];
        triangles[aux + 18] = indicesAux[2];
        triangles[aux + 19] = indicesAux[6];
        triangles[aux + 20] = indicesAux[7];
        triangles[aux + 21] = indicesAux[7];
        triangles[aux + 22] = indicesAux[3];
        triangles[aux + 23] = indicesAux[2];
        triangles[aux + 24] = indicesAux[7];
        triangles[aux + 25] = indicesAux[6];
        triangles[aux + 26] = indicesAux[5];
        triangles[aux + 27] = indicesAux[5];
        triangles[aux + 28] = indicesAux[4];
        triangles[aux + 29] = indicesAux[7];
        triangles[aux + 30] = indicesAux[0];
        triangles[aux + 31] = indicesAux[3];
        triangles[aux + 32] = indicesAux[4];
        triangles[aux + 33] = indicesAux[4];
        triangles[aux + 34] = indicesAux[3];
        triangles[aux + 35] = indicesAux[7];

        nCubos++;

    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    void TransformCar()
    {   
        
        //int n = vertices.Length;
        int i;
        Vector4[] vs = new Vector4[n];
        Vector3[] final = new Vector3[n];
        for(i=0; i < n; i++)
        {
            vs[i] = vertices[i];
            vs[i].w = 1.0f;
        }
        
        Matrix4x4 transform2 = Transformations.RotateM(angle, Transformations.AXIS.AX_Y);
        Matrix4x4 transform1 = Transformations.TranslateM(0, 0.0f, radioPivote);
        Matrix4x4 transform3 = Transformations.TranslateM(0, 0.0f, -radioPivote);
        
        for (i = 0; i < n; i++)
        {
            vs[i] = transform3 * transform2 * transform1 * vs[i];
            final[i] = vs[i];
        }
        
        GetComponent<MeshFilter>().mesh.vertices = final;

    }
    /*
    void Update()
    {
        angle += 0.3f;
        if (angle > 90){
            angle = 0;
        }
        TransformCar();
    }
    */
}
