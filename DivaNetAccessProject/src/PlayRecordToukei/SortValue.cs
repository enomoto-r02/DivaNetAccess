using System;

namespace DivaNetAccess.src.PlayRecordToukei
{
    //コレクションで並び替えができるクラスの定義
    //IComparable<T>ジェネリックインターフェイスを実装する
    public class SortValue : IComparable, IComparable<SortValue>
    {
        // 名称
        public string name { get; private set; }

        // カウント
        public int value { get; private set; }

        // 条件等
        public int keyInt { get; set; }

        // 条件等
        public int keyString { get; set; }

        //コンストラクタ
        public SortValue(string name, int value)
        {
            this.name = name;
            this.value = value;
        }

        //自分自身がotherより小さいときはマイナスの数、大きいときはプラスの数、
        //同じときは0を返す
        public int CompareTo(SortValue other)
        {
            //nullより大きい
            if (other == null)
            {
                return 1;
            }

            //Priceを比較する
            return -value.CompareTo(other.value);
        }

        public int CompareTo(object obj)
        {
            //nullより大きい
            if (obj == null)
            {
                return 1;
            }

            //違う型とは比較できない
            if (GetType() != obj.GetType())
            {
                throw new ArgumentException("別の型とは比較できません。", "obj");
            }

            return -CompareTo((SortValue)obj);
        }
    }


}
