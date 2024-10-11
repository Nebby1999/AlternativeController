using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Nebula
{
    /// <summary>
    /// Extensiones varias
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Revisa si <paramref name="text"/> es null, esta vacio, o son solo espacios.
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string text)
        {
            return (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text));
        }

        /// <summary>
        /// Remueve todos los espacios de <paramref name="str"/>
        /// </summary>
        public static string WithAllWhitespaceStripped(this string str)
        {
            var buffer = new StringBuilder();
            foreach (var ch in str)
                if (!char.IsWhiteSpace(ch))
                    buffer.Append(ch);
            return buffer.ToString();
        }

        /// <summary>
        /// Deconstruye un <see cref="KeyValuePair{TKey, TValue}"/> para poder ocupar una sintaxis mejor en un foreach loop.
        /// 
        /// <code>
        /// for(var (key, value) in someDictionar)
        /// </code>
        /// </summary>
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue value)
        {
            key = kvp.Key;
            value = kvp.Value;
        }

        /// <summary>
        /// <inheritdoc cref="Xoroshiro128Plus.NextElementUniform{T}(T[])"/>.
        /// </summary>
        public static T NextElementUniform<T>(this T[] array, Xoroshiro128Plus rng)
        {
            return rng.NextElementUniform(array);
        }

        /// <summary>
        /// <inheritdoc cref="Xoroshiro128Plus.NextElementUniform{T}(List{T})"/>
        /// </summary>
        public static T NextElementUniform<T>(this List<T> list, Xoroshiro128Plus rng)
        {
            return rng.NextElementUniform(list);
        }

        /// <summary>
        /// <inheritdoc cref="Xoroshiro128Plus.NextElementUniform{T}(IList{T})"/>
        /// </summary>
        public static T NextElementUniform<T>(this IList<T> list, Xoroshiro128Plus rng)
        {
            return rng.NextElementUniform(list);
        }

        /// <summary>
        /// <inheritdoc cref="Xoroshiro128Plus.RetrieveAndRemoveNextElementUniform{T}(ref T[])"/>
        /// </summary>
        public static T RetrieveAndRemoveNextElementUniform<T>(this T[] array, Xoroshiro128Plus rng)
        {
            return rng.RetrieveAndRemoveNextElementUniform(array);
        }

        /// <summary>
        /// <inheritdoc cref="Xoroshiro128Plus.RetrieveAndRemoveNextElementUniform{T}(List{T})"/>
        /// </summary>
        public static T RetrieveAndRemoveNextElementUniform<T>(this List<T> list, Xoroshiro128Plus rng)
        {
            return rng.RetrieveAndRemoveNextElementUniform(list);
        }


        /// <summary>
        /// <inheritdoc cref="Xoroshiro128Plus.RetrieveAndRemoveNextElementUniform{T}(IList{T})"/>
        /// </summary>
        public static T RetrieveAndRemoveNextElementUniform<T>(this IList<T> list, Xoroshiro128Plus rng)
        {
            return rng.RetrieveAndRemoveNextElementUniform(list);
        }

        /// <summary>
        /// Se Asegura que <paramref name="t"/> no sea null en el mundo de C++. esto permite utilizar los operadores ?. y ?? con tal que este metodo sea llamado primero.
        /// </summary>
        public static T AsValidOrNull<T>(this T t) where T : UnityEngine.Object => t ? t : null;

        /// <summary>
        /// Se asegura que el componente <typeparamref name="T"/> exista.
        /// </summary>
        public static T EnsureComponent<T>(this Component c) where T : Component => EnsureComponent<T>(c.gameObject);
        /// <summary>
        /// Se asegura que el componente <typeparamref name="T"/> exista.
        /// </summary>
        public static T EnsureComponent<T>(this Behaviour b) where T : Component => EnsureComponent<T>(b.gameObject);
        /// <summary>
        /// Se asegura que el componente <typeparamref name="T"/> exista.
        /// </summary>
        public static T EnsureComponent<T>(this MonoBehaviour mb) where T : Component => EnsureComponent<T>(mb.gameObject);
        /// <summary>
        /// Se asegura que el componente <typeparamref name="T"/> exista.
        /// </summary>
        public static T EnsureComponent<T>(this GameObject go) where T : Component => go.GetComponent<T>().AsValidOrNull() ?? go.AddComponent<T>();

        /// <summary>
        /// Compara si el Layer del objeto es igual a <paramref name="layerIndex"/>
        /// </summary>
        public static bool CompareLayer(this Component c, int layerIndex) => CompareLayer(c.gameObject, layerIndex);
        /// <summary>
        /// Compara si el Layer del objeto es igual a <paramref name="layerIndex"/>
        /// </summary>
        public static bool CompareLayer(this Behaviour b, int layerIndex) => CompareLayer(b.gameObject, layerIndex);
        /// <summary>
        /// Compara si el Layer del objeto es igual a <paramref name="layerIndex"/>
        /// </summary>
        public static bool CompareLayer(this MonoBehaviour mb, int layerIndex) => CompareLayer(mb.gameObject, layerIndex);
        /// <summary>
        /// Compara si el Layer del objeto es igual a <paramref name="layerIndex"/>
        /// </summary>
        public static bool CompareLayer(this GameObject go, int layerIndex) => go.layer == layerIndex;

        /// <summary>
        /// Revisa si el layer <paramref name="layerIndex"/> se encuentra dentro de la LayerMask <paramref name="mask"/>
        /// </summary>
        public static bool Contains(this LayerMask mask, int layerIndex)
        {
            return mask == (mask | (1 << layerIndex));
        }

        /// <summary>
        /// Crea el color ideal de borde para <paramref name="color32"/>
        /// </summary>
        public static Color32 GetBestOutline(this Color32 color32)
        {
            Color asColor = (Color)color32;
            return (Color32)asColor.GetBestOutline();
        }


        /// <summary>
        /// Crea el color ideal de borde para <paramref name="color"/>
        /// </summary>
        public static Color GetBestOutline(this Color color)
        {
            Color.RGBToHSV(color, out float hue, out float saturation, out float brightness);

            float modifier = brightness > 0.5f ? (-0.5f) : 0.5f;
            float newSaturation = Mathf.Clamp01(saturation + modifier);
            float newBrightness = Mathf.Clamp01(brightness + modifier);
            return Color.HSVToRGB(hue, newSaturation, newBrightness);
        }
    }
}