using Nebula;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Singleton class that manages rendering meshes in a Vector fashion.
    /// <br>Renderers can be added or removed using <see cref="AddRenderer(VectorRendererManager.IVectorRendererDataProvider)"/> and <see cref="RemoveRenderer(VectorRendererManager.IVectorRendererDataProvider)"/> respectively.</br>
    /// </summary>
    public class VectorRendererManager : SingletonMonoBehaviour<VectorRendererManager>
    {
        protected override bool destroyIfDuplicate => true;
        private List<IVectorRendererDataProvider> _dataProvider = new List<IVectorRendererDataProvider>(8);
        private int _drawQueue;

        /// <summary>
        /// Adds the <paramref name="provider"/> to the List of data providers.
        /// </summary>
        public void AddRenderer(IVectorRendererDataProvider provider)
        {
            _dataProvider.Add(provider);
        }

        /// <summary>
        /// Removes the <paramref name="provider"/> from the list of data providers.
        /// </summary>
        public void RemoveRenderer(IVectorRendererDataProvider provider)
        {
            _dataProvider.Remove(provider);
        }

        //This code, and the method "DoRender" where taken and modified from the code found in this blogpost, which is part of the Design Document's visual references: https://www.indiedb.com/games/paradox-vector/tutorials/making-a-modern-vector-graphics-game
        private void OnRenderObject()
        {
            for(int i = 0; i < _dataProvider.Count; i++)
            {
                DoRender(_dataProvider[i]);
            }
        }

        private void DoRender(IVectorRendererDataProvider provider)
        {
            if (true)
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
        /// Interface to declare that a class or a struct provides the necesary metadata for drawing a mesh in a vector style.
        /// </summary>
        public interface IVectorRendererDataProvider
        {
            Matrix4x4 localToWorldMatrix { get; }
            Renderer renderer { get; }
            Material meshMaterial { get; }
            Material wireMaterial { get; }
            Vector3[] renderQueue { get; }
            int drawStart { get; }
            int drawStop { get; }
            int drawStart2 { get; }
            int drawStop2 { get; }
        }
    }
}