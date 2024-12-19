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

                //�����M���Ă��m�[�g�̈ʒu�ς��Ȃ��ˁH
                //�����܂�Note��Enable��Ԃɂ��邾���ł���A�����ʒu�A���x�Ɋւ��v�Z�͂����ł͍s���Ȃ��H�I
                //�m�[�g�̈ʒu�v�Z�Ɋւ�镔������������

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