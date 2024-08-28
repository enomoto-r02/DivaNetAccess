using System;
using System.Text;

namespace Convert._20120711
{
    // プレイ履歴書き込み時のクラス
    public class oldPlayRecordEntity
    {
        private const string DATE_FORMAT_PLAY_RECORD = "yyyy/MM/dd HH:mm";

        // 区切り文字
        private readonly string SEPALATOR = "\t";

        // 削除フラグ
        public bool delFlg { get; set; }

        // No
        public int no { get; set; }

        // 日時
        public string date { get; set; }

        // 場所
        public string place { get; set; }

        // 曲名
        public string name { get; set; }

        // 難易度
        public string diff { get; set; }

        // CLEAR RANK
        public string clear { get; set; }

        // 達成率
        public string tasseiritu { get; set; }

        // SCORE
        public string score { get; set; }

        // COOL
        public string cool { get; set; }

        // FINE
        public string fine { get; set; }

        // SAFE
        public string safe { get; set; }

        // SAD
        public string sad { get; set; }

        // WORST/WRONG
        public string worst { get; set; }

        // COMBO
        public string combo { get; set; }

        // CHALLENGE TIME
        public string challenge { get; set; }

        // 同時押し/ホールド
        public string hold { get; set; }

        // トライアル
        public string trial { get; set; }

        // モジュール1
        public string module1 { get; set; }

        // モジュール2
        public string module2 { get; set; }

        // ボタン音
        public string button { get; set; }

        // スキン
        public string skin { get; set; }

        // キー
        public string key { get; set; }

        /*
         * コンストラクタ
         */
        public oldPlayRecordEntity()
        {
        }

        /*
         * ファイル読み込み用
         */
        public oldPlayRecordEntity(string line)
        {
            string[] data = line.Split(char.Parse(SEPALATOR));

            delFlg = false;

            DateTime tmpDate = DateTime.Parse(data[0]);
            date = tmpDate.ToString(DATE_FORMAT_PLAY_RECORD);
            place = data[1];
            name = data[2];
            diff = data[3];
            clear = data[4];
            tasseiritu = data[5];
            score = data[6];
            cool = data[7];
            fine = data[8];
            safe = data[9];
            sad = data[10];
            worst = data[11];
            combo = data[12];
            challenge = data[13];
            hold = data[14];
            trial = data[15];
            module1 = data[16];
            module2 = data[17];
            button = data[18];
            skin = data[19];

            makeKey();

            tasseiritu = tasseiritu.Replace("％", "");
        }

        /*
         * ファイル書き込み用
         */
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();

            ret.Append(date + SEPALATOR);
            ret.Append(place + SEPALATOR);
            ret.Append(name + SEPALATOR);
            ret.Append(diff + SEPALATOR);
            ret.Append(clear + SEPALATOR);
            ret.Append(tasseiritu + SEPALATOR);
            ret.Append(score + SEPALATOR);
            ret.Append(cool + SEPALATOR);
            ret.Append(fine + SEPALATOR);
            ret.Append(safe + SEPALATOR);
            ret.Append(sad + SEPALATOR);
            ret.Append(worst + SEPALATOR);
            ret.Append(combo + SEPALATOR);
            ret.Append(challenge + SEPALATOR);
            ret.Append(hold + SEPALATOR);
            ret.Append(trial + SEPALATOR);
            ret.Append(module1 + SEPALATOR);
            ret.Append(module2 + SEPALATOR);
            ret.Append(button + SEPALATOR);
            ret.Append(skin);

            ret.Append("\n");

            return ret.ToString();
        }

        public void makeKey()
        {
            key = date + place + name + diff + clear + tasseiritu + score + cool + fine + safe + sad + worst
                + combo + challenge + hold + trial + module1 + module2 + button + skin;
        }
    }
}
