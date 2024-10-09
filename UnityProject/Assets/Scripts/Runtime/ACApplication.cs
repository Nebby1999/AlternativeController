using Nebula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// La Aplicacion principal del juego, contiene informacion vital para la ejecuccion de este mismo junto con manejar la carga de contenido de este mismo
    /// 
    /// <br>Mira tambien <see cref="Nebula.ApplicationBehaviour{T}"/></br>
    /// </summary>
    public class ACApplication : ApplicationBehaviour<ACApplication>
    {
        /// <summary>
        /// Un generador de numeros random usado por la aplicacion
        /// </summary>
        public Xoroshiro128Plus applicationRNG { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            applicationRNG = new Xoroshiro128Plus((ulong)DateTime.Now.Ticks);
        }

        protected override IEnumerator C_LoadGameContent()
        {
            var parallelCoroutineTask = new ParallelCoroutineTask();
            parallelCoroutineTask.Add(EntityStateCatalog.Initialize());
            parallelCoroutineTask.Add(ResourceCatalog.Initialize());

            while (!parallelCoroutineTask.isDone)
                yield return null;
        }
    }
}