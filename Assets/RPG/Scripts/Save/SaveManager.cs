using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RPG.Helper;
using UnityEngine;

namespace RPG.Save
{
    public class SaveManager : MonoBehaviour
    {
        public void Save(string fileName)
        {
            CSerializer.WriteToFile(fileName, new Vector3f(GetPlayerPosition().transform.position));

            Vector3 vector3 = new Vector3(10, 11, 12);
        }

        public void Load(string fileName)
        {
            Vector3 result = (Vector3f)CSerializer.ReadFromFile(fileName);
            Debug.Log("Loaded from file: " + result);
            GameObject.FindWithTag("Player").transform.position = result;
        }

        private Transform GetPlayerPosition()
        {
            return GameObject.FindWithTag("Player").transform;
        }
    }
}
