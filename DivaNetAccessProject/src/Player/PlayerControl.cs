using DivaNetAccess.src.Const;
using DivaNetAccess.src.util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DivaNetAccess.src
{
    public class PlayerControl
    {
        private Dictionary<string, Player> Players;
        private Dictionary<string, Player> Backups;
        private Dictionary<string, Rival> Rivals;
        private List<PlayerCombo> _PlayerCombos;
        private List<PlayerCombo> _BackupCombos;
        private List<PlayerCombo> _RivalCombos;

        public Player ActivePlayer;         // 選択中プレイヤー
        public Player ActivePlayerBase;     // 選択中プレイヤーのbase
        public Player ActivePlayerTo;       // 読み込み先プレイヤー

        public Rival ActiveRival;         // 選択中プレイヤー
        public Rival ActiveRivalBase;     // 選択中プレイヤーのbase
        public Rival ActiveRivalTo;       // 読み込み先プレイヤー

        public int getPlayersCount { get { return Players.Count; } private set { } }

        public string getPlayerNameLabel
        {
            get
            {
                string ret = "";
                if (ActivePlayer != null)
                {
                    if (ActivePlayer.IsBase)
                    {
                        ret = $"{ActivePlayer.playerComboView}";
                    }
                    else
                    {
                        ret = $"{ActivePlayer.playerComboView} [{ActivePlayer.backupComboView}]";
                    }
                }
                return ret;
            }
        }

        public PlayerControl(List<PlayerCombo> playerCombos, List<PlayerCombo> backupCombos, List<PlayerCombo> rivalCombos)
        {
            // プレイヤーリストをコンボボックスに追加
            Players = new Dictionary<string, Player>();
            Backups = new Dictionary<string, Player>();
            Rivals = new Dictionary<string, Rival>();
            _PlayerCombos = playerCombos;
            _BackupCombos = backupCombos;
            _RivalCombos = rivalCombos;
        }

        /*
         * プレイヤー情報をクリア
         * 　ActivePlayer等は削除されないので、プレイヤー情報クリア後に再度読み込む場合は以下の手順を行う
         * 　1. ActivePlayerToにプレイヤーを設定
         * 　2. 本メソッドでプレイヤー情報のリストをクリア
         * 　3. LoadPlayersを呼び出し、1を含むプレイヤー情報をリストに読み込む
         * 　4. LoadPlayerByActivePlayerToを呼び出す
         */
        public void initPlayers()
        {
            Players = new Dictionary<string, Player>();
            Backups = new Dictionary<string, Player>();

            foreach (PlayerCombo pc in _PlayerCombos)
            {
                pc.Items.Clear();
            }

            foreach (PlayerCombo bc in _BackupCombos)
            {
                bc.Items.Clear();
            }
        }

        /*
         * プレイヤー情報読み込み
         * 　accessCodeからプレイヤーを読み込み、ActivePlayerにセットする
         * 　
         * 　ActivePlayerToはnullを設定する
         * 　isAddPlayers：Playersに追加する場合はTrue
         */
        private Player _LoadPlayer(string path, string backupDirName = "")
        {
            // プレイヤー情報生成
            Player ret = null;

            // プレイヤー情報ファイル確認
            if (File.Exists(path) == false)
            {
                return ret;
            }

            // ファイルを開く
            using (StreamReader sr = new StreamReader(
                path,
                SettingConst.FILE_ENCODING
            ))
            {
                // ファイルをすべて読み込む
                string fileStr = sr.ReadToEnd();

                // プレイヤー情報設定
                ret = new Player(fileStr.Split('\n'), backupDirName);
            }

            return ret;
        }

        /*
         * プレイヤー情報読み込み
         * 　accessCodeからプレイヤーを読み込み、ActivePlayerにセットする
         * 　
         * 　ActivePlayerToはnullを設定する
         * 　isAddPlayers：Playersに追加する場合はTrue
         */
        public void LoadPlayerByAccessCode(string accessCode)
        {
            if (string.IsNullOrEmpty(accessCode))
            {
                return;
            }

            string path = "./" + SettingConst.DATA_DIR_NAME + "/" + accessCode + "/" + Player.FILE_NAME;
            ActivePlayer = _LoadPlayer(path);
        }

        /*
         * プレイヤー情報読み込み
         * 　ActivePlayerToに設定されているプレイヤーを読み込み、ActivePlayerにセットする
         * 　ActivePlayerToが設定されていなければ、ActivePlayerに設定されているプレイヤーを再読み込みする
         * 　
         * 　ActivePlayerToはnullを設定する
         * 　isAddPlayers：Players(コンボボックス)に追加する場合はTrue
         */
        public void LoadPlayerByActivePlayerToOrRefreshActivePlayer(bool isAddPlayers)
        {
            Player p;
            if (ActivePlayerTo != null)
            {
                p = _LoadPlayer(ActivePlayerTo.FilePath, ActivePlayerTo.backupDateDir);
            }
            else if (ActivePlayer != null)
            {
                p = _LoadPlayer(ActivePlayer.FilePath, ActivePlayer.backupDateDir);
            }
            else
            {
                return;
            }

            Dictionary<string, Player> addList;

            if (isAddPlayers)
            {
                if (p.IsBase)
                {
                    addList = Players;
                }
                else
                {
                    addList = Backups;
                }
                addList.Add(p.Key, p);
            }

            if (!p.IsBase)
            {
                // Baseプレイヤーの情報を保持
                ActivePlayerBase = ActivePlayer;
            }

            foreach (PlayerCombo pc in _PlayerCombos)
            {
                pc.Text = p.playerComboView;
            }

            ActivePlayer = p;
            ActivePlayerTo = null;
        }

        /*
         * プレイヤー情報読み込み＠初期表示時
         */
        private void _LoadPlayers(string dirPath, Dictionary<string, Player> ps, List<PlayerCombo> cs)
        {
            // フォルダが無い
            if (Directory.Exists(dirPath) == false)
            {
                return;
            }

            // フォルダ一覧取得
            string[] dirNames = Directory.GetDirectories(dirPath, "*", SearchOption.TopDirectoryOnly);

            // フォルダ分ループ
            foreach (string dirName in dirNames)
            {
                // フォルダ名の形式チェック
                var reg = new Regex(@"\.\/data\\\d{20}$");
                if (!reg.IsMatch(dirName))
                {
                    continue;
                }

                string accessCode = Path.GetFileName(dirName);

                LoadPlayerByAccessCode(accessCode);

                if (ActivePlayer != null)
                {
                    ps.Add(ActivePlayer.Key, ActivePlayer);
                }
            }

            DivaNetUtil.getSortValue2(Players);

            foreach (ComboBox c in cs)
            {
                foreach (string key in Players.Keys)
                {
                    Player player = Players[key];
                    c.Items.Add(player);

                    /*
                    if (ActivePlayerTo != null && ActivePlayerTo.Key == player.Key)
                    {
                        c.SelectedItem = player;
                    }
                    */
                }
                //c.SelectedItem = ActivePlayerTo;
            }
            ActivePlayer = null;
            //ActivePlayerTo = null;
        }

        public void LoadPlayers()
        {
            initPlayers();

            string dirPath = "./" + SettingConst.DATA_DIR_NAME;
            _LoadPlayers(dirPath, Players, _PlayerCombos);


            // 1プレイヤーだったら読み込む
            /*
            if (Players.Count == 1)
            {
                foreach (string key in Players.Keys)
                {
                    ActivePlayerTo = Players[key];
                }
            }
            */

            LoadPlayerByActivePlayerToOrRefreshActivePlayer(false);
        }

        public void LoadRivals()
        {
            string dirPath = ActivePlayer.DirPath;
#warning 実装
            //_LoadPlayers(dirPath);
        }

        public void LoadBackups()
        {
            _LoadBackups();
        }

        /*
         * プレイヤー情報バックアップ読み込み＠初期表示時
         */
        private void _LoadBackups()
        {
            // フォルダが無い
            if (Directory.Exists(ActivePlayer.DirPath) == false)
            {
                return;
            }

            Backups.Clear();
            Backups.Add(ActivePlayer.Key, ActivePlayer);

            // フォルダ一覧取得
            string[] dirPaths = Directory.GetDirectories($"{ActivePlayer.DirPath}", "*", SearchOption.TopDirectoryOnly);

            foreach (string dirPath in dirPaths)
            {
                string dirName = Path.GetFileName(dirPath);

                DateTime backupDate;

                if (DateTime.TryParseExact(dirName, Player.FORMAT_DIRECTORY, CultureInfo.CurrentCulture, DateTimeStyles.None, out backupDate))
                {
                    string path = $"{ActivePlayer.DirPath}{dirName}/{Player.FILE_NAME}";

                    if (File.Exists(path))
                    {
                        Player p = _LoadPlayer(path, dirName);
                        Backups.Add(p.Key, p);
                    }
                }
            }

            DivaNetUtil.getSortValue2(Backups);

            foreach (PlayerCombo bc in _BackupCombos)
            {
                bc.Items.Clear();

                foreach (string key in Backups.Keys)
                {
                    Player backup = Backups[key];
                    bc.Items.Add(backup);
                }

                bc.SelectedIndex = 0;
            }
        }

        public readonly static Encoding FILE_ENCODING = Encoding.UTF8;

        /*
         * プレイヤー情報ファイルが存在するかチェック＠プレイ履歴等のチェック用
         */
        public bool isPlayerDataFile()
        {
            //string path = "./" + SettingConst.DATA_DIR_NAME + "/" + accessCode + "/" + Player.FILE_NAME;
            string path = $"{ActivePlayer.DirPath}{Player.FILE_NAME}";

            // プレイヤー情報ファイル確認
            return File.Exists(path);
        }

        /*
         * バックアップ処理
         * ＊ファイル閉じる処理をしてから呼び出す！
         */
        public void CopyBackupFile(bool copySettingFileFlg)
        {
            Dictionary<string, string> target = new Dictionary<string, string>();
            target.Add(SettingConst.FILE_MEMO_DATA, $"./{SettingConst.DATA_DIR_NAME}/{ActivePlayer.accessCode}/{SettingConst.FILE_MEMO_DATA}");
            target.Add(SettingConst.FILE_RECORD_DATA, $"./{SettingConst.DATA_DIR_NAME}/{ActivePlayer.accessCode}/{SettingConst.FILE_RECORD_DATA}");
            target.Add(Player.FILE_NAME, $"./{SettingConst.DATA_DIR_NAME}/{ActivePlayer.accessCode}/{Player.FILE_NAME}");
            target.Add(SettingConst.FILE_RANKING_DATA, $"./{SettingConst.DATA_DIR_NAME}/{ActivePlayer.accessCode}/{SettingConst.FILE_RANKING_DATA}");
            target.Add(SettingConst.FILE_SONG_DATA, $"./{SettingConst.DATA_DIR_NAME}/{ActivePlayer.accessCode}/{SettingConst.FILE_SONG_DATA}");
            target.Add(SettingConst.FILE_URL_SONG_DATA, $"./{SettingConst.DATA_DIR_NAME}/{ActivePlayer.accessCode}/{SettingConst.FILE_URL_SONG_DATA}");
            target.Add(SettingConst.FILE_COLLECTION_CARD_DATA, $"./{SettingConst.DATA_DIR_NAME}/{ActivePlayer.accessCode}/{SettingConst.FILE_COLLECTION_CARD_DATA}");
            target.Add(SettingConst.FILE_MYLIST_DATA, $"./{SettingConst.DATA_DIR_NAME}/{ActivePlayer.accessCode}/{SettingConst.FILE_MYLIST_DATA}");

            string createFolderName = DateTime.Now.ToString(Player.FORMAT_DIRECTORY);
            string destDirectoryPath = $"./{SettingConst.DATA_DIR_NAME}/{ActivePlayer.accessCode}/{createFolderName}";
            Directory.CreateDirectory(destDirectoryPath);

            if (copySettingFileFlg)
            {
                target.Add(SettingConst.FILE_RANK_DATA, $"./{SettingConst.CONF_DIR_NAME}/{SettingConst.FILE_RANK_DATA}");
                target.Add(SettingConst.FILE_TASSEIRITU_RIRON_DATA, $"./{SettingConst.CONF_DIR_NAME}/{SettingConst.FILE_TASSEIRITU_RIRON_DATA}");
            }

            foreach (string fileName in target.Keys)
            {
                string sourceFilePath = target[fileName];

                if (File.Exists(sourceFilePath))
                {
                    File.Copy(sourceFilePath, $"{destDirectoryPath}/{fileName}");
                }
            }
        }
    }
}
