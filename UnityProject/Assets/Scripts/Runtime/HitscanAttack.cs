using AC;
using Nebula;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EntityStates
{
    /// <summary>
    /// Clase que representa un ataque estilo Hitscan. Ocupa un <see cref="Physics2D.Raycast(Vector2, Vector2)"/> para determinar si se colisiono con una <see cref="HurtBox"/>, y de ser asi, daña el <see cref="HealthComponent"/> asociado al <see cref="HurtBox"/>
    /// </summary>
    public class HitscanAttack
    {
        /// <summary>
        /// El atacante que esta disparando el Hitscan
        /// </summary>
        public GameObject attacker;

        /// <summary>
        /// La cantidad de Hitscans a crear.
        /// </summary>
        public int hitscanCount;

        /// <summary>
        /// El punto de origen de los hitscans
        /// </summary>
        public Vector2 origin;

        /// <summary>
        /// La direccion de los hitscans
        /// </summary>
        public Vector2 direction;

        /// <summary>
        /// El radio de los histcans, si esto es un valor que no es 0, se utiliza un <see cref="Physics2D.CircleCast(Vector2, float, Vector2)"/>
        /// </summary>
        public float hitscanRadius;

        /// <summary>
        /// LA distancia que tiene el hitscan
        /// </summary>
        public float distance;

        /// <summary>
        /// Que layers cuentan como layers que podemos detectar y atacar.
        /// </summary>
        public LayerMask hitMask = defaultHitMask;

        /// <summary>
        /// Si uno de los objetos detectados esta en esta layer, no vamos a seguir procesando el ataque.
        /// </summary>
        public LayerMask stopperMask = defaultHitMask;

        /// <summary>
        /// Una variacion de angulo del hitscan, esto es la cantidad minima de variacion
        /// </summary>
        public float minAngleSpread;

        /// <summary>
        /// Una variacion de angulo del hitscan, esto es la cantidad maxima de variacion.
        /// </summary>
        public float maxAngleSpread;

        /// <summary>
        /// Cuanto daño hace cada hitscan
        /// </summary>
        public float damage;

        /// <summary>
        /// Una instancia de RNG para crear las variaciones de angulos y ejecutar el Hitscan
        /// </summary>
        public Xoroshiro128Plus rng;

        /// <summary>
        /// Metodo que se utiliza para calcular un "Falloff" al ataque.
        /// <br>Un "Falloff" es un modificador de daño aplicado al hitscan, para hacer que objetos mas lejos reciban menos daño.</br>
        /// </summary>
        public FalloffCalculateDelegate falloffCalculation = DefaultFalloffCalculation;

        /// <summary>
        /// Metodo que se utiliza para procesar un Hit del ataque.
        /// </summary>
        public HitCallback hitCallback = DefaultHitCallback;

        private RaycastHit2D[] _cachedHits;

        /// <summary>
        /// Dispara este Hitscan attack con la configuracion apropiada.
        /// </summary>
        public void Fire()
        {
            rng ??= new Xoroshiro128Plus(ACApplication.instance.applicationRNG.nextULong); //hay que asegurare un RNG.
            Vector2[] spreadArray = new Vector2[hitscanCount];
            for(int i = 0; i < hitscanCount; i++) //Crear un arreglo de Spreads que son utilizados para dar variacion al Histcan.
            {
                float angle = rng.RangeFloat(minAngleSpread, maxAngleSpread);
                Quaternion spreadRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                spreadArray[i] = spreadRotation * direction;
            };

            for(int i = 0; i < hitscanCount; i++) //Disparar un hitscan
            {
                FireSingle(spreadArray[i]);
            }
        }

        private void FireSingle(Vector2 normal)
        {
            Vector2 endPos = origin + normal * distance; //Precalcular la posicion final del hitscan.
            List<Hit> bulletHit = new List<Hit>();
            if(hitscanRadius == 0)
            {
                _cachedHits = Physics2D.RaycastAll(origin, normal, distance, hitMask);
            }
            else //Usar circlecast si es necesario.
            {
                _cachedHits = Physics2D.CircleCastAll(origin, hitscanRadius, direction, distance, hitMask);
            }

            for(int i = 0; i < _cachedHits.Length; i++)
            {
                Hit hit = default;
                InitBulletHitFromRaycastHit(ref hit, origin, normal, ref _cachedHits[i]); //Creamos un hit a partir del raycast hit, y luego lo procesamos con el callback.
            
                if(hitCallback(this, ref hit)) //Chocamos contra un objeto que detiene el procesamiento del ataque.
                {
                    endPos = hit.hitPoint;
                    break;
                }
            }

#if DEBUG && UNITY_EDITOR
            GlobalGizmos.EnqueueGizmoDrawing(() =>
            {
                UnityEditor.Handles.color = Color.red;
                UnityEditor.Handles.DrawLine(origin, endPos, 5);
            });
#endif
        }

        private void InitBulletHitFromRaycastHit(ref Hit hit, Vector2 origin, Vector2 normal, ref RaycastHit2D raycastHit2D)
        {
            hit.hitColldier = raycastHit2D.collider;
            hit.hurtBox = raycastHit2D.collider.GetComponent<HurtBox>();
            hit.direction = normal;
            hit.distance = raycastHit2D.distance;
            hit.surfaceNormal = raycastHit2D.normal;
            hit.hitPoint = raycastHit2D.distance == 0 ? origin : raycastHit2D.point;
            hit.entityObject = (hit.hurtBox && hit.hurtBox.healthComponent) ? hit.hurtBox.healthComponent.gameObject : raycastHit2D.collider.gameObject; //el entity object es como el "Objeto Entidad" de algo, es mas que nada el objeto principal.
        }

        /// <summary>
        /// Representa una calculacion de falloff por defecto.
        /// </summary>
        /// <param name="distance">La distancia entre el atacante y la victima</param>
        /// <returns>1, la distancia no afecta el daño.</returns>
        public static float DefaultFalloffCalculation(float distance)
        {
            return 1;
        }

        /// <summary>
        /// Representa una calculacion de falloff estilo "Escopeta". Es mas potente que <see cref="BuckshotFalloffCalculation(float)"/>
        /// </summary>
        /// <param name="distance">La distancia entre el atacante y la victima</param>
        /// <returns>La cantidad de daño es reducida considerablemente dependiendo de la distancia</returns>
        public static float BuckshotFalloffCalculation(float distance)
        {
            return 0.25f + Mathf.Clamp01(Mathf.InverseLerp(25f, 7f, distance)) * 0.75f;
        }

        /// <summary>
        /// Representa una calculacion de falloff estilo "Bala". Es menos potente que <see cref="BuckshotFalloffCalculation(float)"/>
        /// </summary>
        /// <param name="distance">La distancia entre el atacante y la victima</param>
        /// <returns> La cantidad de daño es disminuida hasta un 50% dependiendo de la distancia.</returns>
        public static float BulletFalloffCalculation(float distance)
        {
            return 0.5f + Mathf.Clamp01(Mathf.InverseLerp(60f, 25f, distance)) * 0.5f;
        }

        /// <summary>
        /// Representa un callback generico de Hit de un <see cref="HitscanAttack"/>
        /// </summary>
        /// <param name="attack">El ataque que causo el <paramref name="hit"/></param>
        /// <param name="hit">La colision en si</param>
        /// <returns>Falso si deberiamos seguir procesando el ataque, Verdadero si deberiamos parar de procesar el ataque.</returns>
        public static bool DefaultHitCallback(HitscanAttack attack, ref Hit hit)
        {
            if (hit.entityObject && hit.entityObject == attack.attacker)
                return false;

            if (hit.entityObject.CompareTag(attack.attacker.tag)) //Me gustaria un sistema de equipos propiamente tal, pero los objetos con el mismo tag no se pueden atacar entre si.
                return false;

            if (!hit.hurtBox || !hit.hurtBox.healthComponent)
                return false;

            var healthComponent = hit.hurtBox.healthComponent;
            var falloffFactor = attack.falloffCalculation(hit.distance);

            DamageInfo damageInfo = new DamageInfo
            {
                attackerObject = attack.attacker,
                attackerBody = attack.attacker.GetComponent<CharacterBody>(),
                damage = attack.damage * falloffFactor
            };
            healthComponent.TakeDamage(damageInfo);
            return true;
        }

        /// <summary>
        /// Un Delegado usado para calcular el Falloff de daño, el cual es usado para aumentar o disminuir el daño dependiendo de la distancia.
        /// </summary>
        /// <param name="distance">La distancia entre el atacante y la victima</param>
        /// <returns>Un Coeficiente aplicado al daño final</returns>
        public delegate float FalloffCalculateDelegate(float distance);

        /// <summary>
        /// Delegado que maneja el behaviour de choque de un Hitscan attack
        /// </summary>
        /// <param name="attack">El hitscan attack que llamo el delegado</param>
        /// <param name="hit">El <see cref="Hit"/> que se esta procesando</param>
        /// <returns>Verdadero si el procesamiento de Hits deberia parar.</returns>
        public delegate bool HitCallback(HitscanAttack attack, ref Hit hit);

        /// <summary>
        /// Representa una mascara default para choques, incluye <see cref="LayerIndex.mapElements"/> y <see cref="LayerIndex.entityPrecise"/>
        /// </summary>
        private static LayerMask defaultHitMask = LayerIndex.mapElements.mask | LayerIndex.entityPrecise.mask;

        /// <summary>
        /// Representa una colision del Hitscan
        /// </summary>
        public struct Hit
        {
            /// <summary>
            /// La direccion del hitscan entre el dueño y la victima
            /// </summary>
            public Vector2 direction;
            /// <summary>
            /// El punto exacto de colision del hitscan
            /// </summary>
            public Vector2 hitPoint;
            /// <summary>
            /// La direccion normal de la colision
            /// </summary>
            public Vector2 surfaceNormal;
            /// <summary>
            /// La distancia entre el atacante y la victima
            /// </summary>
            public float distance;
            /// <summary>
            /// El collider que chocamos contra
            /// </summary>
            public Collider2D hitColldier;
            /// <summary>
            /// El objeto principal que chocamos.
            /// </summary>
            public GameObject entityObject;
            /// <summary>
            /// El Hurtbox con que chocamos, este valor puede ser null.
            /// </summary>
            public HurtBox hurtBox;
        }
    }
}