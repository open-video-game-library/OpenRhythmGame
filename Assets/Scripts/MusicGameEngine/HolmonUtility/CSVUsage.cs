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
            // ファイルが存在するか確認
            bool fileExists = File.Exists(path);

            // ファイルストリームを使って書き込む/追記する
            using (StreamWriter writer = new StreamWriter(path, append: fileExists))
            {
                // データを書き込む
                writer.WriteLine(string.Join(",", lineDatas));
            }

            Debug.Log("CSVファイルに書き込みが完了しました。パス: " + path);
        }

        public static void WriteCSV(string path, params string[][] datas)
        {

            // ファイルが存在するか確認
            bool fileExists = File.Exists(path);

            // ファイルストリームを使って書き込む/追記する
            using (StreamWriter writer = new StreamWriter(path, append: fileExists))
            {
                foreach(var line in datas)
                {
                    // データを書き込む
                    writer.WriteLine(string.Join(",", line));
                }
            }

            Debug.Log("CSVファイルに書き込みが完了しました。パス: " + path);
        }
    }
}
