using Nebula;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Representa una data de Vectores que es renderizado usando <see cref="VectorRendererManager"/>
    /// Nota: el renderizado en si se ejecuta en <see cref="VectorRendererManager.OnRenderObject"/> por motivos de optimizacion.
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    public class VectorRendererData : MonoBehaviour, VectorRendererManager.IVectorRendererDataProvider
    {
        [Tooltip("El primer vertice a dibujar con el primer color.")]
        public int drawStart;
        [Tooltip("El ultimo vertex a dibujar con el primer color")]
        public int drawStop;

        [Tooltip("El primer vertice a dibujar con el segundo color.")]
        public int drawStart2;
        [Tooltip("El ultimo vertex a dibujar con el segundo color")]
        public int drawStop2;//stores the last vertex to be drawn in the second color

        private int _drawQueue = 0;

        /// <summary>
        /// El mesh filter el cual contiene el Mesh a renderizar
        /// </summary>
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