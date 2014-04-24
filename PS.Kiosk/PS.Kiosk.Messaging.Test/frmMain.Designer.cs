namespace PS.Kiosk.Messaging
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.txtIso = new System.Windows.Forms.TextBox();
            this.tmPicker = new System.Windows.Forms.Timer(this.components);
            this.btnRemain = new System.Windows.Forms.Button();
            this.btnPurch = new System.Windows.Forms.Button();
            this.btnBilling = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDestPAN = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAmnt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPIN = new System.Windows.Forms.TextBox();
            this.lblPIN = new System.Windows.Forms.Label();
            this.txtBillID = new System.Windows.Forms.TextBox();
            this.txtPayID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBranchCode = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbBill = new System.Windows.Forms.ComboBox();
            this.cmbSrc = new System.Windows.Forms.ComboBox();
            this.cmbDest = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtCVV2 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtPin2 = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSignOn = new System.Windows.Forms.Button();
            this.btnSignOff = new System.Windows.Forms.Button();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.btnReconcil = new System.Windows.Forms.Button();
            this.btnAuth = new System.Windows.Forms.Button();
            this.btnState = new System.Windows.Forms.Button();
            this.btnCharge = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtIso
            // 
            this.txtIso.BackColor = System.Drawing.SystemColors.Info;
            this.txtIso.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIso.Location = new System.Drawing.Point(111, 61);
            this.txtIso.MaxLength = 2;
            this.txtIso.Name = "txtIso";
            this.txtIso.Size = new System.Drawing.Size(242, 20);
            this.txtIso.TabIndex = 0;
            this.txtIso.Text = "6280231410858822=93045061351133300001";
            // 
            // btnRemain
            // 
            this.btnRemain.Location = new System.Drawing.Point(9, 361);
            this.btnRemain.Name = "btnRemain";
            this.btnRemain.Size = new System.Drawing.Size(75, 23);
            this.btnRemain.TabIndex = 11;
            this.btnRemain.Text = "Remain";
            this.btnRemain.UseVisualStyleBackColor = true;
            this.btnRemain.Click += new System.EventHandler(this.btnRemain_Click);
            // 
            // btnPurch
            // 
            this.btnPurch.Location = new System.Drawing.Point(90, 361);
            this.btnPurch.Name = "btnPurch";
            this.btnPurch.Size = new System.Drawing.Size(75, 23);
            this.btnPurch.TabIndex = 12;
            this.btnPurch.Text = "Purchase";
            this.btnPurch.UseVisualStyleBackColor = true;
            this.btnPurch.Click += new System.EventHandler(this.btnPurch_Click);
            // 
            // btnBilling
            // 
            this.btnBilling.Location = new System.Drawing.Point(9, 390);
            this.btnBilling.Name = "btnBilling";
            this.btnBilling.Size = new System.Drawing.Size(75, 23);
            this.btnBilling.TabIndex = 17;
            this.btnBilling.Text = "Bill Pay";
            this.btnBilling.UseVisualStyleBackColor = true;
            this.btnBilling.Click += new System.EventHandler(this.btnBilling_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "IsoTrack";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Source Account";
            // 
            // txtDestPAN
            // 
            this.txtDestPAN.BackColor = System.Drawing.SystemColors.Info;
            this.txtDestPAN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDestPAN.Location = new System.Drawing.Point(111, 163);
            this.txtDestPAN.MaxLength = 19;
            this.txtDestPAN.Name = "txtDestPAN";
            this.txtDestPAN.Size = new System.Drawing.Size(242, 20);
            this.txtDestPAN.TabIndex = 3;
            this.txtDestPAN.Text = "6280231410858814";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Destination Account";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Destination PAN";
            // 
            // txtAmnt
            // 
            this.txtAmnt.BackColor = System.Drawing.SystemColors.Info;
            this.txtAmnt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAmnt.Location = new System.Drawing.Point(438, 61);
            this.txtAmnt.MaxLength = 15;
            this.txtAmnt.Name = "txtAmnt";
            this.txtAmnt.Size = new System.Drawing.Size(242, 20);
            this.txtAmnt.TabIndex = 5;
            this.txtAmnt.Text = "20000";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(389, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Amount";
            // 
            // txtPIN
            // 
            this.txtPIN.BackColor = System.Drawing.SystemColors.Info;
            this.txtPIN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPIN.Location = new System.Drawing.Point(438, 91);
            this.txtPIN.MaxLength = 4;
            this.txtPIN.Name = "txtPIN";
            this.txtPIN.Size = new System.Drawing.Size(242, 20);
            this.txtPIN.TabIndex = 6;
            this.txtPIN.Text = "2152";
            // 
            // lblPIN
            // 
            this.lblPIN.AutoSize = true;
            this.lblPIN.Location = new System.Drawing.Point(388, 95);
            this.lblPIN.Name = "lblPIN";
            this.lblPIN.Size = new System.Drawing.Size(25, 13);
            this.lblPIN.TabIndex = 2;
            this.lblPIN.Text = "PIN";
            // 
            // txtBillID
            // 
            this.txtBillID.BackColor = System.Drawing.SystemColors.Info;
            this.txtBillID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBillID.Location = new System.Drawing.Point(438, 126);
            this.txtBillID.MaxLength = 18;
            this.txtBillID.Name = "txtBillID";
            this.txtBillID.Size = new System.Drawing.Size(242, 20);
            this.txtBillID.TabIndex = 7;
            this.txtBillID.Text = "2222363309151";
            // 
            // txtPayID
            // 
            this.txtPayID.BackColor = System.Drawing.SystemColors.Info;
            this.txtPayID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPayID.Location = new System.Drawing.Point(438, 163);
            this.txtPayID.MaxLength = 18;
            this.txtPayID.Name = "txtPayID";
            this.txtPayID.Size = new System.Drawing.Size(242, 20);
            this.txtPayID.TabIndex = 8;
            this.txtPayID.Text = "1000138";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(388, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "BillID";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(388, 167);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "PayID";
            // 
            // txtBranchCode
            // 
            this.txtBranchCode.BackColor = System.Drawing.SystemColors.Info;
            this.txtBranchCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBranchCode.Location = new System.Drawing.Point(111, 197);
            this.txtBranchCode.MaxLength = 5;
            this.txtBranchCode.Name = "txtBranchCode";
            this.txtBranchCode.Size = new System.Drawing.Size(242, 20);
            this.txtBranchCode.TabIndex = 4;
            this.txtBranchCode.Text = "007";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 200);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Branch Code";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(388, 200);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "BillType";
            // 
            // cmbBill
            // 
            this.cmbBill.BackColor = System.Drawing.SystemColors.Info;
            this.cmbBill.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBill.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBill.FormattingEnabled = true;
            this.cmbBill.Items.AddRange(new object[] {
            "WA آب ",
            "EL برق",
            "GA گاز",
            "TC تلفن ثابت",
            "MC همراه",
            "MN شهرداري",
            "UD غيره"});
            this.cmbBill.Location = new System.Drawing.Point(438, 197);
            this.cmbBill.Name = "cmbBill";
            this.cmbBill.Size = new System.Drawing.Size(242, 21);
            this.cmbBill.TabIndex = 9;
            // 
            // cmbSrc
            // 
            this.cmbSrc.BackColor = System.Drawing.SystemColors.Info;
            this.cmbSrc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSrc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSrc.FormattingEnabled = true;
            this.cmbSrc.Location = new System.Drawing.Point(111, 91);
            this.cmbSrc.Name = "cmbSrc";
            this.cmbSrc.Size = new System.Drawing.Size(242, 21);
            this.cmbSrc.TabIndex = 1;
            // 
            // cmbDest
            // 
            this.cmbDest.BackColor = System.Drawing.SystemColors.Info;
            this.cmbDest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbDest.FormattingEnabled = true;
            this.cmbDest.Location = new System.Drawing.Point(111, 126);
            this.cmbDest.Name = "cmbDest";
            this.cmbDest.Size = new System.Drawing.Size(242, 21);
            this.cmbDest.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 232);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "CVV2";
            // 
            // txtCVV2
            // 
            this.txtCVV2.BackColor = System.Drawing.SystemColors.Info;
            this.txtCVV2.Location = new System.Drawing.Point(111, 229);
            this.txtCVV2.Name = "txtCVV2";
            this.txtCVV2.Size = new System.Drawing.Size(242, 20);
            this.txtCVV2.TabIndex = 23;
            this.txtCVV2.Text = "3858";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(388, 232);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "PIN2";
            // 
            // txtPin2
            // 
            this.txtPin2.BackColor = System.Drawing.SystemColors.Info;
            this.txtPin2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPin2.Location = new System.Drawing.Point(438, 229);
            this.txtPin2.MaxLength = 4;
            this.txtPin2.Name = "txtPin2";
            this.txtPin2.Size = new System.Drawing.Size(242, 20);
            this.txtPin2.TabIndex = 25;
            this.txtPin2.Text = "13580";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(7, 260);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(920, 95);
            this.listBox1.TabIndex = 26;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(338, 361);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 30;
            this.button1.Text = "Clear List";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSignOn
            // 
            this.btnSignOn.Location = new System.Drawing.Point(252, 361);
            this.btnSignOn.Name = "btnSignOn";
            this.btnSignOn.Size = new System.Drawing.Size(75, 23);
            this.btnSignOn.TabIndex = 28;
            this.btnSignOn.Text = "Sign On";
            this.btnSignOn.UseVisualStyleBackColor = true;
            this.btnSignOn.Click += new System.EventHandler(this.btnSignOn_Click);
            // 
            // btnSignOff
            // 
            this.btnSignOff.Location = new System.Drawing.Point(252, 390);
            this.btnSignOff.Name = "btnSignOff";
            this.btnSignOff.Size = new System.Drawing.Size(75, 23);
            this.btnSignOff.TabIndex = 29;
            this.btnSignOff.Text = "Charge";
            this.btnSignOff.UseVisualStyleBackColor = true;
            this.btnSignOff.Click += new System.EventHandler(this.btnSignOff_Click);
            // 
            // btnTransfer
            // 
            this.btnTransfer.Location = new System.Drawing.Point(171, 361);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(75, 23);
            this.btnTransfer.TabIndex = 12;
            this.btnTransfer.Text = "Transfer";
            this.btnTransfer.UseVisualStyleBackColor = true;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // btnReconcil
            // 
            this.btnReconcil.Location = new System.Drawing.Point(338, 386);
            this.btnReconcil.Name = "btnReconcil";
            this.btnReconcil.Size = new System.Drawing.Size(177, 23);
            this.btnReconcil.TabIndex = 28;
            this.btnReconcil.Text = "settle(درخواست واریز)";
            this.btnReconcil.UseVisualStyleBackColor = true;
            this.btnReconcil.Click += new System.EventHandler(this.btnReconcil_Click);
            // 
            // btnAuth
            // 
            this.btnAuth.Location = new System.Drawing.Point(90, 390);
            this.btnAuth.Name = "btnAuth";
            this.btnAuth.Size = new System.Drawing.Size(156, 23);
            this.btnAuth.TabIndex = 12;
            this.btnAuth.Text = "Authorize(بررسی حساب)";
            this.btnAuth.UseVisualStyleBackColor = true;
            this.btnAuth.Click += new System.EventHandler(this.btnAuth_Click);
            // 
            // btnState
            // 
            this.btnState.Location = new System.Drawing.Point(476, 363);
            this.btnState.Name = "btnState";
            this.btnState.Size = new System.Drawing.Size(75, 23);
            this.btnState.TabIndex = 29;
            this.btnState.Text = "Statement";
            this.btnState.UseVisualStyleBackColor = true;
            this.btnState.Click += new System.EventHandler(this.btnState_Click);
            // 
            // btnCharge
            // 
            this.btnCharge.Location = new System.Drawing.Point(604, 360);
            this.btnCharge.Name = "btnCharge";
            this.btnCharge.Size = new System.Drawing.Size(75, 23);
            this.btnCharge.TabIndex = 31;
            this.btnCharge.Text = "خرید شارژ";
            this.btnCharge.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 421);
            this.Controls.Add(this.btnCharge);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnState);
            this.Controls.Add(this.btnSignOff);
            this.Controls.Add(this.btnReconcil);
            this.Controls.Add(this.btnSignOn);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtPin2);
            this.Controls.Add(this.txtCVV2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cmbDest);
            this.Controls.Add(this.cmbSrc);
            this.Controls.Add(this.cmbBill);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblPIN);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPayID);
            this.Controls.Add(this.txtPIN);
            this.Controls.Add(this.txtBillID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAmnt);
            this.Controls.Add(this.txtIso);
            this.Controls.Add(this.txtBranchCode);
            this.Controls.Add(this.txtDestPAN);
            this.Controls.Add(this.btnBilling);
            this.Controls.Add(this.btnAuth);
            this.Controls.Add(this.btnTransfer);
            this.Controls.Add(this.btnPurch);
            this.Controls.Add(this.btnRemain);
            this.Name = "frmMain";
            this.Text = "frmMain";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIso;
        private System.Windows.Forms.Timer tmPicker;
        private System.Windows.Forms.Button btnRemain;
        private System.Windows.Forms.Button btnPurch;
        private System.Windows.Forms.Button btnBilling;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDestPAN;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAmnt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPIN;
        private System.Windows.Forms.Label lblPIN;
        private System.Windows.Forms.TextBox txtBillID;
        private System.Windows.Forms.TextBox txtPayID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBranchCode;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbBill;
        private System.Windows.Forms.ComboBox cmbSrc;
        private System.Windows.Forms.ComboBox cmbDest;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtCVV2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtPin2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSignOn;
        private System.Windows.Forms.Button btnSignOff;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Button btnReconcil;
        private System.Windows.Forms.Button btnAuth;
        private System.Windows.Forms.Button btnState;
        private System.Windows.Forms.Button btnCharge;
    }
}