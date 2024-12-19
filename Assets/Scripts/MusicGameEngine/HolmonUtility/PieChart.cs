using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieChart : MonoBehaviour
{
    [SerializeField]
    private GameObject CircleImage;

    [SerializeField]
    private float AnimationSpeed = 1.0f;

    [SerializeField]
    private Color[] _color;

    private bool updateFlg;
    private float ListValueAdd;
    private int SkipCnt;
    private float[] DataList;
    private GameObject[] ChildObject;

    // Update is called once per frame
    void Update()
    {
        if (updateFlg)
        {
            for (int i = SkipCnt; i >= 0; i--)
            {
                ChildObject[i].GetComponent<Image>().fillAmount += AnimationSpeed * Time.deltaTime;
                float _work = 0;
                for (int j = 0; j < DataList.Length - SkipCnt; j++)
                {
                    _work += DataList[j];
                }
                if (ChildObject[i].GetComponent<Image>().fillAmount >= _work / ListValueAdd)
                {
                    ChildObject[i].GetComponent<Image>().fillAmount = _work / ListValueAdd;
                    SkipCnt--;
                }
            }

            if (SkipCnt < 0)
            {
                updateFlg = false;
                ListValueAdd = 0;
                print("end");
            }
        }
    }

    // �q�I�u�W�F�N�g�̃��Z�b�g
    private void ResetCircle()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }

    // �~�O���t�ݒ�(�A�j���[�V�����Ȃ�)
    public void SetPieChartNormal(float[] _set)
    {
        ResetCircle();

        DataList = new float[_set.Length];
        DataList = _set;
        ChildObject = new GameObject[_set.Length];

        // �F�w�肪����Ă��Ȃ��ꍇ�����_���ŐF��ǉ�
        if (_color.Length < DataList.Length)
        {
            List<Color> list = new List<Color>(_color);
            for (int i = _color.Length; i < DataList.Length; i++)
            {
                list.Add(new Color(UnityEngine.Random.Range(0.5f, 1.0f), UnityEngine.Random.Range(0.5f, 1.0f), UnityEngine.Random.Range(0.5f, 1.0f)));
            }
            _color = list.ToArray();
        }

        // �~�̕���
        for (int i = 0; i < DataList.Length; i++)
        {
            ChildObject[i] = Instantiate(CircleImage, Vector3.zero, Quaternion.identity, this.transform);
            ChildObject[i].transform.localPosition = new Vector3(0, 0, 0);
            ChildObject[i].GetComponent<Image>().color = _color[DataList.Length - 1 - i];
            ChildObject[i].GetComponent<Image>().fillAmount = 0;
            ChildObject[i].SetActive(true);
        }

        // �S�̂̐��l���v�Z
        for (int i = 0; i < DataList.Length; i++)
        {
            ListValueAdd += DataList[i];
        }

        for (int i = DataList.Length - 1; i >= 0; i--)
        {
            float _work = 0;
            for (int j = 0; j < DataList.Length - i; j++)
            {
                _work += DataList[j];
            }
            print(ListValueAdd);
            ChildObject[i].GetComponent<Image>().fillAmount = _work / ListValueAdd;
        }

        ListValueAdd = 0;
    }
}
