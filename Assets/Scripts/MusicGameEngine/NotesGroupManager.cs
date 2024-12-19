namespace MusicGameEngine
{
    using HolmonUtility;
    using System;
    using UnityEngine;

    public class NotesGroupManager : MonoBehaviour
    {
        [Header("処理ロジック")]
        [SerializeField] protected GameObject _ScoreTimingNotifyLogic;
        [SerializeField] protected GameObject _BPMChangeNotifyLogic;
        [SerializeField] protected GameObject _inputLogic;
        [Header("ノーツプール")]
        [SerializeField] protected ObjectPool _singleNoteObjectPool;
        [SerializeField] protected ObjectPool _longNoteObjectPool;
        [Header("パラメーター")]
        [SerializeField] private float _hsTweenTime = 0.5f;
        [SerializeField] private float _noteAnimationTime = 6f;

        ExtendNoteReactionBase[] _extendNoteReactions = default;
        TimingNotifyBase _scoreTimingNotify = null;
        TimingNotifyBase _bpmChangeNotify = null;
        InputBase _inputBase = null;
        TaskTweener _hsTweener = null;

        public float HS { get; private set; } = 1.0f;
        public float BPMRatio { get; private set; } = 1.0f;
        public float NextNoteBeatNum { get
            {
                if (_notes.Count == 0) return float.PositiveInfinity;
                else return _notes[0]._noteData.Beat[0] + (_notes[0]._noteData.Beat[1] / _notes[0]._noteData.Beat[2]);
            }
        }

        protected double _judgeOffset = 0.0;
        protected double _animationOffset = 0.0f;

        private string _scoreTimingNotifyKey = "";
        private string _bpmChangeNotifyKey = "";

        //private int _generatedNotesCount { get { return _notesParent.childCount; } }
        //private Queue<GameObject> _availableNoteObjects = new Queue<GameObject>();

        protected IndexableQueue<NoteData> _notes = new IndexableQueue<NoteData>();
        protected IndexableQueue<NoteData> _throwed = new IndexableQueue<NoteData>();
        protected IndexableQueue<NoteData> _existNotes 
        { get 
            {
                IndexableQueue<NoteData> notes = new IndexableQueue<NoteData>();
                foreach (var note in _notes) notes.Enqueue(note);
                foreach (var note in _throwed) notes.Enqueue(note);
                return notes;
            } 
        }

        protected virtual void Start()
        {
            try
            {
                _inputBase = _inputLogic.GetComponent<InputBase>();
            }
            catch
            {
                Debug.LogError(_inputLogic.name + "にInputBaseがアタッチされていません");
            }
            try
            {
                _scoreTimingNotify = _ScoreTimingNotifyLogic.GetComponent<TimingNotifyBase>();
            }
            catch
            {
                Debug.LogError(_ScoreTimingNotifyLogic.name + "にTimingNotifyBaseがアタッチされていません");
            }
            try
            {
                _bpmChangeNotify = _BPMChangeNotifyLogic.GetComponent<TimingNotifyBase>();
            }
            catch
            {
                Debug.LogError(_BPMChangeNotifyLogic.name + "にTimingNotifyBaseがアタッチされていません");
            }

            _inputBase.AssignInputOnCallback(() =>
            {
                if (_notes.Count == 0) return;

                var note = _notes[0];

                /*
                for (int i = 1; i < _notes.Count; i++)
                {
                    if (!note.endJudgeProgress) break;
                    note = _notes[i];
                }

                if (note.endJudgeProgress)
                {
                    throw new Exception();
                }
                */

                foreach (var re in _extendNoteReactions) re.ExtendlInputOnProgress(note._noteData);

                note.Judge(eInputType.On);

                
            });

            _inputBase.AssignInputOffCallback(() =>
            {

                if (_notes.Count == 0) return;

                var note = _notes[0];
                for (int i = 1; i < _notes.Count; i++)
                {
                    if (!note.endJudgeProgress) break;
                    note = _notes[i];
                }
                if (note.endJudgeProgress)
                {
                    Debug.LogError("Fuck");
                }

                foreach (var re in _extendNoteReactions) re.ExtendlInputOffProgress(note._noteData);

                note.Judge(eInputType.Off);

                try
                {
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            });
        }

        public virtual void Play(ScoreData score)
        {
            _scoreTimingNotify.RemoveNotifyCallBack(_scoreTimingNotifyKey);
            _scoreTimingNotifyKey = _scoreTimingNotify.AssignNotifyCallBack((note, time) =>
            {
                GenerateNote(note, time);
            });
            _scoreTimingNotify.Play(score, _noteAnimationTime);

            ScoreData.Note[] bpmChangeTimingNotes = new ScoreData.Note[score.Speeds.Length];
            for (int i = 0; i < bpmChangeTimingNotes.Length; i++)
            {
                bpmChangeTimingNotes[i] = new ScoreData.Note(score.Speeds[i].Beat, null, null, 0);
            }
            ScoreData bpmScore = new ScoreData(score.Speeds, bpmChangeTimingNotes, score.Offset);

            _bpmChangeNotify.RemoveNotifyCallBack(_bpmChangeNotifyKey);
            _bpmChangeNotifyKey = _bpmChangeNotify.AssignNotifyCallBack((_, v) =>
            {
                SetBPM((float)v[0]);
            });
            _bpmChangeNotify.Play(bpmScore, 0);

            //TODO : judgeCallBackとendCallBackを設定

        }

        private void GenerateNote(ScoreData.Note note, params double[] correctInputTimes)
        {
            ObjectPool usePool = null;
            if (correctInputTimes.Length == 1) usePool = _singleNoteObjectPool;
            else usePool = _longNoteObjectPool;

            var noteObject = usePool.GetObject(false);

            ScrollNoteObjectBase scrollBase = null;
            JudgeBase judgeBase = null;
            try
            {
                scrollBase = noteObject.GetComponent<ScrollNoteObjectBase>();
                judgeBase = noteObject.GetComponent<JudgeBase>();

                if (scrollBase == null) throw new Exception();
                if (judgeBase == null) throw new Exception();
            }
            catch
            {
                Debug.LogError("プレファブに必要なコンポーネントがアタッチされていません");
            }

            for (int i = 0; i < correctInputTimes.Length; i++) correctInputTimes[i] += _judgeOffset;
            judgeBase.SetCorrectInputTimes(correctInputTimes);
            //アクションA
            judgeBase.AssignJudgeResultCallback((judge, diff, end) =>
            {
                scrollBase.ReceiveJudgeCallback(judge);
                if (_extendNoteReactions != null || _extendNoteReactions != default)
                {
                    foreach (var re in _extendNoteReactions) re.ExtendJudgeCallBack(judge, note, diff);
                }

                if (end)
                {
                    int atNum = -1;

                    for(int i = 0; i < _notes.Count; i++)
                    {
                        if(_notes[i]._noteData == note)
                        {
                            atNum = i;
                            break;
                        }
                    }

                    if (atNum == -1)
                    {
                        Debug.LogError("ダメです．");
                    }

                    var dequedNote = _notes.DequeueAt(atNum);

                    if (judge == 0) _throwed.Enqueue(dequedNote);
                }
            });

            var newNote = new NoteData(judgeBase, scrollBase, note);
            judgeBase.AssignNotesObjectCompleteCallBack(() =>
            {
                //この時，このreturnしたオブジェクトがまだ_notesに残っている可能性がある
                //その場合は，エラーログを出す
                foreach(var note in _notes)
                {
                    if(note == newNote)
                    {
                        Debug.LogError("あかん．");
                        break;
                    }
                }

                usePool.ReturnObject(noteObject);

                if (_throwed.Count != 0)
                {
                    if (_throwed[0] == newNote)
                    {
                        _throwed.Dequeue();
                    }
                }
            });

            if (note.Endbeat == null)
            {
                if(noteObject.GetComponent<AutoHoldJudge>() ||
                   note.Endbeat == null && noteObject.GetComponent<KeyHoldNote>())
                {
                    Debug.LogError("ホールドノートを生成しようとしていますがendbeatが設定されていません");
                }
            }

            //アニメーションの遅延を計算する
            double animationDelay = AudioPlayingTime.PlayingTime - (correctInputTimes[0]　- _noteAnimationTime) + _animationOffset;
            //Debug.Log(animationDelay);
            newNote.Play(_noteAnimationTime, animationDelay, correctInputTimes);
            newNote.SetHS(HS);
            newNote.SetBPM(BPMRatio);

            _notes.Enqueue(newNote);
            if (newNote.endJudgeProgress)
            {
                Debug.LogError("ほげ");
            }
        }

        public void SetHS(float highSpeed)
        {
            if (_hsTweener != null) _hsTweener.Stop();
            _hsTweener = new TaskTweener(_hsTweenTime);

            var _ = _hsTweener.Play(HS, highSpeed, (v) =>
            {
                foreach (var note in _existNotes)
                {
                    note.SetHS(v);
                }
            });

            HS = highSpeed;
        }

        public void SetBPM(float ratio)
        {
            BPMRatio = ratio;

            Debug.Log("BPM Ratio : " + ratio);

            foreach (var note in _existNotes)
            {
                note.SetBPM(BPMRatio);
            }
        }

        public void Pause(bool pause)
        {
            foreach(var note in _existNotes)
            {
                note.Pause(pause);
            }
        }

        public void SetJudgeOffset(double judgeOffset)
        {
            _judgeOffset = judgeOffset;
        }

        public void SetDisplayOffset(double animationOffset)
        {
            _animationOffset = animationOffset;
        }

        public void SetExtendNoteReactionBases(params ExtendNoteReactionBase[] extendNoteReaction)
        {
            _extendNoteReactions = extendNoteReaction;
        }
    }
}

