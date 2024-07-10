using System;
using UnityEngine;

namespace EntityStates
{
    public class StateConfigurationTest : State
    {
        public static GameObject objectTestStatic;
        public static int testInt;
        public static uint testUInt;
        public static long testLong;
        public static ulong testULong;
        public static bool testBool;
        public static float testFloat;
        public static double testDouble;
        public static string testString;
        public static Color testColor;
        public static LayerMask testLayerMask;
        public static Vector2 testVector2;
        public static Vector2Int testVector2Int;
        public static Vector3 testVector3;
        public static Vector3Int testVector3Int;
        public static Vector4 testVector4;
        public static Rect testRect;
        public static RectInt testRectInt;
        public static char testChar;
        public static Bounds testBounds;
        public static BoundsInt testBoundsInt;
        public static Quaternion testQuaternion;
        public static AnimationCurve animationCurve;
        public static Gradient gradient;

        [SerializeField]
        public ScriptableObject objectTestInstance;

        [SerializeField]
        public TestEnum testEnum;

        [SerializeField]
        public TestFlags testFlags;

        protected override void Initialize()
        {
        }

        public enum TestEnum
        {
            ValueA,
            ValueB,
            ValueC
        }

        [Flags]
        public enum TestFlags
        {
            FlagA = 1,
            FlagB = 2,
            FlagC = 4
        }
    }
}