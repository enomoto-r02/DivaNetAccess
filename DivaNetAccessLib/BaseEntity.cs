using System;
using System.Collections.Generic;
using System.Text;

namespace DivaNetAccessLib
{
    public abstract class BaseEntity
    {
        public abstract string GetKey(params string[] data);

        public BaseEntity()
        {
            this.Line = "";
        }

        public BaseEntity(string line)
        {
            this.Line = line;
            this.Load();
        }

        protected string Line { get; }

        public abstract string Key { get; }

        public abstract void Load();

        public abstract override string ToString();
    }

    public static class BaseEntityUtil
    {

        public static List<BaseEntity> GetMergeEntityList(Dictionary<string, BaseEntity> beList1, Dictionary<string, BaseEntity> beList2)
        {
            // Dictionaryにすると現行分とキーが不一致になる可能性があるので、戻り値はList
            List<BaseEntity> ret = new List<BaseEntity>();
            List<string> keyList = new List<string>();

            foreach (string key in beList1.Keys)
            {
                keyList.Add(beList1[key].Key);
            }

            foreach (string key in beList2.Keys)
            {
                BaseEntity be = beList2[key];

                if (!keyList.Contains(be.Key))
                {
                    ret.Add(be);
                }
            }

            return ret;
        }
    }
}
