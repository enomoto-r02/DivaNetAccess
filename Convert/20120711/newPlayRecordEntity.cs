using System;
using System.Text;

namespace Convert._20120711
{
    // �v���C�����N���X
    public class newPlayRecordEntity
    {
        public enum Index
        {
            DATE = 0,
            PLACE,
            NAME,
            DIFF,
            STAR,
            CLEAR,
            TASSEIRITU,
            SCORE,
            COOL,
            COOLP,
            FINE,
            FINEP,
            SAFE,
            SAFEP,
            SAD,
            SADP,
            WORST,
            WORSTP,
            COMBO,
            CHALLENGE,
            HOLD,
            TRIAL,
            MODULE1,
            MODULE2,
            BUTTON,
            SKIN,
            MEMO
        }

        // ��؂蕶��
        private readonly string SEPALATOR = "\t";

        // ���t����
        private readonly string DATE_FORMAT = "yyyy/MM/dd HH:mm:ss";

        // �폜�t���O
        public bool delFlg { get; set; }

        // ����
        public DateTime date { get; set; }

        // �ꏊ
        public string place { get; set; }

        // �Ȗ�
        public string name { get; set; }

        // ��Փx
        public string diff { get; set; }

        // ��
        public int star { get; set; }

        // CLEAR
        public string clear { get; set; }

        // �B����
        public int tasseiritu { get; set; }

        // �X�R�A
        public int score { get; set; }

        // COOL
        public int cool { get; set; }

        // COOL��
        public int coolP { get; set; }

        // FINE
        public int fine { get; set; }

        // FINE��
        public int fineP { get; set; }

        // SAFE
        public int safe { get; set; }

        // SAFE
        public int safeP { get; set; }

        // SAD
        public int sad { get; set; }

        // SAD��
        public int sadP { get; set; }

        // WORST/WRONG
        public int worst { get; set; }

        // WORST/WRONG��
        public int worstP { get; set; }

        // COMBO
        public int combo { get; set; }

        // CHALLENGE TIME
        public int challenge { get; set; }

        // ��������/�z�[���h
        public int hold { get; set; }

        // �g���C�A��
        public string trial { get; set; }

        // ���W���[��1
        public string module1 { get; set; }

        // ���W���[��2
        public string module2 { get; set; }

        // �{�^����
        public string button { get; set; }

        // �X�L��
        public string skin { get; set; }

        // ����
        public string memo { get; set; }

        // Dictionary�p�L�[
        public string key { get; set; }

        /*
         * �R���X�g���N�^
         */
        public newPlayRecordEntity()
        {
        }

        /*
         * �t�@�C���ǂݍ��ݗp
         */
        public newPlayRecordEntity(string line)
        {
            string[] data = line.Split(char.Parse(SEPALATOR));

            delFlg = false;

            date = DateTime.Parse(data[(int)Index.DATE]);
            place = data[(int)Index.PLACE];
            name = data[(int)Index.NAME];
            diff = data[(int)Index.DIFF];
            star = int.Parse(data[(int)Index.STAR]);
            clear = data[(int)Index.CLEAR];
            tasseiritu = int.Parse(data[(int)Index.TASSEIRITU]);
            score = int.Parse(data[(int)Index.SCORE]);
            cool = int.Parse(data[(int)Index.COOL]);
            coolP = int.Parse(data[(int)Index.COOLP]);
            fine = int.Parse(data[(int)Index.FINE]);
            fineP = int.Parse(data[(int)Index.FINEP]);
            safe = int.Parse(data[(int)Index.SAFE]);
            safeP = int.Parse(data[(int)Index.SAFEP]);
            sad = int.Parse(data[(int)Index.SAD]);
            sadP = int.Parse(data[(int)Index.SADP]);
            worst = int.Parse(data[(int)Index.WORST]);
            worstP = int.Parse(data[(int)Index.WORSTP]);
            combo = int.Parse(data[(int)Index.COMBO]);
            challenge = int.Parse(data[(int)Index.CHALLENGE]);
            hold = int.Parse(data[(int)Index.HOLD]);
            trial = data[(int)Index.TRIAL];
            module1 = data[(int)Index.MODULE1];
            module2 = data[(int)Index.MODULE2];
            button = data[(int)Index.BUTTON];
            skin = data[(int)Index.SKIN];
            memo = data[(int)Index.MEMO];

            // �L�[����
            makeKey();
        }

        /*
         * �t�@�C���������ݗp
         */
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(date.ToString(DATE_FORMAT) + SEPALATOR);
            sb.Append(place + SEPALATOR);
            sb.Append(name + SEPALATOR);
            sb.Append(diff.ToString() + SEPALATOR);
            sb.Append(star.ToString() + SEPALATOR);
            sb.Append(clear + SEPALATOR);
            sb.Append(tasseiritu.ToString() + SEPALATOR);
            sb.Append(score.ToString() + SEPALATOR);
            sb.Append(cool.ToString() + SEPALATOR);
            sb.Append(coolP.ToString() + SEPALATOR);
            sb.Append(fine.ToString() + SEPALATOR);
            sb.Append(fineP.ToString() + SEPALATOR);
            sb.Append(safe.ToString() + SEPALATOR);
            sb.Append(safeP.ToString() + SEPALATOR);
            sb.Append(sad.ToString() + SEPALATOR);
            sb.Append(sadP.ToString() + SEPALATOR);
            sb.Append(worst.ToString() + SEPALATOR);
            sb.Append(worstP.ToString() + SEPALATOR);
            sb.Append(combo.ToString() + SEPALATOR);
            sb.Append(challenge.ToString() + SEPALATOR);
            sb.Append(hold.ToString() + SEPALATOR);
            sb.Append(trial + SEPALATOR);
            sb.Append(module1 + SEPALATOR);
            sb.Append(module2 + SEPALATOR);
            sb.Append(button + SEPALATOR);
            sb.Append(skin + SEPALATOR);
            sb.Append(memo);

            sb.Append("\n");

            return sb.ToString();
        }

        /*
         * �L�[����
         */
        public void makeKey()
        {
            // �B�����͐؂�̂ē��������Ėʓ|�Ȃ̂Œf�O�B�B
            key = date + name + diff + score;
        }
    }
}
