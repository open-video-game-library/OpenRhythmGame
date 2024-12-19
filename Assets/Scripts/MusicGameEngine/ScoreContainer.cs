namespace MusicGameEngine
{
    using System.Threading.Tasks;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewScoreContainer", menuName = "ScriptableObjects/ScoreContainer")]
    public class ScoreContainer : ScriptableObject
    {
        [Header("Metadata")]
        [SerializeField] private int _scoreID;
        [SerializeField] private string _songName;
        [SerializeField] private string _artistName;
        [SerializeField] private int _songID;
        [SerializeField] private int _difference;
        [SerializeField] private List<ScriptableObject> _additionalScriptableObjects;

        [Header("Assets")]
        [SerializeField] private AudioClip _song;
        [SerializeField] private TextAsset _score;
        [SerializeField] private Sprite Shumbnail;
        [SerializeField] private List<AudioClip> _additionalClipData;

        //各パラメーターをGetプロパティで公開
        public int ScoreID { get { return _scoreID; } }
        public string SongName { get { return _songName; } }
        public string ArtistName { get { return _artistName; } }
        public int SongID { get { return _songID; } }
        public int Difference { get { return _difference; } }
        public List<ScriptableObject> AdditionalScriptableObjects { get { return _additionalScriptableObjects; } }
        public AudioClip Song { get { return _song; } }
        //public TextAsset Score { get { return _score; } }
        public Sprite Thumbnail { get { return Shumbnail; } }
        public List<AudioClip> AdditionalClipData { get { return _additionalClipData; } }
        public float FirstBPM { get { return GetScore().Speeds[0].BPM; } }
        public float MusicOffset { get { return GetScore().Offset; } }

        public void GenerateRandomScoreID()
        {
            _scoreID = Random.Range(100000, 1000000);
        }
        public void GenerateRandomSongID()
        {
            _songID = Random.Range(100000, 1000000);
        }

        ScoreData cash = null;
        public ScoreData GetScore()
        {
            cash = JsonUtility.FromJson<ScoreData>(_score.ToString());
            return cash;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ScoreContainer))]
    public class ScoreContainerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var script = (ScoreContainer)target;

            if (GUILayout.Button("GenerateRandomScoreID"))
            {
                script.GenerateRandomScoreID();
            }
            if (GUILayout.Button("GenerateRandomSongID"))
            {
                script.GenerateRandomSongID();
            }

        }
    }
#endif
}


