namespace StampsApp
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
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
            this.components = new System.ComponentModel.Container();
            this.FileListBox = new System.Windows.Forms.ListBox();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.CreateSampleButton = new System.Windows.Forms.Button();
            this.AntPicture = new StampsApp.behavior.AntPictureBox(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AntPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // FileListBox
            // 
            this.FileListBox.AllowDrop = true;
            this.FileListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.FileListBox.FormattingEnabled = true;
            this.FileListBox.ItemHeight = 19;
            this.FileListBox.Location = new System.Drawing.Point(14, 12);
            this.FileListBox.Margin = new System.Windows.Forms.Padding(5);
            this.FileListBox.Name = "FileListBox";
            this.FileListBox.Size = new System.Drawing.Size(166, 498);
            this.FileListBox.TabIndex = 0;
            this.FileListBox.SelectedIndexChanged += new System.EventHandler(this.FileListBox_SelectedIndexChanged);
            this.FileListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.FileListBox_DragDrop);
            this.FileListBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.FileListBox_DragEnter);
            // 
            // PictureBox
            // 
            this.PictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PictureBox.Location = new System.Drawing.Point(188, 12);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(584, 537);
            this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox.TabIndex = 1;
            this.PictureBox.TabStop = false;
            // 
            // CreateSampleButton
            // 
            this.CreateSampleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CreateSampleButton.Location = new System.Drawing.Point(12, 519);
            this.CreateSampleButton.Name = "CreateSampleButton";
            this.CreateSampleButton.Size = new System.Drawing.Size(130, 30);
            this.CreateSampleButton.TabIndex = 3;
            this.CreateSampleButton.Text = "CreateSample";
            this.CreateSampleButton.UseVisualStyleBackColor = true;
            this.CreateSampleButton.Click += new System.EventHandler(this.CreateSampleButton_Click);
            // 
            // AntPicture
            // 
            this.AntPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AntPicture.BackColor = System.Drawing.Color.Transparent;
            this.AntPicture.Location = new System.Drawing.Point(188, 12);
            this.AntPicture.Name = "AntPicture";
            this.AntPicture.Size = new System.Drawing.Size(584, 537);
            this.AntPicture.TabIndex = 2;
            this.AntPicture.TabStop = false;
            this.AntPicture.DoubleClick += new System.EventHandler(this.AntPicture_DoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.CreateSampleButton);
            this.Controls.Add(this.AntPicture);
            this.Controls.Add(this.PictureBox);
            this.Controls.Add(this.FileListBox);
            this.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AntPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox FileListBox;
        private System.Windows.Forms.PictureBox PictureBox;
        private behavior.AntPictureBox AntPicture;
        private System.Windows.Forms.Button CreateSampleButton;
    }
}

