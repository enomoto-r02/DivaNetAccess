namespace DivaNetAccess.src.PlayRecordToukeiCntView
{
    partial class PlayRecordToukeiCntViewWindow
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
            this.tboxPlayRecordToukeiCntView = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tboxPlayRecordToukeiCntView
            // 
            this.tboxPlayRecordToukeiCntView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxPlayRecordToukeiCntView.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tboxPlayRecordToukeiCntView.Location = new System.Drawing.Point(12, 12);
            this.tboxPlayRecordToukeiCntView.Multiline = true;
            this.tboxPlayRecordToukeiCntView.Name = "tboxPlayRecordToukeiCntView";
            this.tboxPlayRecordToukeiCntView.ReadOnly = true;
            this.tboxPlayRecordToukeiCntView.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tboxPlayRecordToukeiCntView.Size = new System.Drawing.Size(420, 548);
            this.tboxPlayRecordToukeiCntView.TabIndex = 0;
            this.tboxPlayRecordToukeiCntView.WordWrap = false;
            // 
            // playRecordToukeiCntViewWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 572);
            this.Controls.Add(this.tboxPlayRecordToukeiCntView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "playRecordToukeiCntViewWindow";
            this.ShowInTaskbar = false;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tboxPlayRecordToukeiCntView;
    }
}