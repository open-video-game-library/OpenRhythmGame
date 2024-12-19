import json

score_pass = "hitomania.json"

def main():
    jsonFile = open(score_pass, "r")
    scoreData = json.load(jsonFile)

    # for文でNoteの各要素にアクセス

    notes = scoreData["note"]
    for note in notes:
        additionalAdder(note) # Additionalが設定されていない場合追加処理をおこなう
        #modifyColumn(note) # column番号を必要に応じて変化させる
        #-----プレイヤー側-----
        #keySoundSet(note, 0) # column=0or3のタップノートに対してキー音のaddi1tional情報をセットする
        #effectorSet(note, 0) # column=0or3のホールドノートに対してエフェクターに必要なadditional情報をセットする
        #scrSoundSet(note, 1) # column=1or4のノートに対してスクラッチ音のadditional情報をセットする
        #scrHoldSoundSet(note, 100) # column=100or101のノートに対してスクラッチ音のadditional情報をセットする
        #arrowSet(note, 2) # column=2or5のノートに対して矢印のadditional情報をセットする
        #-----オート側-----
        #keySoundSet(note, 3) # column=0or3のタップノートに対してキー音のadditional情報をセットする
        #effectorSet(note, 3) # column=0or3のホールドノートに対してエフェクターに必要なadditional情報をセットする
        #scrSoundSet(note, 4) # column=1or4のノートに対してスクラッチ音のadditional情報をセットする
        #scrHoldSoundSet(note, 101) # column=100or101のノートに対してスクラッチ音のadditional情報をセットする
        #arrowSet(note, 5) # column=2or5のノートに対して矢印のadditional情報をセットする
    print(notes)
    scoreData["note"] = notes
    with open(score_pass, "w") as f:
        json.dump(scoreData, f, indent=4)



def additionalAdder(note):
    exist = "additional" in note
    if(exist == False):
        note["additional"] = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0,]

def modifyColumn(note):
    #スクラッチロングノートのcolumn修正
    if(note["column"] == 1):
        exist = "endbeat" in note
        if(exist == True):
            note["column"] = 100
    if(note["column"] == 4):
        exist = "endbeat" in note
        if(exist == True):
            note["column"] = 101

def keySoundSet(note, column):
    if(note["column"] == column):
        exist = "endbeat" in note
        if(exist == True): return
        print("-----keySoundSet-----")
        print(note)
        res = input("キー音のindex番号を入力してください。設定スキップは-1を入力:")
        if(is_integer(res) == False): return
        if(res == ""): res = -1
        if(float(res) == -1 or res == ""): return
        note["additional"][0] = float(res)

def effectorSet(note, column):
    if(note["column"] == column):
        exist = "endbeat" in note
        if(exist == True):
            print("-----effectorSet-----")
            print(note)
            res = input("エフェクターの種類番号を入力してください。設定スキップは-1を入力:")
            if(is_integer(res) == False): return
            if(res == ""): res = -1
            if(float(res) != -1):
                note["additional"][1] = float(res)
                if(float(res) == 1 or float(res) == 2):
                    res = input("エフェクターに設定するカーブ番号を入力してください:")
                    if(is_integer(res) == False): return
                    note["additional"][5] = float(res)
                else:
                    res = input("エフェクターに設定するvalueを入力してください:")
                    if(is_integer(res) == False): return
                    note["additional"][2] = float(res)

def scrSoundSet(note, column):
    if(note["column"] == column):
        print("-----scrSoundSet-----")
        print(note)
        res = input("スクラッチ音のindex番号を入力してください。設定スキップは-1を入力:")
        if(is_integer(res) == False): return
        if(res == ""): res = -1
        if(float(res) == -1 or res == ""): return
        note["additional"][0] = float(res)

def scrHoldSoundSet(note, column):
    if(note["column"] == column):
        print("-----scrHoldSoundSet-----")
        print(note)
        res = input("スクラッチ音のindex番号を入力してください。設定スキップは-1を入力:")
        if(is_integer(res) == False): return
        if(res == ""): res = -1
        if(float(res) == -1 or res == ""): return
        note["additional"][0] = float(res)

def arrowSet(note, column):
    if(note["column"] == column):
        print("-----arrowSet-----")
        print(note)
        res = input("矢印の音のindex番号を入力してください。設定スキップは-1を入力:")
        if(is_integer(res) == False): return
        if(res == ""): res = -1
        if(float(res) == -1 or res == ""): return
        note["additional"][0] = float(res)
        res = input("矢印の向き番号を入力してください。設定スキップは-1を入力:")
        if(is_integer(res) == False): return
        if(res == ""): res = -1
        if(float(res) == -1 or res == ""): return
        note["additional"][4] = float(res)

def is_integer(str):
    try:
        # 入力値を整数に変換してみる
        int(str)
        return True
    except ValueError:
        # 整数に変換できない場合はFalseを返す
        print("入力値エラー：スキップします")
        return False

if __name__ == "__main__":
    main()

'''
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
				22, // 小節数
				0,  // 拍数
				4   // 拍数の分母
			],
			"endbeat": [
				23,
				0,
				4
			],
			"additional": [
				0, //キー音
				0, //エフェクター種類
				0, //エフェクターに設定するvalue(必要な物のみ)
				0, //column=1,2,3における列番号
				0, //arrowノートにおける矢印の向き 1:上 2:下
				0, //エフェクターカーブ値
				0,
				0,
				0,
				0
			],
			"column": 0
		},
	],
	"offset": 0 // 秒表記
}

---Additionalの役立つ情報---
・エフェクター種類
0:なし
1:Lowcut
	defValueは1
2:Highcut
	defValueは0
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
	valueの設定は必要ない



'''