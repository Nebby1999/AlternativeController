using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Nebula
{
    /// <summary>
    /// Representa una coleccion con elementos "Pesados". Utilizados para conseguir un elemento con una "chance" de obtener y que esta chance sea realmente aleatoria.
    /// <br></br>
    /// El peso de un item representa el peso a conseguir dicho item en relacion a los otros items de la coleccion, por ejemplo, una lista de 3 elementos donde cada uno tiene un peso de 1 significa que cada elemento tiene un 33% de probabilidad de ser escogido. Si uno de estos elementos tiene mayor peso, ese elemento saldra con mayor frequencia.
    /// </summary>
    /// <typeparam name="T">El tipo de valor dentro de la coleccion</typeparam>
    public class WeightedCollection<T> : IEnumerable<WeightedCollection<T>.WeightedValue>, ICollection<WeightedCollection<T>.WeightedValue>, IList<WeightedCollection<T>.WeightedValue>
        where T : class
    {
        /// <summary>
        /// Indexador para conseguir un <see cref="WeightedValue"/> de manera directa.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public WeightedValue this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                _items[index] = value;
            }
        }

        /// <summary>
        /// La cantidad de elementos dentro de la coleccion
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// Si la coleccion es de solo lectura
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// El peso total de todos los elementos dentro de la coleccion
        /// </summary>
        public float totalWeight { get; private set; }

        private List<WeightedValue> _items = new List<WeightedValue>();
        internal Xoroshiro128Plus _rng = new Xoroshiro128Plus((ulong)DateTime.Now.Ticks);
        internal ulong? _seed;

        /// <summary>
        /// Consigue un item pesado de la coleccion
        /// </summary>
        /// <returns>El item de la coleccion</returns>
        public T Next()
        {
            int index = NextIndex();
            if (index == -1)
                return default;
            return _items[index].value;
        }

        /// <summary>
        /// Consigue el siguiente indice que deberia ser usado para conseguir el siguiente elemento
        /// </summary>
        /// <returns>El indice del siguiente elemento, -1 si la coleccion esta vacia</returns>
        public int NextIndex()
        {
            if (Count == 0)
                return -1;

            float startWeight = _rng.RangeFloat(0, totalWeight);
            for (int i = 0; i < _items.Count; i++)
            {
                startWeight -= _items[i].weight;
                if (startWeight <= 0)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Añade un nuevo elemento con el peso especificado en <paramref name="weight"/>
        /// </summary>
        /// <param name="value">El nuevo elemento a agregar</param>
        /// <param name="weight">El peso de dicho elemento</param>
        public void Add(T value, float weight)
        {
            AddInternal(new WeightedValue(value, weight));
            RecalculateTotalWeight();
        }

        /// <summary>
        /// Agrega un nuevo <see cref="WeightedValue"/> a la coleccion
        /// </summary>
        /// <param name="item">El nuevo elemento para la coleccion</param>
        public void Add(WeightedValue item)
        {
            AddInternal(item);
            RecalculateTotalWeight();
        }

        /// <summary>
        /// Agrega una coleccion de elementos pesados
        /// </summary>
        /// <param name="collection">La coleccion de elementos</param>
        public void Add(IEnumerable<WeightedValue> collection)
        {
            foreach (var value in collection)
            {
                AddInternal(value);
            }
            RecalculateTotalWeight();
        }

        /// <summary>
        /// Cambia la semilla usada por el sistema de numero aleatorio dentro de esta coleccion
        /// <br>Mira tambien <see cref="Xoroshiro128Plus.ResetSeed(ulong)"/></br>
        /// </summary>
        /// <param name="seed">La semilla a usar</param>
        public void SetSeed(ulong seed)
        {
            _seed = seed;
            _rng.ResetSeed(seed);
        }

        private void AddInternal(WeightedValue item)
        {
            _items.Add(item);
        }

        private void RecalculateTotalWeight()
        {
            totalWeight = 0;
            for (int i = 0; i < Count; i++)
            {
                totalWeight += this[i].weight;
            }
        }

        /// <summary>
        /// Quita todos los elementos de la coleccion pesada
        /// </summary>
        public void Clear()
        {
            _items.Clear();
            RecalculateTotalWeight();
        }

        /// <summary>
        /// Revisa si un valor en especifico esta en la coleccion.
        /// </summary>
        /// <param name="item">El valor a verificar</param>
        /// <returns>Verdadero si <paramref name="item"/> esta dentro de la coleccion</returns>
        public bool Contains(WeightedValue item)
        {
            return _items.Contains(item);
        }

        /// <summary>
        /// Copia todos los valores dentro de esta coleccion a <paramref name="array"/>, a partir del indice especificado en <paramref name="arrayIndex"/>
        /// </summary>
        /// <param name="array">El arreglo de destino</param>
        /// <param name="arrayIndex">El indice a comenzar</param>
        public void CopyTo(WeightedValue[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Consigue un enumerador para enumerar todos los elementos de la coleccion
        /// </summary>
        /// <returns>El enumerador</returns>
        public IEnumerator<WeightedValue> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Consigue el indice del <paramref name="item"/>
        /// </summary>
        /// <param name="item">El item a conseguir su indice</param>
        /// <returns>El indice del item</returns>
        public int IndexOf(WeightedValue item)
        {
            return _items.IndexOf(item);
        }

        /// <summary>
        /// Inserta el elemento <paramref name="value"/> en el indice <paramref name="index"/>
        /// </summary>
        /// <param name="index">El indice del nuevo item</param>
        /// <param name="value">El nuevo item</param>
        /// <param name="weight">El peso del nuevo item</param>
        public void Insert(int index, T value, float weight)
        {
            Insert(index, new WeightedValue(value, weight));
        }

        /// <summary>
        /// Inserta el elemento <paramref name="item"/> en el indice <paramref name="index"/>
        /// </summary>
        /// <param name="index">El indice del nuevo item</param>
        /// <param name="item">El nuevo item</param>
        public void Insert(int index, WeightedValue item)
        {
            _items.Insert(index, item);
            RecalculateTotalWeight();
        }

        /// <summary>
        /// Inserta todos los items en <paramref name="collection"/> a partir del indice <paramref name="index"/>
        /// </summary>
        /// <param name="index">El indice donde los nuevos elementos empezaran</param>
        /// <param name="collection">La coleccion de elementos</param>
        public void InsertRange(int index, IEnumerable<(T, float)> collection)
        {
            InsertRange(index, collection.Select(t => new WeightedValue(t.Item1, t.Item2)));
        }

        /// <summary>
        /// Inserta todos los items en <paramref name="collection"/> a partir del indice <paramref name="index"/>
        /// </summary>
        /// <param name="index">El indice donde los nuevos elementos empezaran</param>
        /// <param name="collection">La coleccion de elementos</param>
        public void InsertRange(int index, IEnumerable<WeightedValue> collection)
        {
            _items.InsertRange(index, collection);
            RecalculateTotalWeight();
        }

        /// <summary>
        /// Quita el elemento <paramref name="item"/> de la coleccion
        /// </summary>
        /// <param name="item">El elemento a quitar</param>
        /// <returns>Verdadero si el item efectivamente fue removido</returns>
        public bool Remove(WeightedValue item)
        {
            return _items.Remove(item);
        }

        /// <summary>
        /// Remueve el item en el indice <paramref name="index"/> de la coleccion
        /// </summary>
        /// <param name="index">El indice a remover</param>
        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
            RecalculateTotalWeight();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Genera un valor legible para esta coleccion
        /// </summary>
        /// <returns>El valor legible</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("WeightedList<");
            stringBuilder.Append(typeof(T).Name);
            stringBuilder.Append(">: TotalWeight:");
            stringBuilder.Append(totalWeight);
            stringBuilder.Append(", Count:");
            stringBuilder.Append(Count);
            stringBuilder.Append(", {\n");
            for (int i = 0; i < Count; i++)
            {
                string s = _items[i].ToString();
                s += i < Count - 1 ? "," : "";
                stringBuilder.AppendLine(s);
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Crea una nueva coleccion pesada
        /// </summary>
        public WeightedCollection()
        {
            _seed = (ulong)DateTime.Now.Ticks;
            _rng = new Xoroshiro128Plus(_seed.Value);
        }

        /// <summary>
        /// Crea una nueva coleccion pesada con una cantidad de elementos
        /// </summary>
        /// <param name="collection">Los elementos para la coleccion</param>
        public WeightedCollection(IEnumerable<WeightedValue> collection) : this()
        {
            Add(collection);
        }

        /// <summary>
        /// Crea una copia de la coleccion <paramref name="orig"/>
        /// </summary>
        /// <param name="orig">La coleccion a copiar</param>
        public WeightedCollection(WeightedCollection<T> orig)
        {
            _seed = orig._seed;
            _rng = orig._rng;
            Add(orig._items);
        }

        /// <summary>
        /// Representacion de un elemento pesado dentro de un <see cref="WeightedCollection{T}"/> 
        /// </summary>
        public struct WeightedValue : IEquatable<WeightedValue>
        {
            /// <summary>
            /// El valor en si
            /// </summary>
            public T value;

            /// <summary>
            /// El peso de el valor
            /// </summary>
            public float weight;

            /// <summary>
            /// Retorna verdadero si <paramref name="obj"/> es de tipo <see cref="WeightedValue"/> y ambos <see cref="value"/> son iguales.
            /// </summary>
            /// <param name="obj">El objeto a comparar</param>
            /// <returns>Verdadero si ambos objetos son <see cref="WeightedValue"/> y ambos <see cref="value"/> son iguales</returns>
            public override bool Equals(object obj)
            {
                return obj is WeightedValue other && Equals(other);
            }

            /// <summary>
            /// Retorna verdadero si tanto este elemento como <paramref name="other"/> tienen <see cref="value"/> iguales
            /// </summary>
            /// <param name="other">El otro weighted value</param>
            /// <returns>La igualdad entre ambos</returns>
            public bool Equals(WeightedValue other)
            {
                return value == other.value;
            }

            /// <summary>
            /// Operador de igualdad para <see cref="WeightedValue"/>
            /// </summary>
            /// <returns>True si ambos apuntan al mismo value</returns>
            public static bool operator ==(WeightedValue lhs, WeightedValue rhs)
            {
                return lhs.Equals(rhs);
            }


            /// <summary>
            /// Operador de desigualdad para <see cref="WeightedValue"/>
            /// </summary>
            /// <returns>True si ambos NO apuntan al mismo value</returns>
            public static bool operator !=(WeightedValue lhs, WeightedValue other)
            {
                return !(lhs == other);
            }

            /// <summary>
            /// Consigue un hashcode de este elemento usando <see cref="value"/>
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return value.GetHashCode();
            }

            /// <summary>
            /// Consigue una representacion legible de este valor
            /// </summary>
            public override readonly string ToString()
            {
                return string.Format("{0} : {1}", weight, value);
            }

            /// <summary>
            /// Constructor para un nuevo WeightedValue
            /// </summary>
            /// <param name="value">El valor en si</param>
            /// <param name="weight">El peso del valor</param>
            public WeightedValue(T value, float weight)
            {
                this.value = value;
                this.weight = weight;
            }
        }
    }
}