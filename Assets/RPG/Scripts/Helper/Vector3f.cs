/*
 * This script is a C# class in Unity that defines a custom Vector3f type
 * that is used to store 3D vector data. The class is marked with the
 * [Serializable] attribute, which allows it to be serialized and deserialized
 * as part of a larger object.
 *  
 * The class has three public float fields: x, y, and z, which represent the
 * three dimensions of the vector. It has a constructor that takes three
 * float arguments and initializes the fields with the values of the arguments,
 * and another constructor that takes a Vector3 object and initializes the
 * fields with the values of the Vector3 object's x, y, and z components.
 *  
 * The class also has two implicit operator methods that allow it to be cast
 * to and from the built-in Vector3 type. The first method, an implicit
 * operator that converts a Vector3f object to a Vector3 object, creates a new
 * Vector3 object with the x, y, and z components of the Vector3f object, and
 * returns it. The second method, an implicit operator that converts a Vector3
 * object to a Vector3f object, creates a new Vector3f object with the x, y,
 * and z components of the Vector3 object, and returns it.
 *  
 * Finally, the class has a ToVector3() method that returns a new Vector3
 * object with the x, y, and z components of the Vector3f object. This method
 * allows the Vector3f object to be easily converted back to a Vector3 object
 * when needed.
 */

using System;
using UnityEngine;

namespace RPG.Helper
{
    [Serializable]
    public class Vector3f
    {
        public float x, y, z;

        /// <summary>
        /// Constructs a new Vector3f object with the specified x, y, and z components.
        /// </summary>
        /// <param name="x">The x component of the vector.</param>
        /// <param name="y">The y component of the vector.</param>
        /// <param name="z">The z component of the vector.</param>
        public Vector3f(float x, float y, float z)
        {
            this.x = x; 
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Constructs a new Vector3f object with the x, y, and z components of the specified Vector3 object.
        /// </summary>
        /// <param name="vector">The Vector3 object to get the x, y, and z components from.</param>
        public Vector3f(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        /// <summary>
        /// Implicitly converts a Vector3f object to a Vector3 object.
        /// </summary>
        /// <param name="vector3f">The Vector3f object to convert.</param>
        /// <returns>A new Vector3 object with the x, y, and z components of the Vector3f object.</returns>
        public static implicit operator Vector3f(Vector3 vector3)
        {
            return new Vector3f(vector3);
        }

        /// <summary>
        /// Implicitly converts a Vector3 object to a Vector3f object.
        /// </summary>
        /// <param name="vector3">The Vector3 object to convert.</param>
        /// <returns>A new Vector3f object with the x, y, and z components of the Vector3 object.</returns>
        public static implicit operator Vector3(Vector3f vector3f)
        {
            return new Vector3(vector3f.x, vector3f.y, vector3f.z);
        }

        /// <summary>
        /// Returns a new Vector3 object with the x, y, and z components of the Vector3f object.
        /// </summary>
        /// <returns>A new Vector3 object with the x, y, and z components of the Vector3f object.</returns>
        public Vector3f ToVector3() => new Vector3(x, y, z);
    }
}
