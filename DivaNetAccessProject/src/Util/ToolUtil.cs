namespace DivaNetAccess.src.Util
{
    public static class ToolUtil
    {
        /*
         * 全角数字→半角数字
         */
        public static string convNum(string target)
        {
            string ret = target;

            ret = ret.Replace("０", "0");
            ret = ret.Replace("１", "1");
            ret = ret.Replace("２", "2");
            ret = ret.Replace("３", "3");
            ret = ret.Replace("４", "4");
            ret = ret.Replace("５", "5");
            ret = ret.Replace("６", "6");
            ret = ret.Replace("７", "7");
            ret = ret.Replace("８", "8");
            ret = ret.Replace("９", "9");

            return ret;
        }

        /*
         * 全角記号→半角記号
         * 　一般的な変換ではないものも含むので、使う前に確認すること
         */
        public static string convKigo(string target)
        {
            string ret = target;

            ret = ret.Replace("～", "-");
            ret = ret.Replace("．", ".");
            ret = ret.Replace("、", ",");
            ret = ret.Replace("　", " ");

            return ret;
        }
    }
}
