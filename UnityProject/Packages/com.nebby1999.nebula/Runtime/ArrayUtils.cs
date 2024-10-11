using System.Collections.Generic;
using System;

namespace Nebula
{
    /// <summary>
    /// Clase con utilidades para manipular arreglos
    /// </summary>
    public static class ArrayUtils
    {
        /// <summary>
        /// Inserta <paramref name="tElement"/> en el arreglo <paramref name="array"/>. <paramref name="arraySize"/> debe ser el tamaño del arreglo y <paramref name="position"/> la posicion para el nuevo elemento.
        /// </summary>
        public static void InsertNoResize<T>(ref T[] array, int arraySize, int position, in T tElement)
        {
            for (int num = arraySize - 1; num > position; num--)
            {
                array[num] = array[num - 1];
            }
            array[position] = tElement;
        }

        /// <summary>
        /// Inserta <paramref name="tElement"/> en el arreglo <paramref name="array"/>, el tamaño del arreglo es modificado para acomodar el nuevo elemento. y el elemento es colocado en la posicion <paramref name="position"/>
        /// </summary>
        public static void Insert<T>(ref T[] array, ref int arraySize, int position, in T tElement)
        {
            arraySize++;
            if (arraySize > array.Length)
            {
                Array.Resize(ref array, arraySize);
            }
            InsertNoResize(ref array, arraySize, position, in tElement);
        }

        /// <summary>
        /// Inserta <paramref name="value"/> en el arreglo <paramref name="array"/> en posicion <paramref name="position"/>
        /// </summary>
        public static void Insert<T>(ref T[] array, int position, in T value)
        {
            int arraySize = array.Length;
            Insert(ref array, ref arraySize, position, in value);
        }

        /// <summary>
        /// Inserta <paramref name="tElement"/> en el arreglo <paramref name="tArray"/> al final de este.
        /// </summary>
        /// <param name="arraySize">el tamaño de <paramref name="tArray"/></param>
        public static void Append<T>(ref T[] tArray, ref int arraySize, in T tElement)
        {
            Insert(ref tArray, ref arraySize, arraySize, in tElement);
        }

        /// <summary>
        /// Inserta <paramref name="tElement"/> en el arreglo <paramref name="tArray"/> al final de este
        /// </summary>
        public static void Append<T>(ref T[] tArray, in T tElement)
        {
            int arraySize = tArray.Length;
            Append(ref tArray, ref arraySize, tElement);
        }

        /// <summary>
        /// Remueve cantidad <paramref name="count"/> de elementos a partir de <paramref name="pos"/> del arreglo <paramref name="tArray"/>
        /// </summary>
        /// <param name="arraySize">El tamaño actual del arreglo</param>
        public static void RemoveAt<T>(ref T[] tArray, ref int arraySize, int pos, int count = 0)
        {
            int num = arraySize;
            arraySize -= count;
            int i = pos;
            for (int num2 = arraySize; i < num2; i++)
            {
                tArray[i] = tArray[i + count];
            }
            for (int j = arraySize; j < num; j++)
            {
                tArray[j] = default(T);
            }
        }

        /// <summary>
        /// Remuueve cantidad <paramref name="count"/> de elementos a partir de <paramref name="pos"/> del arreglo <paramref name="tArray"/>. El tamaño del arreglo es modificado para acomodar el nuevo tamaño
        /// </summary>
        public static void RemoveAtAndResize<T>(ref T[] tArray, int pos, int count)
        {
            int arraySize = tArray.Length;
            RemoveAt(ref tArray, ref arraySize, pos, count);
            Array.Resize(ref tArray, arraySize);
        }

        /// <summary>
        /// Consigue el valor en el indice <paramref name="index"/> del arreglo <paramref name="tArray"/>, sin tirar una excepcion.
        /// </summary>
        public static T GetSafe<T>(ref T[] tArray, int index)
        {
            if ((uint)index >= tArray.Length)
                return default(T);

            return tArray[index];
        }


        /// <summary>
        /// Consigue el valor en el indice <paramref name="index"/> del arreglo <paramref name="tArray"/>, sin tirar una excepcion. Si el indice es invalido, se retorna <paramref name="defaultValue"/>
        /// </summary>
        public static T GetSafe<T>(ref T[] tArray, int index, in T defaultValue)
        {
            if ((uint)index >= tArray.Length)
                return defaultValue;

            return tArray[index];
        }

        /// <summary>
        /// Cambia todos los valores en <paramref name="tArray"/> por <paramref name="val"/>
        /// </summary>
        public static void SetAll<T>(ref T[] tArray, in T val)
        {
            for (int i = 0; i < tArray.Length; i++)
            {
                tArray[i] = val;
            }
        }

        /// <summary>
        /// Asigna el valor <paramref name="val"/> a <paramref name="count"/> elementos de <paramref name="tArray"/>, a partir del indice <paramref name="startPos"/>
        /// </summary>
        public static void SetRange<T>(ref T[] tArray, in T val, int startPos, int count)
        {
            if (startPos < 0)
                throw new ArgumentOutOfRangeException("startPos", "startPos cannot be less than zero.");

            int num = startPos + count;
            if (tArray.Length < num)
                throw new ArgumentOutOfRangeException("count", "startPos + count cannot exceed tArray.Length.");

            for (int i = startPos; i < num; i++)
            {
                tArray[i] = val;
            }
        }

        /// <summary>
        /// Se asegura que la capacidad de <paramref name="tArray"/> sea almenos <paramref name="capacity"/>.
        /// </summary>
        public static void EnsureCapacity<T>(ref T[] tArray, int capacity)
        {
            if (tArray.Length < capacity)
            {
                Array.Resize(ref tArray, capacity);
            }
        }

        /// <summary>
        /// Cambia de lugar el valor de indice <paramref name="a"/> al indice b, y el valor del indice <paramref name="b"/> al indice a
        /// </summary>
        public static void Swap<T>(T[] array, int a, int b)
        {
            ref T reference = ref array[a];
            ref T reference2 = ref array[b];
            T val = reference;
            reference = reference2;
            reference2 = val;
        }

        /// <summary>
        /// Elimina todos los valores dentro de <paramref name="array"/>.
        /// </summary>
        /// <param name="count">La cantidad de elementos a eliminar.</param>
        public static void Clear<T>(T[] array, ref int count)
        {
            Array.Clear(array, 0, count);
            count = 0;
        }

        /// <summary>
        /// Revisa si la sequencia de elementos entre <paramref name="a"/> y <paramref name="b"/> son iguales.
        /// </summary>
        public static bool SequenceEquals<T>(T[] a, T[] b) where T : IEquatable<T>
        {
            if (a == null || b == null)
            {
                return a == null == (b == null);
            }
            if (a == b)
            {
                return true;
            }
            if (a.Length != b.Length)
            {
                return false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (!a[i].Equals(b[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Revisa si la sequencia de elementos entre <paramref name="a"/> y <paramref name="b"/> son iguales usando el comparador <paramref name="equalityComparison"/>
        /// </summary>
        public static bool SequenceEquals<T, TComparer>(T[] a, T[] b, TComparer equalityComparison) where TComparer : IEqualityComparer<T>
        {
            if (a == null || b == null)
            {
                return a == null == (b == null);
            }
            if (a == b)
            {
                return true;
            }
            if (a.Length != b.Length)
            {
                return false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (!equalityComparison.Equals(a[i], b[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Crea un clon de <paramref name="src"/>
        /// </summary>
        public static T[] Clone<T>(T[] src)
        {
            T[] array = new T[src.Length];
            Array.Copy(src, array, src.Length);
            return array;
        }

        /// <summary>
        /// Crea un clon de la lista <paramref name="src"/>
        /// </summary>
        public static TElement[] Clone<TElement, TSrc>(TSrc src) where TSrc : IReadOnlyList<TElement>
        {
            TElement[] array = new TElement[src.Count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = src[i];
            }
            return array;
        }

        /// <summary>
        /// Crea un clon de la lista <paramref name="src"/>, y este clon es asignado a <paramref name="dest"/>
        /// </summary>
        public static void Clone<TElement, TSrc>(TSrc src, out TElement[] dest) where TSrc : IReadOnlyList<TElement>
        {
            dest = Clone<TElement, TSrc>(src);
        }

        /// <summary>
        /// Clona todos los elementos de <paramref name="src"/> a <paramref name="dest"/>.
        /// </summary>
        public static void CloneTo<T>(T[] src, ref T[] dest)
        {
            Array.Resize(ref dest, src.Length);
            Array.Copy(src, dest, src.Length);
        }

        /// <summary>
        /// Clona todos los elementos de <paramref name="src"/> a <paramref name="dest"/>, asegurando que <paramref name="dest"/> tenga una capacidad de <paramref name="destLength"/>
        /// </summary>
        public static void CloneTo<T>(T[] src, ref T[] dest, ref int destLength)
        {
            EnsureCapacity(ref dest, src.Length);
            Array.Copy(src, dest, src.Length);
            destLength = src.Length;
        }

        /// <summary>
        /// Clona todos los elementos de <paramref name="src"/> a <paramref name="dest"/>.
        /// </summary>
        public static void CloneTo<TElement, TSrc>(TSrc src, ref TElement[] dest) where TSrc : IReadOnlyList<TElement>
        {
            Array.Resize(ref dest, src.Count);
            for (int i = 0; i < dest.Length; i++)
            {
                dest[i] = src[i];
            }
        }

        /// <summary>
        /// Clona todos los elementos de <paramref name="src"/> a <paramref name="dest"/>, asegurando que <paramref name="dest"/> tenga una capacidad de <paramref name="destLength"/>
        /// </summary>
        public static void CloneTo<TElement, TSrc>(TSrc src, ref TElement[] dest, ref int destLength) where TSrc : IReadOnlyList<TElement>
        {
            EnsureCapacity(ref dest, src.Count);
            destLength = src.Count;
            for (int i = 0; i < destLength; i++)
            {
                dest[i] = src[i];
            }
        }

        /// <summary>
        /// Revisa si el indice <paramref name="index"/> es valido.
        /// </summary>
        public static bool IsInBounds<T>(T[] array, int index)
        {
            return (uint)index < array.Length;
        }

        /// <summary>
        /// Revisa si el indice <paramref name="index"/> es valido.
        /// </summary>
        public static bool IsInBounds<T>(T[] array, uint index)
        {
            return index < array.Length;
        }

        /// <summary>
        /// Combina los arreglos <paramref name="a"/> y <paramref name="b"/>
        /// </summary>
        public static T[] Join<T>(T[] a, T[] b)
        {
            int num = a.Length + b.Length;
            if (num == 0)
            {
                return Array.Empty<T>();
            }
            T[] array = new T[num];
            Array.Copy(a, 0, array, 0, a.Length);
            Array.Copy(b, 0, array, a.Length, b.Length);
            return array;
        }

        /// <summary>
        /// Combina multiples arreglos bidimensionales en un solo arreglo.
        /// </summary>
        public static T[] Join<T>(params T[][] arrays)
        {
            int num = 0;
            T[][] array = arrays;
            foreach (T[] array2 in array)
            {
                num += array2.Length;
            }
            if (num == 0)
            {
                return Array.Empty<T>();
            }
            T[] array3 = new T[num];
            int num2 = 0;
            array = arrays;
            foreach (T[] array4 in array)
            {
                Array.Copy(array4, 0, array3, num2, array4.Length);
                num2 += array4.Length;
            }
            return array3;
        }
    }
}