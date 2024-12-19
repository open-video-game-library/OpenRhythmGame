using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HolmonUtility
{
    public static class CSVUsage
    {
        public static void WriteLineCSV(string path, params string[] lineDatas)
        {
            // �t�@�C�������݂��邩�m�F
            bool fileExists = File.Exists(path);

            // �t�@�C���X�g���[�����g���ď�������/�ǋL����
            using (StreamWriter writer = new StreamWriter(path, append: fileExists))
            {
                // �f�[�^����������
                writer.WriteLine(string.Join(",", lineDatas));
            }

            Debug.Log("CSV�t�@�C���ɏ������݂��������܂����B�p�X: " + path);
        }

        public static void WriteCSV(string path, params string[][] datas)
        {

            // �t�@�C�������݂��邩�m�F
            bool fileExists = File.Exists(path);

            // �t�@�C���X�g���[�����g���ď�������/�ǋL����
            using (StreamWriter writer = new StreamWriter(path, append: fileExists))
            {
                foreach(var line in datas)
                {
                    // �f�[�^����������
                    writer.WriteLine(string.Join(",", line));
                }
            }

            Debug.Log("CSV�t�@�C���ɏ������݂��������܂����B�p�X: " + path);
        }
    }
}
