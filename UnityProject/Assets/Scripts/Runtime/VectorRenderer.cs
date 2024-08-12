using Nebula;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Note: Actual rendering is done inside <see cref="VectorRendererManager.OnRenderObject"/> for optimziation purposes.
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    public class VectorRendererData : MonoBehaviour, VectorRendererManager.IVectorRendererDataProvider
    {
        //this method allows for two separate colors to be drawn on one model
        //since different models may have different vertex numbers, I made these public so they
        //can be assigned in the editor
        [Tooltip("The first vertex to be drawn in the first color")]
        public int drawStart;
        [Tooltip("The last vertex to be drawn in the first color")]
        public int drawStop;

        [Tooltip("The first vertex to be draw in the second color")]
        public int drawStart2;
        [Tooltip("The last vertex to be drawin in the second color")]
        public int drawStop2;//stores the last vertex to be drawn in the second color

        private int _drawQueue = 0;


        public MeshFilter meshFilter { get; private set; }

        //This method uses materials to choose the colors
        //assign a color in the editor to each of these slots
        //the names are arbitrary, they both do the same thing,
        //but one will be used for the first set of vertices, and the other for the second
        public Material meshMaterial;
        public Material wireMaterial;

        public Vector3[] renderQueue;
        private int _myVertices;

        private Renderer _renderer;
        private Mesh _mesh;
        //Cached transform to avoid having to do an internal call.
        private Transform _transform;

        Matrix4x4 VectorRendererManager.IVectorRendererDataProvider.localToWorldMatrix => _transform.localToWorldMatrix;
        Renderer VectorRendererManager.IVectorRendererDataProvider.renderer => _renderer;
        Material VectorRendererManager.IVectorRendererDataProvider.meshMaterial => meshMaterial;
        Material VectorRendererManager.IVectorRendererDataProvider.wireMaterial => wireMaterial;
        Vector3[] VectorRendererManager.IVectorRendererDataProvider.renderQueue => renderQueue;
        int VectorRendererManager.IVectorRendererDataProvider.drawStart => drawStart;
        int VectorRendererManager.IVectorRendererDataProvider.drawStop => drawStop;
        int VectorRendererManager.IVectorRendererDataProvider.drawStart2 => drawStart2;
        int VectorRendererManager.IVectorRendererDataProvider.drawStop2 => drawStop2;

        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            _renderer = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            VectorRendererManager.instance.AddRenderer(this);
        }

        private void OnDisable()
        {
            VectorRendererManager.instance.RemoveRenderer(this);
        }

        //Start will initialize the mesh and determine the locations of each vertex
        void Start()
        {
            _mesh = meshFilter.mesh;
            _transform = transform;

            //CREATE AN ARRAY TO STORE VERTEX INFORMATION
            renderQueue = new Vector3[_mesh.vertexCount];
            _myVertices = _mesh.vertexCount;
            for (var i = 0; i < _myVertices; i += 1)
            {
                renderQueue[i] = _mesh.vertices[i];
            }
        }
    }
}