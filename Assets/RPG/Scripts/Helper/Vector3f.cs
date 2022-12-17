using System;
using UnityEngine;

namespace RPG.Helper
{
    [Serializable]
    public class Vector3f
    {
        public float x, y, z;

        public Vector3f(float x, float y, float z)
        {
            this.x = x; 
            this.y = y;
            this.z = z;
        }

        public Vector3f(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public static implicit operator Vector3f(Vector3 vector3)
        {
            return new Vector3f(vector3);
        }

        public static implicit operator Vector3(Vector3f vector3f)
        {
            return new Vector3(vector3f.x, vector3f.y, vector3f.z);
        }

        public Vector3f ToVector3() => new Vector3(x, y, z);
    }
}
