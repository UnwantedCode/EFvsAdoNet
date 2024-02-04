namespace EFvsAdoNet
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            pobierzEF = new Button();
            pobierzAN = new Button();
            numberInput = new NumericUpDown();
            btnSaveAdo = new Button();
            btnSaveEF = new Button();
            btnUpdateEF = new Button();
            btnUpdateAdo = new Button();
            btnDeleteEF = new Button();
            btnDeleteAdo = new Button();
            btnReset = new Button();
            labelOutput = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numberInput).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(25, 21);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(906, 395);
            dataGridView1.TabIndex = 0;
            // 
            // pobierzEF
            // 
            pobierzEF.Location = new Point(999, 49);
            pobierzEF.Name = "pobierzEF";
            pobierzEF.Size = new Size(118, 23);
            pobierzEF.TabIndex = 1;
            pobierzEF.Text = "Pobierz EF";
            pobierzEF.UseVisualStyleBackColor = true;
            pobierzEF.Click += pobierzEF_Click;
            // 
            // pobierzAN
            // 
            pobierzAN.Location = new Point(1242, 49);
            pobierzAN.Name = "pobierzAN";
            pobierzAN.Size = new Size(131, 23);
            pobierzAN.TabIndex = 2;
            pobierzAN.Text = "Pobierz AdoNet";
            pobierzAN.UseVisualStyleBackColor = true;
            pobierzAN.Click += pobierzAN_Click;
            // 
            // numberInput
            // 
            numberInput.Location = new Point(999, 100);
            numberInput.Name = "numberInput";
            numberInput.Size = new Size(374, 23);
            numberInput.TabIndex = 3;
            numberInput.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // btnSaveAdo
            // 
            btnSaveAdo.Location = new Point(1242, 181);
            btnSaveAdo.Name = "btnSaveAdo";
            btnSaveAdo.Size = new Size(131, 23);
            btnSaveAdo.TabIndex = 4;
            btnSaveAdo.Text = "Generuj AdoNet";
            btnSaveAdo.UseVisualStyleBackColor = true;
            btnSaveAdo.Click += btnSaveAdo_Click;
            // 
            // btnSaveEF
            // 
            btnSaveEF.Location = new Point(999, 181);
            btnSaveEF.Name = "btnSaveEF";
            btnSaveEF.Size = new Size(118, 23);
            btnSaveEF.TabIndex = 5;
            btnSaveEF.Text = "Generuj EF";
            btnSaveEF.UseVisualStyleBackColor = true;
            btnSaveEF.Click += btnSaveEF_Click;
            // 
            // btnUpdateEF
            // 
            btnUpdateEF.Location = new Point(999, 244);
            btnUpdateEF.Name = "btnUpdateEF";
            btnUpdateEF.Size = new Size(118, 23);
            btnUpdateEF.TabIndex = 6;
            btnUpdateEF.Text = "Aktualizuj EF";
            btnUpdateEF.UseVisualStyleBackColor = true;
            btnUpdateEF.Click += btnUpdateEF_Click;
            // 
            // btnUpdateAdo
            // 
            btnUpdateAdo.Location = new Point(1242, 244);
            btnUpdateAdo.Name = "btnUpdateAdo";
            btnUpdateAdo.Size = new Size(131, 23);
            btnUpdateAdo.TabIndex = 7;
            btnUpdateAdo.Text = "Aktualizuj AdoNet";
            btnUpdateAdo.UseVisualStyleBackColor = true;
            btnUpdateAdo.Click += btnUpdateAdo_Click;
            // 
            // btnDeleteEF
            // 
            btnDeleteEF.Location = new Point(999, 308);
            btnDeleteEF.Name = "btnDeleteEF";
            btnDeleteEF.Size = new Size(118, 23);
            btnDeleteEF.TabIndex = 8;
            btnDeleteEF.Text = "Usuń EF";
            btnDeleteEF.UseVisualStyleBackColor = true;
            btnDeleteEF.Click += btnDeleteEF_Click;
            // 
            // btnDeleteAdo
            // 
            btnDeleteAdo.Location = new Point(1242, 308);
            btnDeleteAdo.Name = "btnDeleteAdo";
            btnDeleteAdo.Size = new Size(131, 23);
            btnDeleteAdo.TabIndex = 9;
            btnDeleteAdo.Text = "Usuń AdoNet";
            btnDeleteAdo.UseVisualStyleBackColor = true;
            btnDeleteAdo.Click += btnDeleteAdo_Click;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(999, 387);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(374, 23);
            btnReset.TabIndex = 10;
            btnReset.Text = "Resetowanie danych";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // labelOutput
            // 
            labelOutput.AutoSize = true;
            labelOutput.Location = new Point(999, 356);
            labelOutput.Name = "labelOutput";
            labelOutput.Size = new Size(0, 15);
            labelOutput.TabIndex = 11;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1415, 450);
            Controls.Add(labelOutput);
            Controls.Add(btnReset);
            Controls.Add(btnDeleteAdo);
            Controls.Add(btnDeleteEF);
            Controls.Add(btnUpdateAdo);
            Controls.Add(btnUpdateEF);
            Controls.Add(btnSaveEF);
            Controls.Add(btnSaveAdo);
            Controls.Add(numberInput);
            Controls.Add(pobierzAN);
            Controls.Add(pobierzEF);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numberInput).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Button pobierzEF;
        private Button pobierzAN;
        private NumericUpDown numberInput;
        private Button btnSaveAdo;
        private Button btnSaveEF;
        private Button btnUpdateEF;
        private Button btnUpdateAdo;
        private Button btnDeleteEF;
        private Button btnDeleteAdo;
        private Button btnReset;
        private Label labelOutput;
    }
}
