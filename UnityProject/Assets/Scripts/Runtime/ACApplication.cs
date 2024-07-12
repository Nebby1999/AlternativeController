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
    public class ACApplication : ApplicationBehaviour<ACApplication>
    {
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

            while (!parallelCoroutineTask.IsDone)
                yield return null;
        }
    }
}