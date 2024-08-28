using System;

namespace DivaNetAccess.src.util
{
    public static class MessageConst
    {
        // エラーメッセージ
        public static readonly String E_MSG_ERROR_T = "エラー";
        public static readonly String N_MSG_FINISH_T = "完了";
        public static readonly String I_MSG_INFO_T = "確認";
        public static readonly String N_MSG_FINISH_AFTER = "\n--------------------------------------------------------------------------------------\n※ツールへ情報を反映させるには、情報を再取得してください。";

        public static readonly String E_MSG_0001 = "アクセスコードまたはパスワードが未入力です。";
        public static readonly String E_MSG_0002 = "ログインに失敗しました。\nアクセスコードとパスワードを確認してください。";
        public static readonly String E_MSG_0003 = "楽曲の取得に失敗しました。";
        public static readonly String E_MSG_0004 = "楽曲情報がありません。\nDIVA.NETから取得してください。";
        public static readonly String E_MSG_0006 = "楽曲の読み込みに失敗しました。";
        public static readonly String E_MSG_0007 = "DIVA.NETの利用権がありません。\n利用権を取得してから実行してください。";
        public static readonly String E_MSG_0008 = "プレイ履歴がありません。\nプレイヤーを選択するか、プレイ履歴を取得してください。";
        public static readonly String E_MSG_0009 = "プレイ履歴を選択してください。";
        public static readonly String E_MSG_0010 = "URL情報がありません。\nDIVA.NETから取得してください。";
        public static readonly String E_MSG_0011 = "設定する値を選択してください。";
        public static readonly String E_MSG_0012 = "楽曲別設定がありません。\nプレイヤーを選択するか、楽曲別設定を取得してください。";
        public static readonly String E_MSG_0013 = "この楽曲は「衣装チェンジ」対象曲です。\n同じキャラクターを設定してください。";
        public static readonly String E_MSG_0014 = "ライバルコードが未入力です。";
        public static readonly String E_MSG_0015 = "プレイヤーを選択してください。";

        public static readonly String N_MSG_0001 = "DIVA.NETから最新データを取得します。\nこの作業は数分かかる場合があります。";
        public static readonly String N_MSG_0002 = "DIVA.NETから楽曲情報を取得しました。";
        public static readonly String N_MSG_0003 = "PDA Wikiから達成率理論値を取得しました。";
        public static readonly String N_MSG_0004 = "DIVA.NETからプレイ履歴を取得しました。";
        public static readonly String N_MSG_0005 = "DIVA.NETからURLを取得しました。";
        public static readonly String N_MSG_0006 = "DIVA.NETから楽曲別設定を取得しました。";
        public static readonly String N_MSG_0007 = "楽曲別設定を設定しました。";
        public static readonly String N_MSG_0008 = "DIVA.NETからマイリスト情報を取得しました。\n\n※マイリストは検索の条件として指定できます。";
        public static readonly String N_MSG_0009 = "DIVA.NETからコレクションカード情報を取得しました。";
        public static readonly String N_MSG_0010 = "バックアップの作成が完了しました。";

        public static readonly String I_MSG_0001 = "選択したプレイ履歴を削除します。\nよろしいですか？";
        public static readonly String I_MSG_0002 = "楽曲情報を取得します。";
        public static readonly String I_MSG_0003 = "URL情報を取得します。\nよろしいですか？\n\n※この処理は非常に時間がかかります。";
        public static readonly String I_MSG_0004 = "表示中のプレイヤーのバックアップを作成します。\nよろしいですか？";

        public static readonly String E_MSG_9000 = "予期せぬエラーが発生しました。\n\nログファイルを確認してください。";
    }
}
