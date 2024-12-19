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
            // ディレクトリが存在しない場合は作成
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // ファイルが存在するか確認
            bool fileExists = File.Exists(path);

            //datasをJsonに変換
            string json = JsonUtility.ToJson(datas);

            // ファイルストリームを使って書き込む
            using (StreamWriter writer = new StreamWriter(path, append: fileExists))
            {
                writer.WriteLine(json);
            }

            Debug.Log("JSONファイルの書き出しが完了しました。パス: " + path);
        }
    }
}
