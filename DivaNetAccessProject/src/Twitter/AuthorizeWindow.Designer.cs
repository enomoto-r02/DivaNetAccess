namespace DivaNetAccess.src.twitter
{
    partial class AuthorizeWindow
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
            this.label1 = new System.Windows.Forms.Label();
            this.tboxPinCode = new System.Windows.Forms.TextBox();
            this.btnAuthentication = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "PIN";
            // 
            // tboxPinCode
            // 
            this.tboxPinCode.Location = new System.Drawing.Point(77, 28);
            this.tboxPinCode.MaxLength = 7;
            this.tboxPinCode.Name = "tboxPinCode";
            this.tboxPinCode.Size = new System.Drawing.Size(140, 19);
            this.tboxPinCode.TabIndex = 5;
            // 
            // btnAuthentication
            // 
            this.btnAuthentication.Location = new System.Drawing.Point(28, 76);
            this.btnAuthentication.Name = "btnAuthentication";
            this.btnAuthentication.Size = new System.Drawing.Size(94, 35);
            this.btnAuthentication.TabIndex = 7;
            this.btnAuthentication.Text = "決定";
            this.btnAuthentication.UseVisualStyleBackColor = true;
            this.btnAuthentication.Click += new System.EventHandler(this.btnAuthentication_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(179, 76);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 35);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AuthorizeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 123);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAuthentication);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tboxPinCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AuthorizeWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Twitter連動";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tboxPinCode;
        private System.Windows.Forms.Button btnAuthentication;
        private System.Windows.Forms.Button btnCancel;

    }
}