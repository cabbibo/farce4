using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.XR;
using UnityEngine.XR.FaceSubsystem;
using UnityEngine.XR.ARFoundation;

public class FaceController : MonoBehaviour
{
        

  public ARFace face;
  public List<int> indices;
  public List<Vector3> vertices;
  public List<Vector2> uvs;
  public bool topologyUpdatedThisFrame;
  public Mesh mesh;

  public MakeFaceMesh makeFaceMesh;

        void Awake()
        {
            mesh = new Mesh();
//            m_MeshRenderer = GetComponent<MeshRenderer>();
            face = GetComponent<ARFace>();
            indices = new List<int>();
            vertices = new List<Vector3>();
            uvs = new List<Vector2>();
            makeFaceMesh = GameObject.FindGameObjectWithTag("FACE").GetComponent<MakeFaceMesh>();
            print("FACE MESH FOUND: " + makeFaceMesh);

            face.updated += OnUpdated;
            ((MutatingVerts)makeFaceMesh.verts).mesh = mesh;
            makeFaceMesh.faceController = this;

        }


        void OnUpdated(ARFace face)
        {
            if (!topologyUpdatedThisFrame)
            {
                //print("setting Topo");
                SetMeshTopology();
                makeFaceMesh.transform.position = face.xrFace.pose.position;
                makeFaceMesh.transform.rotation = face.xrFace.pose.rotation;
                makeFaceMesh.Set();
            }

            topologyUpdatedThisFrame = false;
        }


        void SetMeshTopology()
        {
            if (mesh == null)
            {
                print("No Mesh");
                return;
            }


            bool canDo = true;

            if (face.TryGetFaceMeshVertices(vertices) && face.TryGetFaceMeshIndices(indices)){
           
            }else{
              canDo = false;print("No Face");
            }

            if (face.TryGetFaceMeshUVs(uvs))
            {
                
            }else{
              canDo = false; print("No UVs");
            }
            

            if( canDo ){

              mesh.Clear();

              mesh.SetVertices(vertices);
              mesh.SetTriangles(indices, 0);
              mesh.SetUVs(0, uvs);
              mesh.RecalculateBounds();
              mesh.RecalculateNormals();
              mesh.RecalculateTangents();
            }
          
            topologyUpdatedThisFrame = true;
        }
}
