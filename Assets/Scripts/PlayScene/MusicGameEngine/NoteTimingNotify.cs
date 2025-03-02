namespace MusicGameEngine
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class NoteTimingNotify : TimingNotifyBase
    {
        void FixedUpdate()
        {
            if (_notes.Count != 0)
            {
                //Debug.Log(GetNoteTime(_notes[0].beat) + _offset);

                //ここ弄ってもノートの位置変わんなくね？
                //あくまでNoteをEnable状態にするだけであり、生成位置、密度に関わる計算はここでは行われない？！
                //ノートの位置計算に関わる部分を見直そう

                if (GetNotifyTime(_notes[0]) + _musicOffset < AudioPlayingTime.PlayingTime)
                {
                    // Debug.Log(GetNotifyTime(_notes[0].beat) + _offset);

                    if (_notes[0].Endbeat != null)
                    {
                        ExecuteNotifyCallBack(_notes[0], GetNoteTime(_notes[0]) + _musicOffset, GetEndNoteTIme(_notes[0]) + _musicOffset);
                    }
                    else
                    {
                        ExecuteNotifyCallBack(_notes[0], GetNoteTime(_notes[0]) + _musicOffset);
                    }

                    _notes.Dequeue();

                }



            }
        }
    }
}