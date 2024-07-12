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