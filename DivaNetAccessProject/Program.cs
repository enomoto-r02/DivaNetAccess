using DivaNetAccess.src.util;
using DivaNetAccess.src.Util;
using System;
using System.Threading;
using System.Windows.Forms;

namespace DivaNetAccess
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ThreadExceptionイベント・ハンドラを登録する
            Application.ThreadException += new
                ThreadExceptionEventHandler(Application_ThreadException);

            MainForm form = new MainForm();
            Application.Run(form);
        }

        // 未処理例外をキャッチするイベント・ハンドラ
        // （Windowsアプリケーション用）
        // 引用元：http://www.atmarkit.co.jp/fdotnet/dotnettips/320appexception/appexception.html
        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(MessageConst.E_MSG_9000, MessageConst.E_MSG_ERROR_T);
            LogUtil.writeLog(DateTime.Now.ToString() + " " + e.Exception.Message + "\r\n" + e.Exception.StackTrace);

            Application.Exit();
        }

    }
}
