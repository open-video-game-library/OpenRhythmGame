using System;
using System.Collections.Generic;
using UnityEngine;

namespace MusicGameEngine
{
	[Serializable]
    public class ScoreData
    {
        [Serializable]
        public class Speed
        {
			[SerializeField] private int[] beat;
			[SerializeField] private float bpm;

            public int[] Beat => beat;
            public float BPM => bpm;

			public double BeatT { get; private set; }

			public void SetBeatT(double t)
			{
                BeatT = t;
            }

			public Speed(int[] beat, float bpm)
			{
				this.beat = beat;
				this.bpm = bpm;
			}
        }

        [Serializable]
        public class Note
        {
            [SerializeField] private int[] beat;
            [SerializeField] private int[] endbeat;
            [SerializeField] private float[] additional;
            [SerializeField] private int column;

            public int[] Beat => beat;
            public int[] Endbeat => endbeat;
            public float[] Additional => additional;
            public int Column => column;	

            public double BeatT { get; private set; }
            public double EndBeatT { get; private set; }

            public void SetBeatT(double t)
			{
				BeatT = t;
			}
			public void SetEndBeatT(double t)
			{
                EndBeatT = t;
            }

			public Note(int[] beat, int[] endbeat, float[] additional, int column)
			{
                this.beat = beat;
                this.endbeat = endbeat;
                this.additional = additional;
                this.column = column;
            }
		}

		[SerializeField] private Speed[] speed;
        [SerializeField] private Note[] note;
        [SerializeField] private float offset;

		public Speed[] Speeds => speed;
        public Note[] Notes => note;
		public float Offset => offset;

		//note�̎��Ԃ�ݒ�A���������s��
		public void PreCalculateNoteTime()
		{
			//speed�̎��Ԃ̏�����
			for(int i = 1; i < Speeds.Length; i++)
			{
				if (Speeds[i].BeatT != 0) continue;

				double t = 60 / Speeds[i].BPM * SubBeat(Speeds[i].Beat, Speeds[i-1].Beat);
				Speeds[i].SetBeatT(t);
            }

			//note�̎��Ԃ̏�����
			foreach(var note in Notes)
			{
				if (note.BeatT != 0) continue;

				{
                    int speedDex = 0;
                    for (int i = 0; i < Speeds.Length; i++)
                    {
                        if (SubBeat(note.Beat, Speeds[i].Beat) >= 0) speedDex = i;
                    }

                    //beat, bpm
                    List<(int[], float)> points = new List<(int[], float)>();
                    for (int i = 0; i <= speedDex; i++)
					{
                        points.Add((Speeds[i].Beat, Speeds[i].BPM));
                    }
                    points.Add((note.Beat, Speeds[speedDex].BPM));

                    double t = 0;
                    for (int i = 1; i < points.Count; i++)
                    {
                        t += 60 / points[i].Item2 * SubBeat(points[i].Item1, points[i - 1].Item1);
                    }

                    note.SetBeatT(t);
                }

				//endBeat�����݂���ꍇ�́A���̕����v�Z����
				if(note.Endbeat != null)
				{
                    int speedDex = 0;
                    for (int i = 0; i < Speeds.Length; i++)
                    {
                        if (SubBeat(note.Endbeat, Speeds[i].Beat) >= 0) speedDex = i;
                    }

                    //beat, bpm
                    List<(int[], float)> points = new List<(int[], float)>();
                    for (int i = 0; i <= speedDex; i++)
					{
                        points.Add((Speeds[i].Beat, Speeds[i].BPM));
                    }
                    points.Add((note.Endbeat, Speeds[speedDex].BPM));

                    double t = 0;
                    for (int i = 1; i < points.Count; i++)
                    {
                        t += 60 / points[i].Item2 * SubBeat(points[i].Item1, points[i - 1].Item1);
                    }

                    note.SetEndBeatT(t);
                }
			}
		}

		public ScoreData(Speed[] speeds, Note[] notes, float offset)
		{
            this.speed = speeds;
            this.note = notes;
            this.offset = offset;

			PreCalculateNoteTime();
        }

		/// <summary>
		/// a-b��beat�̌��Z���ʂ�Ԃ�
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float SubBeat(int[] ia, int[] ib)
		{
			//a��b��float�̔z��ɕϊ�
			float[] fa = new float[3];
			float[] fb = new float[3];
            for (int i = 0; i < 3; i++)
			{
				fa[i] = (float)ia[i];
				fb[i] = (float)ib[i];
			}

            float m = (fa[0] - fb[0]);
			float b = (fa[1] / fa[2]) - (fb[1] / fb[2]);

			return m + b;
        }

		//���̊֐������������삷�邩�m�F����
		public static double GetLongNoteTime(ScoreData score, Note note)
		{
			if (note.Endbeat == null) return 0;

			float retT = 0;
			Speed[] speeds = GetNoteBPMs(score, note);

			List<int[]> points = new List<int[]>();
			points.Add(note.Beat);
			for(int i = 0; i < speeds.Length; i++)
			{
				if (i == 0) continue;
				points.Add(speeds[i].Beat);
			}
			points.Add(note.Endbeat);

			for(int i = 0; i < points.Count-1; i++)
			{
				float bpm = speeds[i].BPM;
				float beatL = SubBeat(points[i+1], points[i]);
				retT += beatL * (60 / bpm);
			}

			return retT;
		}

        /// <summary>
        /// ���̃m�[�g��BPM���擾����
		/// �����̔z��́A�V���O���m�[�g�̏ꍇ��1�A�����O�m�[�g�̏ꍇ�͕���
        /// </summary>
        /// <param name="score"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public static Speed[] GetNoteBPMs(ScoreData score, Note note)
        {
			List<Speed> retBPMs = new List<Speed>();

			//�m�[�g���V���O���m�[�g�̏ꍇ
            if (note.Endbeat == null)
            {
                Speed BPM = null;

                float sBeatT = note.Beat[0] + (note.Beat[1] / note.Beat[2]);

                for (int i = 0; i < score.Speeds.Length; i++)
				{
					float tBeatT = score.Speeds[i].Beat[0] + (score.Speeds[i].Beat[1] / score.Speeds[i].Beat[2]);

					if (tBeatT <= sBeatT) BPM = score.Speeds[i];
					else break;
                }

				retBPMs.Add(BPM);
			}
			//�m�[�g�������O���[�g�̏ꍇ
			else 
			{ 
                List<Speed> BPMs = new List<Speed>();

				Speed sBPM = null;
				Speed eBPM = null;

                float sBeatT = note.Beat[0] + (note.Beat[1] / note.Beat[2]);    
                float eBeatT = note.Endbeat[0] + (note.Endbeat[1] / note.Endbeat[2]);

                int sIndex = 0;
                for (int i = 0; i < score.Speeds.Length; i++)
                {
                    float tBeatT = score.Speeds[i].Beat[0] + (score.Speeds[i].Beat[1] / score.Speeds[i].Beat[2]);

                    if (tBeatT <= sBeatT)
					{
                        sBPM = score.Speeds[i];
						sIndex = i;
                    }
                    else break;
                }

				int eIndex = 0;
                for (int i = sIndex; i < score.Speeds.Length; i++)
                {
                    float tBeatT = score.Speeds[i].Beat[0] + (score.Speeds[i].Beat[1] / score.Speeds[i].Beat[2]);

                    if (tBeatT <= eBeatT)
                    {
                        eBPM = score.Speeds[i];
                        eIndex = i;
                    }
                    else break;
                }

				BPMs.Add(sBPM);
				if(sIndex != eIndex)
				{
					for (int i = sIndex + 1; i <= eIndex; i++)
					{
						BPMs.Add(score.Speeds[i]);
                    }
				}
				BPMs.Add(eBPM);

                retBPMs = BPMs;
            }

			return retBPMs.ToArray();
        }
    }
}

/*
{
	"speed": [
		{
			"beat": [
				0,
				0,
				4
			],
			"bpm": 156
		}
	],
	"note": [
		{
			"beat": [
				22, // ���ߐ�
				0,  // ����
				4   // �����̕���
			],
			"endbeat": [
				23,
				0,
				4
			],
			"additional": [
				0, //�L�[��
				0, //�G�t�F�N�^�[���
				0, //�G�t�F�N�^�[�ɐݒ肷��value(�K�v�ȕ��̂�)
				0, //column=1,2,3�ɂ������ԍ�
				0, //arrow�m�[�g�ɂ�������̌��� 1:�� 2:��
				0,�@//�G�t�F�N�^�[�J�[�u�l
				0,
				0,
				0,
				0
			],
			"column": 0
		},
	],
	"offset": 0 // �b�\�L
}

---Additional�̖𗧂��---
�E�G�t�F�N�^�[���
0:�Ȃ�
1:Lowcut
	defValue��1
2:Highcut
	defValue��0
3:Loop
    0.5:1/2
    1  :1/1
    2  :1/2
    4  :1/4
	8  :1/8
4:Cut
    4  :1/4
	8  :1/8
	16 :1/16
5:Flange
	value�̐ݒ�͕K�v�Ȃ�



*/