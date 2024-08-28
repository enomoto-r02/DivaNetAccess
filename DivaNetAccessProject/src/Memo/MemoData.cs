using DivaNetAccessLib;
using System.Text;

namespace DivaNetAccess
{
    public class MemoData : BaseEntity
    {
        /*
         * キー
         * name + "." + diff
         */
        public override string GetKey(params string[] data)
        {
            if (data.Length != 2)
            {
                throw new System.NotImplementedException();
            }

            return data[0] + "." + data[1];
        }

        public readonly static string DATA_SEPARATOR = "\t";
        public readonly static string LINE_SEPARATOR = "\n";

        public override string Key { get { return name + "." + diff; } }

        // 曲名
        public string name { get; set; }

        // 難易度
        public string diff { get; set; }

        // メモ欄
        public string memo { get; set; }

        // メモ欄2
        public string memo2 { get; set; }

        public MemoData(string name, string diff)
        {
            this.name = name;
            this.diff = diff;
        }

        public MemoData(string name, string diff, string memo)
        {
            this.name = name;
            this.diff = diff;
            this.memo = memo;
        }

        public MemoData(string line) : base(line)
        {
        }

        public override void Load()
        {
            string[] data = Line.Split(char.Parse(MemoData.DATA_SEPARATOR));

            name = data[0];
            diff = data[1];
            memo = data[2];
            memo2 = data[3];
        }

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();

            ret.Append(name + MemoData.DATA_SEPARATOR);
            ret.Append(diff + MemoData.DATA_SEPARATOR);
            ret.Append(memo + MemoData.DATA_SEPARATOR);
            ret.Append(memo2);

            return ret.ToString();
        }
    }
}
