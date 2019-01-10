namespace FGOtest
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
            this.components = new System.ComponentModel.Container();
            this.buttonMulti = new System.Windows.Forms.Button();
            this.testTimes = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonSingle = new System.Windows.Forms.Button();
            this.buttonR = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonMulti
            // 
            this.buttonMulti.Location = new System.Drawing.Point(239, 19);
            this.buttonMulti.Name = "buttonMulti";
            this.buttonMulti.Size = new System.Drawing.Size(117, 44);
            this.buttonMulti.TabIndex = 0;
            this.buttonMulti.Text = "多个回合";
            this.buttonMulti.UseVisualStyleBackColor = true;
            this.buttonMulti.Click += new System.EventHandler(this.buttonMulti_Click);
            // 
            // testTimes
            // 
            this.testTimes.Location = new System.Drawing.Point(270, 99);
            this.testTimes.Name = "testTimes";
            this.testTimes.Size = new System.Drawing.Size(50, 20);
            this.testTimes.TabIndex = 26;
            this.testTimes.Text = "10000";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(252, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 16);
            this.label4.TabIndex = 27;
            this.label4.Text = "测试回合数";
            // 
            // buttonSingle
            // 
            this.buttonSingle.Location = new System.Drawing.Point(116, 19);
            this.buttonSingle.Name = "buttonSingle";
            this.buttonSingle.Size = new System.Drawing.Size(117, 44);
            this.buttonSingle.TabIndex = 34;
            this.buttonSingle.Text = "下一回合";
            this.buttonSingle.UseVisualStyleBackColor = true;
            this.buttonSingle.Click += new System.EventHandler(this.buttonSingle_Click);
            // 
            // buttonR
            // 
            this.buttonR.Location = new System.Drawing.Point(145, 75);
            this.buttonR.Name = "buttonR";
            this.buttonR.Size = new System.Drawing.Size(50, 44);
            this.buttonR.TabIndex = 35;
            this.buttonR.Text = "重置";
            this.buttonR.UseVisualStyleBackColor = true;
            this.buttonR.Click += new System.EventHandler(this.buttonR_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.buttonEnd);
            this.groupBox1.Controls.Add(this.buttonStart);
            this.groupBox1.Controls.Add(this.buttonMulti);
            this.groupBox1.Controls.Add(this.buttonR);
            this.groupBox1.Controls.Add(this.testTimes);
            this.groupBox1.Controls.Add(this.buttonSingle);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(550, 620);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(480, 150);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "模拟";
            // 
            // buttonEnd
            // 
            this.buttonEnd.Location = new System.Drawing.Point(362, 19);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(100, 100);
            this.buttonEnd.TabIndex = 37;
            this.buttonEnd.Text = "结束";
            this.buttonEnd.UseVisualStyleBackColor = true;
            this.buttonEnd.Click += new System.EventHandler(this.buttonEnd_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(10, 19);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(100, 100);
            this.buttonStart.TabIndex = 36;
            this.buttonStart.Text = "开始";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(225, 117);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 38;
            this.button1.Text = "礼装测试";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1062, 782);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "FGOtest v0.5  (by 贴吧ID 给取名字不)";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonMulti;
        private System.Windows.Forms.TextBox testTimes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonSingle;
        private System.Windows.Forms.Button buttonR;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonEnd;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Button button1;
    }
}

