
namespace BluetoothPairTool
{
    partial class Form_RemoteCar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button_Close = new System.Windows.Forms.Button();
            this.button_Back = new System.Windows.Forms.Button();
            this.button_Front = new System.Windows.Forms.Button();
            this.button_Left = new System.Windows.Forms.Button();
            this.button_Right = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.listBox1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.button_Close, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.button_Back, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.button_Front, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.button_Left, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.button_Right, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(524, 426);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 312);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Response";
            // 
            // listBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.listBox1, 2);
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 14;
            this.listBox1.Location = new System.Drawing.Point(177, 243);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(344, 151);
            this.listBox1.TabIndex = 4;
            // 
            // button_Close
            // 
            this.button_Close.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Close.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Close.Location = new System.Drawing.Point(351, 400);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(170, 23);
            this.button_Close.TabIndex = 1;
            this.button_Close.Text = "Close";
            this.button_Close.UseVisualStyleBackColor = true;
            // 
            // button_Back
            // 
            this.button_Back.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Back.Font = new System.Drawing.Font("Consolas", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Back.Location = new System.Drawing.Point(177, 123);
            this.button_Back.Name = "button_Back";
            this.button_Back.Size = new System.Drawing.Size(168, 114);
            this.button_Back.TabIndex = 2;
            this.button_Back.TabStop = false;
            this.button_Back.Text = "🔽";
            this.button_Back.UseVisualStyleBackColor = true;
            // 
            // button_Front
            // 
            this.button_Front.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Front.Font = new System.Drawing.Font("Consolas", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Front.Location = new System.Drawing.Point(177, 3);
            this.button_Front.Name = "button_Front";
            this.button_Front.Size = new System.Drawing.Size(168, 114);
            this.button_Front.TabIndex = 5;
            this.button_Front.TabStop = false;
            this.button_Front.Text = "🔼";
            this.button_Front.UseVisualStyleBackColor = true;
            // 
            // button_Left
            // 
            this.button_Left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Left.Font = new System.Drawing.Font("Consolas", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Left.Location = new System.Drawing.Point(3, 123);
            this.button_Left.Name = "button_Left";
            this.button_Left.Size = new System.Drawing.Size(168, 114);
            this.button_Left.TabIndex = 6;
            this.button_Left.TabStop = false;
            this.button_Left.Text = "◀";
            this.button_Left.UseVisualStyleBackColor = true;
            // 
            // button_Right
            // 
            this.button_Right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Right.Font = new System.Drawing.Font("Consolas", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Right.Location = new System.Drawing.Point(351, 123);
            this.button_Right.Name = "button_Right";
            this.button_Right.Size = new System.Drawing.Size(170, 114);
            this.button_Right.TabIndex = 7;
            this.button_Right.TabStop = false;
            this.button_Right.Text = "▶";
            this.button_Right.UseVisualStyleBackColor = true;
            // 
            // Form_RemoteCar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 426);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Name = "Form_RemoteCar";
            this.Text = "Form_RemoteCar";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_RemoteCar_FormClosing);
            this.Load += new System.EventHandler(this.Form_RemoteCar_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_RemoteCar_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form_RemoteCar_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Form_RemoteCar_PreviewKeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.Button button_Back;
        private System.Windows.Forms.Button button_Front;
        private System.Windows.Forms.Button button_Left;
        private System.Windows.Forms.Button button_Right;
    }
}