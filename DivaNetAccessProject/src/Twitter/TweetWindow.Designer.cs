namespace DivaNetAccess.src.twitter
{
    partial class TweetWindow
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
            this.tboxPostStr = new System.Windows.Forms.TextBox();
            this.btnTweetPost = new System.Windows.Forms.Button();
            this.labTextLength = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tboxPostStr
            // 
            this.tboxPostStr.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxPostStr.Location = new System.Drawing.Point(12, 24);
            this.tboxPostStr.Multiline = true;
            this.tboxPostStr.Name = "tboxPostStr";
            this.tboxPostStr.Size = new System.Drawing.Size(370, 107);
            this.tboxPostStr.TabIndex = 0;
            this.tboxPostStr.TextChanged += new System.EventHandler(this.tboxPostStr_TextChanged);
            // 
            // btnTweetPost
            // 
            this.btnTweetPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTweetPost.Location = new System.Drawing.Point(295, 137);
            this.btnTweetPost.Name = "btnTweetPost";
            this.btnTweetPost.Size = new System.Drawing.Size(87, 24);
            this.btnTweetPost.TabIndex = 1;
            this.btnTweetPost.Text = "つぶやく";
            this.btnTweetPost.UseVisualStyleBackColor = true;
            this.btnTweetPost.Click += new System.EventHandler(this.btnTweetPost_Click);
            // 
            // labTextLength
            // 
            this.labTextLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labTextLength.AutoSize = true;
            this.labTextLength.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labTextLength.Location = new System.Drawing.Point(354, 9);
            this.labTextLength.Name = "labTextLength";
            this.labTextLength.Size = new System.Drawing.Size(83, 12);
            this.labTextLength.TabIndex = 5;
            this.labTextLength.Text = "labTextLength";
            // 
            // TweetWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 173);
            this.Controls.Add(this.labTextLength);
            this.Controls.Add(this.btnTweetPost);
            this.Controls.Add(this.tboxPostStr);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TweetWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "つぶやく";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tboxPostStr;
        private System.Windows.Forms.Button btnTweetPost;
        private System.Windows.Forms.Label labTextLength;
    }
}