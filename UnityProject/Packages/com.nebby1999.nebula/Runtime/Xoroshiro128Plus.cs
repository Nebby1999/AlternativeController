using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using Random = UnityEngine.Random;

namespace Nebula
{
    /// <summary>
    /// Clase utilizada para generacion de numeros aleatorios, a diferencia del <see cref="UnityEngine.Random"/>, Xoroshiro128Plus permite la utilizacion de semillas y tener multiples instancias de generacion de numeros random.
    /// </summary>
    public class Xoroshiro128Plus
    {
        private class SplitMix64
        {
            public ulong x;

            public ulong Next()
            {
                long num1 = (long)(this.x += 11400714819323198485UL);
                long num2 = (num1 ^ (long)((ulong)num1 >> 30)) * -4658895280553007687L;
                long num3 = (num2 ^ (long)((ulong)num2 >> 27)) * -7723592293110705685L;
                return (ulong)num3 ^ (ulong)num3 >> 31;
            }
        }

        private ulong state0;
        private ulong state1;
        private static readonly SplitMix64 initializer = new SplitMix64();
        private const ulong JUMP0 = 16109378705422636197;
        private const ulong JUMP1 = 1659688472399708668;
        private static readonly ulong[] JUMP = new ulong[2]
        {
    16109378705422636197UL,
    1659688472399708668UL
        };
        private const ulong LONG_JUMP0 = 15179817016004374139;
        private const ulong LONG_JUMP1 = 15987667697637423809;
        private static readonly ulong[] LONG_JUMP = new ulong[2]
        {
    15179817016004374139UL,
    15987667697637423809UL
        };
        private static readonly int[] logTable256 = Xoroshiro128Plus.GenerateLogTable();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong RotateLeft(ulong x, int k) => x << k | x >> 64 - k;

        /// <summary>
        /// Constructor de una nueva clase random.
        /// </summary>
        /// <param name="seed">La semilla a usar para esta instancia</param>
        public Xoroshiro128Plus(ulong seed) => this.ResetSeed(seed);

        /// <summary>
        /// Constructor de una nueva clase random.
        /// 
        /// <br>El estado actual de <paramref name="other"/> sera copiado a esta nueva instancia.</br>
        /// </summary>
        /// <param name="other">La otra instancia el cual va a ser copiada</param>
        public Xoroshiro128Plus(Xoroshiro128Plus other)
        {
            this.state0 = other.state0;
            this.state1 = other.state1;
        }

        /// <summary>
        /// Reinicia la semilla de esta clase random.
        /// 
        /// <br>Dependiendo de la semilla los numeros random obtenidos siempre seran los mismos</br>
        /// </summary>
        /// <param name="seed">La semilla a usar</param>
        public void ResetSeed(ulong seed)
        {
            Xoroshiro128Plus.initializer.x = seed;
            this.state0 = Xoroshiro128Plus.initializer.Next();
            this.state1 = Xoroshiro128Plus.initializer.Next();
        }

        /// <summary>
        /// Consigue un valor aleatorio de tipo <see cref="ulong"/>
        /// </summary>
        public ulong Next()
        {
            ulong state0 = this.state0;
            ulong state1 = this.state1;
            long num = (long)state0 + (long)state1;
            ulong x = state1 ^ state0;
            this.state0 = (ulong)((long)Xoroshiro128Plus.RotateLeft(state0, 24) ^ (long)x ^ (long)x << 16);
            this.state1 = Xoroshiro128Plus.RotateLeft(x, 37);
            return (ulong)num;
        }

        public void Jump()
        {
            ulong num1 = 0;
            ulong num2 = 0;
            for (int index1 = 0; index1 < Xoroshiro128Plus.JUMP.Length; ++index1)
            {
                for (int index2 = 0; index2 < 64; ++index2)
                {
                    if (((long)Xoroshiro128Plus.JUMP[index1] & 1L) << index2 != 0L)
                    {
                        num1 ^= this.state0;
                        num2 ^= this.state1;
                    }
                    long num3 = (long)this.Next();
                }
            }
            this.state0 = num1;
            this.state1 = num2;
        }

        public void LongJump()
        {
            ulong num1 = 0;
            ulong num2 = 0;
            for (int index1 = 0; index1 < Xoroshiro128Plus.LONG_JUMP.Length; ++index1)
            {
                for (int index2 = 0; index2 < 64; ++index2)
                {
                    if (((long)Xoroshiro128Plus.LONG_JUMP[index1] & 1L) << index2 != 0L)
                    {
                        num1 ^= this.state0;
                        num2 ^= this.state1;
                    }
                    long num3 = (long)this.Next();
                }
            }
            this.state0 = num1;
            this.state1 = num2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double ToDouble01Fast(ulong x) => BitConverter.Int64BitsToDouble(4607182418800017408L | (long)(x >> 12));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double ToDouble01(ulong x) => (double)(x >> 11) * 1.11022302462516E-16;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float ToFloat01(uint x) => (float)(x >> 8) * 5.960464E-08f;

        /// <summary>
        /// Consigue un valor aleatorio de tipo <see cref="bool"/>
        /// </summary>
        public bool nextBool => this.nextLong < 0L;

        /// <summary>
        /// Consigue un valor aleatorio de tipo <see cref="uint"/>
        /// </summary>
        public uint nextUInt => (uint)this.Next();

        /// <summary>
        /// Consigue un valor aleatorio de tipo <see cref="int"/>
        /// </summary>
        public int nextInt => (int)this.Next();

        /// <summary>
        /// Consigue un valor aleatorio de tipo <see cref="float"/>
        /// </summary>
        public ulong nextULong => this.Next();

        /// <summary>
        /// Consigue un valor aleatorio de tipo <see cref="long"/>
        /// </summary>
        public long nextLong => (long)this.Next();

        /// <summary>
        /// Consigue un valor entre 0 y 1 de tipo <see cref="double"/>
        /// </summary>
        public double nextNormalizedDouble => Xoroshiro128Plus.ToDouble01Fast(this.Next());

        /// <summary>
        /// Consigue un valor entre 0 y 1 de tipo <see cref="float"/>
        /// </summary>
        public float nextNormalizedFloat => Xoroshiro128Plus.ToFloat01(this.nextUInt);

        /// <summary>
        /// Consigue un valor entre los valores <paramref name="minInclusive"/> y <paramref name="maxInclusive"/>
        /// </summary>
        /// <param name="minInclusive">El valor minimo del numero</param>
        /// <param name="maxInclusive">El valor maximo del numero</param>
        /// <returns>El numero aleatorio</returns>
        public float RangeFloat(float minInclusive, float maxInclusive) => minInclusive + (maxInclusive - minInclusive) * this.nextNormalizedFloat;

        /// <summary>
        /// Consigue un valor entre <paramref name="minInclusive"/> y el numero antes de <paramref name="maxExclusive"/>.
        /// <br></br>
        /// El valor maximo es exclusivo para facilitar el uso de obtener un elemento aleatorio de una coleccion
        /// </summary>
        /// <param name="minInclusive">El valor minimo del numero</param>
        /// <param name="maxExclusive">El valor maximo exclusivo del numero</param>
        /// <returns>El numero aleatorio</returns>
        public int RangeInt(int minInclusive, int maxExclusive) => minInclusive + (int)this.RangeUInt32Uniform((uint)(maxExclusive - minInclusive));

        /// <summary>
        /// Consigue un valor entre <paramref name="minInclusive"/> y el numero antes de <paramref name="maxExclusive"/>.
        /// <br></br>
        /// El valor maximo es exclusivo para facilitar el uso de obtener un elemento aleatorio de una coleccion
        /// </summary>
        /// <param name="minInclusive">El valor minimo del numero</param>
        /// <param name="maxExclusive">El valor maximo exclusivo del numero</param>
        /// <returns>El numero aleatorio</returns>
        public long RangeLong(long minInclusive, long maxExclusive) => minInclusive + (long)this.RangeUInt64Uniform((ulong)(maxExclusive - minInclusive));

        private ulong RangeUInt64Uniform(ulong maxExclusive)
        {
            if (maxExclusive == 0UL)
                throw new ArgumentOutOfRangeException("Range cannot have size of zero.");
            int num1 = 64 - Xoroshiro128Plus.CalcRequiredBits(maxExclusive);
            ulong num2;
            do
            {
                num2 = this.nextULong >> num1;
            }
            while (num2 >= maxExclusive);
            return num2;
        }

        private uint RangeUInt32Uniform(uint maxExclusive)
        {
            if (maxExclusive == 0U)
                throw new ArgumentOutOfRangeException("Range cannot have size of zero.");
            int num1 = 32 - Xoroshiro128Plus.CalcRequiredBits(maxExclusive);
            uint num2;
            do
            {
                num2 = this.nextUInt >> num1;
            }
            while (num2 >= maxExclusive);
            return num2;
        }

        private static int[] GenerateLogTable()
        {
            int[] logTable = new int[256];
            logTable[0] = logTable[1] = 0;
            for (int index = 2; index < 256; ++index)
                logTable[index] = 1 + logTable[index / 2];
            logTable[0] = -1;
            return logTable;
        }

        private static int CalcRequiredBits(ulong v)
        {
            int num = 0;
            while (v != 0UL)
            {
                v >>= 1;
                ++num;
            }
            return num;
        }

        private static int CalcRequiredBits(uint v)
        {
            int num = 0;
            while (v != 0U)
            {
                v >>= 1;
                ++num;
            }
            return num;
        }

        /// <summary>
        /// Equivalente a usar <see cref="UnityEngine.Random.insideUnitCircle"/>
        /// <br>Consigue un Vector2 el cual representa un punto aleatorio dentro o encima de un circulo de radio 1</br>
        /// </summary>
        /// <returns>El punto aleatorio</returns>
        public Vector2 InsideUnitCircle()
        {
            float x = nextNormalizedFloat;
            float y = nextNormalizedFloat;
            return new Vector2(x, y);
        }


        /// <summary>
        /// Equivalente a usar <see cref="UnityEngine.Random.insideUnitSphere"/>
        /// <br>Consigue un Vector3 el cual representa un punto aleatorio dentro o encima de una esfera de radio 1</br>
        /// </summary>
        /// <returns>El punto aleatorio</returns>
        public Vector3 InsideUnitSphere()
        {
            float x = nextNormalizedFloat;
            float y = nextNormalizedFloat;
            float z = nextNormalizedFloat;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Equivalente a usar <see cref="UnityEngine.Random.onUnitSphere"/>
        /// <br>Consigue un Vector3 el cual representa un punto aleatorio encima de una esfera de radio 1</br>
        /// </summary>
        /// <returns></returns>
        public Vector3 OnUnitSphere()
        {
            var previousState = Random.state;
            Random.InitState(nextInt);
            Vector3 result = Random.onUnitSphere;
            Random.state = previousState;
            return result;
        }

        /// <summary>
        /// Consigue un elemento aleatorio dentro del arreglo deseado
        /// </summary>
        /// <typeparam name="T">El tipo de elemento</typeparam>
        /// <param name="array">El arreglo en si</param>
        /// <returns>Un elemento aleatorio</returns>
        public ref T NextElementUniform<T>(ref T[] array) => ref array[this.RangeInt(0, array.Length)];

        /// <summary>
        /// Consigue un elemento aleatorio dentro de la lista deseada
        /// </summary>
        /// <typeparam name="T">El tipo de elemento</typeparam>
        /// <param name="list">El arreglo en si</param>
        /// <returns>Un elemento aleatorio</returns>
        public T NextElementUniform<T>(List<T> list) => list[this.RangeInt(0, list.Count)];

        /// <summary>
        /// Consigue un elemento aleatorio dentro de la lista deseada
        /// </summary>
        /// <typeparam name="T">El tipo de elemento</typeparam>
        /// <param name="list">El arreglo en si</param>
        /// <returns>Un elemento aleatorio</returns>
        public T NextElementUniform<T>(IList<T> list) => list[this.RangeInt(0, list.Count)];

        /// <summary>
        /// Consigue un elemento aleatorio dentro del arreglo deseado, y elimina el elemento de dicho arreglo
        /// </summary>
        /// <typeparam name="T">El tipo de elemento</typeparam>
        /// <param name="list">El arreglo en si</param>
        /// <returns>Un elemento aleatorio</returns>
        public T RetrieveAndRemoveNextElementUniform<T>(ref T[] array)
        {
            int index = RangeInt(0, array.Length);
            T element = array[index];
            ArrayUtils.RemoveAtAndResize(ref array, index, 1);
            return element;
        }

        /// <summary>
        /// Consigue un elemento aleatorio dentro de la lista deseada, y elimina el elemento de dicha lista
        /// </summary>
        /// <typeparam name="T">El tipo de elemento</typeparam>
        /// <param name="list">El arreglo en si</param>
        /// <returns>Un elemento aleatorio</returns>
        public T RetrieveAndRemoveNextElementUniform<T>(List<T> list)
        {
            int index = RangeInt(0, list.Count);
            T element = list[index];
            list.RemoveAt(index);
            return element;
        }

        /// <summary>
        /// Consigue un elemento aleatorio dentro de la lista deseada, y elimina el elemento de dicha lista
        /// </summary>
        /// <typeparam name="T">El tipo de elemento</typeparam>
        /// <param name="list">El arreglo en si</param>
        /// <returns>Un elemento aleatorio</returns>
        public T RetrieveAndRemoveNextElementUniform<T>(IList<T> list)
        {
            int index = RangeInt(0, list.Count);
            T element = list[index];
            list.RemoveAt(index);
            return element;
        }
    }
}