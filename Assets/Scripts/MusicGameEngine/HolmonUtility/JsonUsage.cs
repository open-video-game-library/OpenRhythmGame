using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HolmonUtility
{
    public static class JsonUsage<T>
    {
        public static void WriteJson(string path, T datas)
        {
            // �f�B���N�g�������݂��Ȃ��ꍇ�͍쐬
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // �t�@�C�������݂��邩�m�F
            bool fileExists = File.Exists(path);

            //datas��Json�ɕϊ�
            string json = JsonUtility.ToJson(datas);

            // �t�@�C���X�g���[�����g���ď�������
            using (StreamWriter writer = new StreamWriter(path, append: fileExists))
            {
                writer.WriteLine(json);
            }

            Debug.Log("JSON�t�@�C���̏����o�����������܂����B�p�X: " + path);
        }
    }
}
