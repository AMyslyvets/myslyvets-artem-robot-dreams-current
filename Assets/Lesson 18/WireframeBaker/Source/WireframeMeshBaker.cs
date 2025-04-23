using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Wireframe
{
    public class WireframeMeshBaker : MonoBehaviour
    {
        [SerializeField] private Mesh _mesh;
        [SerializeField] private bool _toSubMesh;

        [ContextMenu("Generate Mesh")]
        private void GenerateMesh()
        {
            #if UNITY_EDITOR
            Mesh mesh = Instantiate(_mesh);
            int[] triangles = mesh.triangles;
            int[] wires = new int[triangles.Length * 2];

            int triangleCount = triangles.Length / 3;
            for (int i = 0; i < triangleCount; ++i)
            {
                int wireOffset = i * 6;
                int triangleOffset = i * 3;
                
                wires[wireOffset + 0] = triangles[triangleOffset + 0];
                wires[wireOffset + 1] = triangles[triangleOffset + 1];
                wires[wireOffset + 2] = triangles[triangleOffset + 1];
                wires[wireOffset + 3] = triangles[triangleOffset + 2];
                wires[wireOffset + 4] = triangles[triangleOffset + 2];
                wires[wireOffset + 5] = triangles[triangleOffset + 0];
            }

            if (_toSubMesh)
            {
                mesh.subMeshCount = 2;
                mesh.SetIndices(wires, MeshTopology.Lines, 1);
            }
            else
            {
                mesh.SetIndices(wires, MeshTopology.Lines, 0);
            }

            mesh.UploadMeshData(false);
            
            string path = EditorUtility.SaveFilePanelInProject("Save mesh", mesh.name, "asset", "Save mesh");
            
            AssetDatabase.CreateAsset(mesh, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            #endif
        }
    }
}