using Convert.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Convert._20120711
{
    public static class Conv_20120711
    {
        /*
         * ���C������
         */
        public static void convMain(string[] args)
        {
            // �Ώۃt�@�C���擾
            Dictionary<string, string> files = ConvertCommonUtil.getDataFile(SettingConst.FILE_RECORD_DATA);

            // �t�@�C���Ȃ�
            if (files == null)
            {
                Console.WriteLine("�R���o�[�g�Ώۃt�@�C��������܂���B");
                Console.ReadKey();

                return;
            }

            // �o�b�N�A�b�v
            ConvertCommonUtil.backupFiles(files);

            // �R���o�[�g
            List<SortedDictionary<string, newPlayRecordEntity>> newRecordsAllUser = convert(files);

            // �㏑��
            writePlayRecordData(files, newRecordsAllUser);

            Console.WriteLine("�R���o�[�g���������܂����B");
            Console.ReadKey();

            return;
        }

        /*
         * �R���o�[�g
         */
        public static List<SortedDictionary<string, newPlayRecordEntity>> convert(Dictionary<string, string> files)
        {
            // �S�v���C���[�̋��v���C�������X�g�ǂݍ���
            List<SortedDictionary<string, oldPlayRecordEntity>> oldRecordsAllUser = new List<SortedDictionary<string, oldPlayRecordEntity>>();

            foreach (string accessCode in files.Keys)
            {
                oldRecordsAllUser.Add(readOldPlayRecordData(files[accessCode]));
            }

            // �S�v���C���[�̐V�v���C�������X�g�ɕϊ�
            List<SortedDictionary<string, newPlayRecordEntity>> newRecordsAllUser = new List<SortedDictionary<string, newPlayRecordEntity>>();

            foreach (SortedDictionary<string, oldPlayRecordEntity> oldRecords in oldRecordsAllUser)
            {
                SortedDictionary<string, newPlayRecordEntity> newRerocrds = new SortedDictionary<string, newPlayRecordEntity>();

                foreach (string key in oldRecords.Keys)
                {
                    oldPlayRecordEntity oldRecord = oldRecords[key];
                    newPlayRecordEntity newRecord = new newPlayRecordEntity();

                    // ���ڃZ�b�g
                    newRecord.date = DateTime.Parse(oldRecord.date);
                    newRecord.place = oldRecord.place;
                    newRecord.name = oldRecord.name;
                    newRecord.diff = oldRecord.diff.Split('�@')[0];
                    newRecord.star = int.Parse(oldRecord.diff.Split('�@')[1].Replace("��", ""));
                    newRecord.clear = oldRecord.clear.Trim();
                    newRecord.tasseiritu = convTasseiritu(oldRecord.tasseiritu);
                    newRecord.score = int.Parse(oldRecord.score);
                    newRecord.cool = int.Parse(oldRecord.cool.Split('/')[0]);
                    newRecord.coolP = convTasseiritu(oldRecord.cool.Split('/')[1]);
                    newRecord.fine = int.Parse(oldRecord.fine.Split('/')[0]);
                    newRecord.fineP = convTasseiritu(oldRecord.fine.Split('/')[1]);
                    newRecord.safe = int.Parse(oldRecord.safe.Split('/')[0]);
                    newRecord.safeP = convTasseiritu(oldRecord.safe.Split('/')[1]);
                    newRecord.sad = int.Parse(oldRecord.sad.Split('/')[0]);
                    newRecord.sadP = convTasseiritu(oldRecord.sad.Split('/')[1]);
                    newRecord.worst = int.Parse(oldRecord.worst.Split('/')[0]);
                    newRecord.worstP = convTasseiritu(oldRecord.worst.Split('/')[1]);
                    newRecord.combo = int.Parse(oldRecord.combo);
                    newRecord.challenge = int.Parse(oldRecord.challenge);
                    newRecord.hold = int.Parse(oldRecord.hold);
                    newRecord.trial = oldRecord.trial;
                    newRecord.module1 = oldRecord.module1;
                    newRecord.module2 = oldRecord.module2;
                    newRecord.button = oldRecord.button;
                    newRecord.skin = oldRecord.skin;
                    newRecord.memo = "";

                    // �L�[����
                    newRecord.makeKey();
                    newRerocrds.Add(newRecord.key, newRecord);
                }

                newRecordsAllUser.Add(newRerocrds);
            }

            return newRecordsAllUser;
        }



        /*
         * �v���C����ǂݍ���
         */
        public static SortedDictionary<string, oldPlayRecordEntity> readOldPlayRecordData(string path)
        {
            // �v���C���[��񐶐�
            SortedDictionary<string, oldPlayRecordEntity> ret = new SortedDictionary<string, oldPlayRecordEntity>();

            // �v���C���[���t�@�C���m�F
            if (File.Exists(path) == false)
            {
                return null;
            }

            // �t�@�C�����J��
            using (StreamReader sr = new StreamReader(
                path,
                SettingConst.FILE_ENCODING
            ))
            {
                string line;

                // �t�@�C���̖����܂�
                while ((line = sr.ReadLine()) != null)
                {
                    oldPlayRecordEntity record = new oldPlayRecordEntity(line);

                    // ���X�g�ɒǉ�
                    ret.Add(record.key, record);
                }
            }

            return ret;
        }

        /*
         * �v���C������������
         */
        public static void writePlayRecordData(Dictionary<string, string> files, List<SortedDictionary<string, newPlayRecordEntity>> newRecordsAllUser)
        {
            int i = 0;

            foreach (string accessCode in files.Keys)
            {
                string path = files[accessCode];

                // 1�v���C���[�p�o�b�t�@
                StringBuilder buf = new StringBuilder();

                SortedDictionary<string, newPlayRecordEntity> newRecords = newRecordsAllUser[i];
                foreach (string key in newRecords.Keys)
                {
                    buf.Append(newRecords[key].ToString());
                }

                // �v���C���[��񏑂�����
                FileUtil.writeFile(
                    buf.ToString(),
                    path,
                    false
                );

                i++;
            }
        }

        /*
         * �B�����ϊ�(XX.XX%)��(XXXX)
         */
        public static int convTasseiritu(string tasseiritu)
        {
            string[] tmpArray = tasseiritu.Replace("��", "").Replace("%", "").Split('.');
            return int.Parse(int.Parse(tmpArray[0]) + tmpArray[1].PadRight(2, '0'));
        }
    }
}
