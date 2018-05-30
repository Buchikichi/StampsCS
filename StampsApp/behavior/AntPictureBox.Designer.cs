namespace StampsApp.behavior
{
    partial class AntPictureBox
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // AntPictureBox
            // 
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.AntPictureBox_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AntPictureBox_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AntPictureBox_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AntPictureBox_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
