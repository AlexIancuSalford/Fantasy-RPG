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
            Transform playerTransform = GetPlayerPosition();
            byte[] playerBytes = CSerializer.SerializeVector(playerTransform.position);
            CSerializer.WriteToFile(fileName, playerBytes);
        }

        public void Load(string fileName)
        {
            CSerializer.ReadFromFile(fileName);
            GameObject.FindWithTag("Player").transform.position = CSerializer.DeserializedVector;
        }

        private Transform GetPlayerPosition()
        {
            return GameObject.FindWithTag("Player").transform;
        }
    }
}
