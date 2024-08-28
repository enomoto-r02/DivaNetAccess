namespace DivaNetAccess.src.searchSong
{
    partial class SearchCollectionCardWindow
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtMaisuOver = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMaisuUnder = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.txtGetDateAfter = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtGetDateBefore = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.chkTypeSong = new System.Windows.Forms.CheckBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.chkTypeMiku = new System.Windows.Forms.CheckBox();
            this.chkTypeRin = new System.Windows.Forms.CheckBox();
            this.chkTypeLen = new System.Windows.Forms.CheckBox();
            this.chkTypeLuka = new System.Windows.Forms.CheckBox();
            this.chkTypeMeiko = new System.Windows.Forms.CheckBox();
            this.chkTypeKaito = new System.Windows.Forms.CheckBox();
            this.chkTypeHasei = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCardName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtMaisuOver
            // 
            this.txtMaisuOver.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txtMaisuOver.Location = new System.Drawing.Point(74, 77);
            this.txtMaisuOver.MaxLength = 3;
            this.txtMaisuOver.Name = "txtMaisuOver";
            this.txtMaisuOver.Size = new System.Drawing.Size(57, 19);
            this.txtMaisuOver.TabIndex = 190;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "枚数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(137, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "枚 以上";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(251, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "枚 以下";
            // 
            // txtMaisuUnder
            // 
            this.txtMaisuUnder.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txtMaisuUnder.Location = new System.Drawing.Point(188, 77);
            this.txtMaisuUnder.MaxLength = 3;
            this.txtMaisuUnder.Name = "txtMaisuUnder";
            this.txtMaisuUnder.Size = new System.Drawing.Size(57, 19);
            this.txtMaisuUnder.TabIndex = 200;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(217, 156);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(70, 25);
            this.btnClear.TabIndex = 240;
            this.btnClear.Text = "クリア";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(251, 105);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(29, 12);
            this.label16.TabIndex = 45;
            this.label16.Text = "以降";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtGetDateAfter
            // 
            this.txtGetDateAfter.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txtGetDateAfter.Location = new System.Drawing.Point(188, 102);
            this.txtGetDateAfter.MaxLength = 10;
            this.txtGetDateAfter.Name = "txtGetDateAfter";
            this.txtGetDateAfter.Size = new System.Drawing.Size(57, 19);
            this.txtGetDateAfter.TabIndex = 220;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(137, 105);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(29, 12);
            this.label17.TabIndex = 44;
            this.label17.Text = "以前";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(2, 97);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(29, 12);
            this.label18.TabIndex = 43;
            this.label18.Text = "初回";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtGetDateBefore
            // 
            this.txtGetDateBefore.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txtGetDateBefore.Location = new System.Drawing.Point(74, 102);
            this.txtGetDateBefore.MaxLength = 10;
            this.txtGetDateBefore.Name = "txtGetDateBefore";
            this.txtGetDateBefore.Size = new System.Drawing.Size(57, 19);
            this.txtGetDateBefore.TabIndex = 210;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(96, 156);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 25);
            this.btnClose.TabIndex = 230;
            this.btnClose.Text = "検　索";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkTypeSong
            // 
            this.chkTypeSong.AutoSize = true;
            this.chkTypeSong.Location = new System.Drawing.Point(74, 8);
            this.chkTypeSong.Name = "chkTypeSong";
            this.chkTypeSong.Size = new System.Drawing.Size(48, 16);
            this.chkTypeSong.TabIndex = 100;
            this.chkTypeSong.Text = "楽曲";
            this.chkTypeSong.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(36, 9);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(29, 12);
            this.label22.TabIndex = 88;
            this.label22.Text = "種類";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(286, 105);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(79, 12);
            this.label27.TabIndex = 1041;
            this.label27.Text = "(例：20120707)";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkTypeMiku
            // 
            this.chkTypeMiku.AutoSize = true;
            this.chkTypeMiku.Location = new System.Drawing.Point(143, 8);
            this.chkTypeMiku.Name = "chkTypeMiku";
            this.chkTypeMiku.Size = new System.Drawing.Size(63, 16);
            this.chkTypeMiku.TabIndex = 110;
            this.chkTypeMiku.Text = "初音ミク";
            this.chkTypeMiku.UseVisualStyleBackColor = true;
            // 
            // chkTypeRin
            // 
            this.chkTypeRin.AutoSize = true;
            this.chkTypeRin.Location = new System.Drawing.Point(212, 8);
            this.chkTypeRin.Name = "chkTypeRin";
            this.chkTypeRin.Size = new System.Drawing.Size(64, 16);
            this.chkTypeRin.TabIndex = 120;
            this.chkTypeRin.Text = "鏡音リン";
            this.chkTypeRin.UseVisualStyleBackColor = true;
            // 
            // chkTypeLen
            // 
            this.chkTypeLen.AutoSize = true;
            this.chkTypeLen.Location = new System.Drawing.Point(282, 8);
            this.chkTypeLen.Name = "chkTypeLen";
            this.chkTypeLen.Size = new System.Drawing.Size(66, 16);
            this.chkTypeLen.TabIndex = 130;
            this.chkTypeLen.Text = "鏡音レン";
            this.chkTypeLen.UseVisualStyleBackColor = true;
            // 
            // chkTypeLuka
            // 
            this.chkTypeLuka.AutoSize = true;
            this.chkTypeLuka.Location = new System.Drawing.Point(74, 30);
            this.chkTypeLuka.Name = "chkTypeLuka";
            this.chkTypeLuka.Size = new System.Drawing.Size(67, 16);
            this.chkTypeLuka.TabIndex = 140;
            this.chkTypeLuka.Text = "巡音ルカ";
            this.chkTypeLuka.UseVisualStyleBackColor = true;
            // 
            // chkTypeMeiko
            // 
            this.chkTypeMeiko.AutoSize = true;
            this.chkTypeMeiko.Location = new System.Drawing.Point(143, 30);
            this.chkTypeMeiko.Name = "chkTypeMeiko";
            this.chkTypeMeiko.Size = new System.Drawing.Size(58, 16);
            this.chkTypeMeiko.TabIndex = 150;
            this.chkTypeMeiko.Text = "MEIKO";
            this.chkTypeMeiko.UseVisualStyleBackColor = true;
            // 
            // chkTypeKaito
            // 
            this.chkTypeKaito.AutoSize = true;
            this.chkTypeKaito.Location = new System.Drawing.Point(212, 30);
            this.chkTypeKaito.Name = "chkTypeKaito";
            this.chkTypeKaito.Size = new System.Drawing.Size(57, 16);
            this.chkTypeKaito.TabIndex = 160;
            this.chkTypeKaito.Text = "KAITO";
            this.chkTypeKaito.UseVisualStyleBackColor = true;
            // 
            // chkTypeHasei
            // 
            this.chkTypeHasei.AutoSize = true;
            this.chkTypeHasei.Location = new System.Drawing.Point(282, 30);
            this.chkTypeHasei.Name = "chkTypeHasei";
            this.chkTypeHasei.Size = new System.Drawing.Size(74, 16);
            this.chkTypeHasei.TabIndex = 170;
            this.chkTypeHasei.Text = "派生キャラ";
            this.chkTypeHasei.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1049;
            this.label1.Text = "獲得日時";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 12);
            this.label7.TabIndex = 1050;
            this.label7.Text = "カード名";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCardName
            // 
            this.txtCardName.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtCardName.Location = new System.Drawing.Point(74, 52);
            this.txtCardName.MaxLength = 100;
            this.txtCardName.Name = "txtCardName";
            this.txtCardName.Size = new System.Drawing.Size(171, 19);
            this.txtCardName.TabIndex = 180;
            // 
            // searchCollectionCardWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 193);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtCardName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkTypeHasei);
            this.Controls.Add(this.chkTypeKaito);
            this.Controls.Add(this.chkTypeMeiko);
            this.Controls.Add(this.chkTypeLuka);
            this.Controls.Add(this.chkTypeLen);
            this.Controls.Add(this.chkTypeRin);
            this.Controls.Add(this.chkTypeMiku);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.chkTypeSong);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtGetDateAfter);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.txtGetDateBefore);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMaisuUnder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMaisuOver);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "searchCollectionCardWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "検索条件";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.searchSongWindow_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMaisuOver;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMaisuUnder;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtGetDateAfter;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtGetDateBefore;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chkTypeSong;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.CheckBox chkTypeMiku;
        private System.Windows.Forms.CheckBox chkTypeRin;
        private System.Windows.Forms.CheckBox chkTypeLen;
        private System.Windows.Forms.CheckBox chkTypeLuka;
        private System.Windows.Forms.CheckBox chkTypeMeiko;
        private System.Windows.Forms.CheckBox chkTypeKaito;
        private System.Windows.Forms.CheckBox chkTypeHasei;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCardName;
    }
}