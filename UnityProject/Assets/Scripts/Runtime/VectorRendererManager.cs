using Nebula;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Clase Singleton que maneja el renderizado de meshes en un estilo Vectorial.
    /// <br>Renderes se pueden agregar o quitar usando <see cref="AddRenderer(VectorRendererManager.IVectorRendererDataProvider)"/> y <see cref="RemoveRenderer(VectorRendererManager.IVectorRendererDataProvider)"/> respectivamente.</br>
    /// </summary>
    public class VectorRendererManager : SingletonMonoBehaviour<VectorRendererManager>
    {
        protected override bool destroyIfDuplicate => true;
        private List<IVectorRendererDataProvider> _dataProvider = new List<IVectorRendererDataProvider>(8);
        private int _drawQueue;

        /// <summary>
        /// Añade <paramref name="provider"/> a la lista de proveedores de data.
        /// </summary>
        public void AddRenderer(IVectorRendererDataProvider provider)
        {
            _dataProvider.Add(provider);
        }

        /// <summary>
        /// Quita <paramref name="provider"/> de la lista de proveedores de data.
        /// </summary>
        public void RemoveRenderer(IVectorRendererDataProvider provider)
        {
            _dataProvider.Remove(provider);
        }

        //Este codigo y el metodo "DoRender" fueron sacados y modificados del codigo de este blogpost, el cual es parte del DesignDocument.: https://www.indiedb.com/games/paradox-vector/tutorials/making-a-modern-vector-graphics-game
        private void OnRenderObject()
        {
            for(int i = 0; i < _dataProvider.Count; i++)
            {
                DoRender(_dataProvider[i]);
            }
        }

        private void DoRender(IVectorRendererDataProvider provider)
        {
            if (provider.renderer.isVisible)
            {
                GL.MultMatrix(provider.localToWorldMatrix);

                //Begin drawing first set of lines
                GL.Begin(GL.LINES);
                provider.meshMaterial.SetPass(0);
                GL.Color(new Color(0f, 0f, 0f, 1f));

                _drawQueue = provider.drawStart;
                while (_drawQueue < provider.drawStop)
                {
                    GL.Vertex3(provider.renderQueue[_drawQueue].x, provider.renderQueue[_drawQueue].y, provider.renderQueue[_drawQueue].z);
                    GL.Vertex3(provider.renderQueue[_drawQueue + 1].x, provider.renderQueue[_drawQueue + 1].y, provider.renderQueue[_drawQueue + 1].z);

                    _drawQueue += 1;
                }

                GL.End();

                //begin drawing second set of lines

                GL.Begin(GL.LINES);
                provider.wireMaterial.SetPass(0);
                GL.Color(new Color(0f, 0f, 0f, 1f));

                _drawQueue = provider.drawStart2;
                while (_drawQueue < provider.drawStop2 - 1)
                {
                    GL.Vertex3(provider.renderQueue[_drawQueue].x, provider.renderQueue[_drawQueue].y, provider.renderQueue[_drawQueue].z);
                    GL.Vertex3(provider.renderQueue[_drawQueue + 1].x, provider.renderQueue[_drawQueue + 1].y,  provider.   renderQueue[_drawQueue + 1].z);

                    _drawQueue += 1;
                }
                GL.End();
            }
        }

        /// <summary>
        /// Interfaz que declara que una clase o estructura provee la metadata necesaria para dibujar un Mesh en vectores.
        /// </summary>
        public interface IVectorRendererDataProvider
        {
            /// <summary>
            /// La matriz usada para renderizar
            /// </summary>
            Matrix4x4 localToWorldMatrix { get; }
            /// <summary>
            /// El renderer que vamos a renderizar
            /// </summary>
            Renderer renderer { get; }
            /// <summary>
            /// El material que se usara en el mesh en si
            /// </summary>
            Material meshMaterial { get; }
            /// <summary>
            /// El material que se usara para los "cables" de vectores.
            /// </summary>
            Material wireMaterial { get; }
            Vector3[] renderQueue { get; }
            int drawStart { get; }
            int drawStop { get; }
            int drawStart2 { get; }
            int drawStop2 { get; }
        }
    }
}