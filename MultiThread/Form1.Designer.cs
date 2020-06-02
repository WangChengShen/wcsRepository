namespace MultiThread
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gb1 = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lbBlue = new System.Windows.Forms.Label();
            this.lbRed6 = new System.Windows.Forms.Label();
            this.lbRed5 = new System.Windows.Forms.Label();
            this.lbRed4 = new System.Windows.Forms.Label();
            this.lbRed3 = new System.Windows.Forms.Label();
            this.lbRed2 = new System.Windows.Forms.Label();
            this.lbRed1 = new System.Windows.Forms.Label();
            this.gb1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb1
            // 
            this.gb1.Controls.Add(this.btnStop);
            this.gb1.Controls.Add(this.btnStart);
            this.gb1.Controls.Add(this.lbBlue);
            this.gb1.Controls.Add(this.lbRed6);
            this.gb1.Controls.Add(this.lbRed5);
            this.gb1.Controls.Add(this.lbRed4);
            this.gb1.Controls.Add(this.lbRed3);
            this.gb1.Controls.Add(this.lbRed2);
            this.gb1.Controls.Add(this.lbRed1);
            this.gb1.Location = new System.Drawing.Point(57, 44);
            this.gb1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gb1.Name = "gb1";
            this.gb1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gb1.Size = new System.Drawing.Size(498, 284);
            this.gb1.TabIndex = 0;
            this.gb1.TabStop = false;
            this.gb1.Text = "双色球号码";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(236, 170);
            this.btnStop.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(103, 30);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "结束";
            this.btnStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(71, 170);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(103, 30);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "开始";
            this.btnStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lbBlue
            // 
            this.lbBlue.AutoSize = true;
            this.lbBlue.Font = new System.Drawing.Font("宋体", 26.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBlue.ForeColor = System.Drawing.Color.Blue;
            this.lbBlue.Location = new System.Drawing.Point(320, 87);
            this.lbBlue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbBlue.Name = "lbBlue";
            this.lbBlue.Size = new System.Drawing.Size(53, 35);
            this.lbBlue.TabIndex = 0;
            this.lbBlue.Text = "00";
            this.lbBlue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbRed6
            // 
            this.lbRed6.AutoSize = true;
            this.lbRed6.Font = new System.Drawing.Font("宋体", 26.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbRed6.ForeColor = System.Drawing.Color.Red;
            this.lbRed6.Location = new System.Drawing.Point(260, 87);
            this.lbRed6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbRed6.Name = "lbRed6";
            this.lbRed6.Size = new System.Drawing.Size(53, 35);
            this.lbRed6.TabIndex = 0;
            this.lbRed6.Text = "00";
            this.lbRed6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbRed5
            // 
            this.lbRed5.AutoSize = true;
            this.lbRed5.Font = new System.Drawing.Font("宋体", 26.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbRed5.ForeColor = System.Drawing.Color.Red;
            this.lbRed5.Location = new System.Drawing.Point(209, 87);
            this.lbRed5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbRed5.Name = "lbRed5";
            this.lbRed5.Size = new System.Drawing.Size(53, 35);
            this.lbRed5.TabIndex = 0;
            this.lbRed5.Text = "00";
            this.lbRed5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbRed4
            // 
            this.lbRed4.AutoSize = true;
            this.lbRed4.Font = new System.Drawing.Font("宋体", 26.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbRed4.ForeColor = System.Drawing.Color.Red;
            this.lbRed4.Location = new System.Drawing.Point(160, 87);
            this.lbRed4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbRed4.Name = "lbRed4";
            this.lbRed4.Size = new System.Drawing.Size(53, 35);
            this.lbRed4.TabIndex = 0;
            this.lbRed4.Text = "00";
            this.lbRed4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbRed3
            // 
            this.lbRed3.AutoSize = true;
            this.lbRed3.Font = new System.Drawing.Font("宋体", 26.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbRed3.ForeColor = System.Drawing.Color.Red;
            this.lbRed3.Location = new System.Drawing.Point(113, 87);
            this.lbRed3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbRed3.Name = "lbRed3";
            this.lbRed3.Size = new System.Drawing.Size(53, 35);
            this.lbRed3.TabIndex = 0;
            this.lbRed3.Text = "00";
            this.lbRed3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbRed2
            // 
            this.lbRed2.AutoSize = true;
            this.lbRed2.Font = new System.Drawing.Font("宋体", 26.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbRed2.ForeColor = System.Drawing.Color.Red;
            this.lbRed2.Location = new System.Drawing.Point(65, 87);
            this.lbRed2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbRed2.Name = "lbRed2";
            this.lbRed2.Size = new System.Drawing.Size(53, 35);
            this.lbRed2.TabIndex = 0;
            this.lbRed2.Text = "00";
            this.lbRed2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbRed1
            // 
            this.lbRed1.AutoSize = true;
            this.lbRed1.Font = new System.Drawing.Font("宋体", 26.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbRed1.ForeColor = System.Drawing.Color.Red;
            this.lbRed1.Location = new System.Drawing.Point(17, 87);
            this.lbRed1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbRed1.Name = "lbRed1";
            this.lbRed1.Size = new System.Drawing.Size(53, 35);
            this.lbRed1.TabIndex = 0;
            this.lbRed1.Text = "00";
            this.lbRed1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 389);
            this.Controls.Add(this.gb1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.gb1.ResumeLayout(false);
            this.gb1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lbBlue;
        private System.Windows.Forms.Label lbRed6;
        private System.Windows.Forms.Label lbRed5;
        private System.Windows.Forms.Label lbRed4;
        private System.Windows.Forms.Label lbRed3;
        private System.Windows.Forms.Label lbRed2;
        private System.Windows.Forms.Label lbRed1;
    }
}

