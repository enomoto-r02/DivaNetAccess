using System;
using System.Collections.Generic;
using System.Text;

namespace DivaNetAccess.src.myList
{
    public class MyListEntity
    {
        // 区切り文字
        private readonly string SEPALATOR = "\n";

        // マイリスト情報
        public List<myListData> myLists = new List<myListData>();

        /*
         * コンストラクタ
         */
        public MyListEntity()
        {
        }

        /*
         * コンストラクタ(ファイル読み込み用)
         */
        public MyListEntity(string fileStr)
        {
            // 空白行は読み込まない
            string[] lines = fileStr.Split(new string[] { SEPALATOR }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                myListData myList = new myListData(line);

                myLists.Add(myList);
            }
        }

        /*
         * ファイル書き込み用
         */
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();

            foreach (myListData ml in myLists)
            {
                ret.Append(ml.ToString() + SEPALATOR);
            }

            return ret.ToString();
        }

        /*
         * 指定されたインデックスのマイリストに含まれるマイリスト情報を取得
         */
        public List<string> getMyListFromIndex(int index)
        {
            List<string> ret = new List<string>();

            foreach (myListData myList in myLists)
            {
                if (myList.listNo == index)
                {
                    ret.Add(myList.songName);
                }
            }

            return ret;
        }
    }


    public class myListData
    {
        // 区切り文字
        private readonly string SEPALATOR = "\t";

        // マイリストNo
        public int listNo;

        // マイリスト名
        public string listName;

        // 最終更新時刻
        public DateTime updateDate;

        // 筐体使用マイリスト
        //   ※マイリストA～Cどれに使われているかは現状不明。今後区別できた時のためにintで定義
        public int useKyotaiNo;

        // 楽曲No
        public int songNo;

        // 楽曲名
        public string songName;

        /*
         * コンストラクタ
         */
        public myListData()
        {
        }

        /*
         * コンストラクタ(ファイル読み込み用)
         */
        public myListData(string line)
        {
            string[] data = line.Split(char.Parse(SEPALATOR));

            listNo = int.Parse(data[0]);
            listName = data[1];
            updateDate = DateTime.Parse(data[2]);
            useKyotaiNo = int.Parse(data[3]);
            songNo = int.Parse(data[4]);
            songName = data[5];
        }

        /*
         * ファイル書き込み用
         */
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();

            ret.Append(listNo + SEPALATOR);
            ret.Append(listName + SEPALATOR);
            ret.Append(updateDate.ToString() + SEPALATOR);
            ret.Append(useKyotaiNo.ToString() + SEPALATOR);
            ret.Append(songNo + SEPALATOR);
            ret.Append(songName);

            return ret.ToString();
        }
    }
}
