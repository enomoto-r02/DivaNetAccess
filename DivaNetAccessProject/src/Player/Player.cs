using System;
using System.Globalization;
using System.Text;

namespace DivaNetAccess.src
{
    // プレイヤー情報クラス
    public class Player
    {
        public readonly static string FORMAT_DIRECTORY = "yyyyMMddHHmmss";
        public readonly static string FORMAT_COMBO_VIEW = "yyyy/MM/dd HH:mm:ss";
        public readonly static string SEPALATOR = "\n";
        public readonly static string FILE_NAME = "playerData.txt";
        public readonly static string DIR_NAME = "data";

        public virtual string FilePath
        {
            get
            {
                string ret;
                if (IsBase)
                {
                    ret = $"{AppDomain.CurrentDomain.BaseDirectory}{DIR_NAME}/{accessCode}/{FILE_NAME}";
                }
                else
                {
                    ret = $"{AppDomain.CurrentDomain.BaseDirectory}{DIR_NAME}/{accessCode}/{backupDateDir}/{FILE_NAME}";
                }
                return ret;
            }
            private set { }
        }

        public virtual string DirPath
        {
            get
            {
                string ret;
                if (IsBase)
                {
                    ret = $"{AppDomain.CurrentDomain.BaseDirectory}{DIR_NAME}/{accessCode}/";
                }
                else
                {
                    ret = $"{AppDomain.CurrentDomain.BaseDirectory}{DIR_NAME}/{accessCode}/{backupDateDir}/";
                }
                return ret;
            }
            private set { }
        }

        public string accessCode { get; private set; }
        public string password { get; private set; }
        // プレイヤー名
        public string name { get; set; }
        // VP
        public int vp { get; set; }

        // 称号
        public string rank { get; set; }

        // チケット
        public int ticket { get; set; }

        // ライバルコード
        public string rivalCode { get; set; }

        // 利用期限
        public DateTime limit { get; set; }

        // 最終取得日
        public DateTime getDate { get; set; }

        // 利用権＠ファイル出力はしない
        //public bool isAuthorization { get; set; }

        // メモ
        public string memo { get; set; }

        // バックアップ日時
        private DateTime? backupDate { get; set; }

        public string backupDateDir
        {
            get
            {
                return backupDate == null ? "" : backupDate.Value.ToString(Player.FORMAT_DIRECTORY);
            }
        }

        public string backupDateView
        {
            get
            {
                return backupDate == null ? "" : backupDate.Value.ToString(Player.FORMAT_COMBO_VIEW);
            }
        }

        public string playerComboView
        {
            get { return name; }
        }

        public string backupComboView
        {
            get
            {
                return $"バックアップ取得日 : {backupDateView}";
            }
        }

        public string Key
        {
            get
            {
                return string.IsNullOrEmpty(backupDateDir) ? accessCode : accessCode + "_" + backupDateDir;
            }
        }

        public bool IsBase
        {
            get
            {
                //return backupDate == null;
                if (backupDate == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /*
         * コンストラクタ＠DIVA.NET接続時
         */
        public Player()
        {
            backupDate = null;
        }

        /*
         * コンストラクタ＠DIVA.NET接続時
         */
        public Player(string accessCode, string password) : this()
        {
            this.accessCode = accessCode;
            this.password = password;
        }

        /*
         * コンストラクタ＠ファイル読み込み時
         */
        public Player(string[] lines) : this()
        {
            accessCode = lines[0];
            password = lines[1];
            name = lines[2];
            vp = int.Parse(lines[3]);
            rank = lines[4];
            ticket = int.Parse(lines[5]);
            rivalCode = lines[6];
            limit = DateTime.Parse(lines[7]);
            getDate = DateTime.Parse(lines[8]);
            memo = lines[9];
        }

        /*
         * コンストラクタ＠バックアップファイル読み込み時
         */
        public Player(string[] lines, string backupDirName) : this(lines)
        {
            if (!string.IsNullOrEmpty(backupDirName))
            {
                DateTime dt;
                if (DateTime.TryParseExact(backupDirName, Player.FORMAT_DIRECTORY, CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
                {
                    backupDate = dt;
                }
            }
        }

        /*
         * プレイヤー情報書き込み用
         */
        public string Write()
        {
            StringBuilder ret = new StringBuilder();

            ret.Append(accessCode + SEPALATOR);
            ret.Append(password + SEPALATOR);
            ret.Append(name + SEPALATOR);
            ret.Append(vp + SEPALATOR);
            ret.Append(rank + SEPALATOR);
            ret.Append(ticket + SEPALATOR);
            ret.Append(rivalCode + SEPALATOR);
            ret.Append(limit + SEPALATOR);
            ret.Append(getDate + SEPALATOR);
            ret.Append(memo + SEPALATOR);

            return ret.ToString();
        }

        /*
         * コンボボックス表示＠プレイヤーもバックアップも共通
         * プレイヤーコンボボックス：playerComboView(name)
         * バックアップコンボボックス：playerComboView(name)
         */
        public override string ToString()
        {
            return IsBase ? playerComboView : backupComboView;
        }
    }
}
