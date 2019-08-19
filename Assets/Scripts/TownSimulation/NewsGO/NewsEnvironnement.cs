using UnityEngine;

/*
 * This script handle the foggy environnement when you open a news item.
 * It's a big white transparent sphere attach to your head, you can change the size with the float sizeOfNewsEnvironnement.
 * We use FlipNormals to see the sphere from the inside.
 * 
*/
namespace Assets.Scripts.TownSimulation.NewsGO
{
    public class NewsEnvironnement : MonoBehaviour
    {

        public float sizeOfNewsEnvironnement;

        private GameObject HeadCollider;

        // Use this for initialization
        void Start()
        {

            HeadCollider = GameObject.Find("HeadCollider");

            transform.localScale = new Vector3(sizeOfNewsEnvironnement, sizeOfNewsEnvironnement, sizeOfNewsEnvironnement);
            FlipNormals();
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = HeadCollider.transform.position;
        }

        void FlipNormals()
        {
            MeshFilter filter = GetComponent(typeof(MeshFilter)) as MeshFilter;
            if (filter != null)
            {
                Mesh mesh = filter.mesh;

                Vector3[] normals = mesh.normals;
                for (int i = 0; i < normals.Length; i++)
                    normals[i] = -normals[i];
                mesh.normals = normals;

                for (int m = 0; m < mesh.subMeshCount; m++)
                {
                    int[] triangles = mesh.GetTriangles(m);
                    for (int i = 0; i < triangles.Length; i += 3)
                    {
                        int temp = triangles[i + 0];
                        triangles[i + 0] = triangles[i + 1];
                        triangles[i + 1] = temp;
                    }
                    mesh.SetTriangles(triangles, m);
                }
            }
        }
    }
}
