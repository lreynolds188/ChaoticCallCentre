namespace ChaoticCallCentre
{
    partial class ChangeCarrierName
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtCarrierName = new System.Windows.Forms.TextBox();
            this.btnUpdateCarrier = new System.Windows.Forms.Button();
            this.lblHeading = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(151, 74);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(109, 29);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtCarrierName
            // 
            this.txtCarrierName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCarrierName.Location = new System.Drawing.Point(65, 37);
            this.txtCarrierName.Margin = new System.Windows.Forms.Padding(2);
            this.txtCarrierName.Name = "txtCarrierName";
            this.txtCarrierName.Size = new System.Drawing.Size(246, 26);
            this.txtCarrierName.TabIndex = 6;
            // 
            // btnUpdateCarrier
            // 
            this.btnUpdateCarrier.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateCarrier.Location = new System.Drawing.Point(264, 74);
            this.btnUpdateCarrier.Margin = new System.Windows.Forms.Padding(2);
            this.btnUpdateCarrier.Name = "btnUpdateCarrier";
            this.btnUpdateCarrier.Size = new System.Drawing.Size(112, 29);
            this.btnUpdateCarrier.TabIndex = 5;
            this.btnUpdateCarrier.Text = "Update Carrier";
            this.btnUpdateCarrier.UseVisualStyleBackColor = true;
            this.btnUpdateCarrier.Click += new System.EventHandler(this.btnUpdateCarrier_Click);
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.Location = new System.Drawing.Point(6, 7);
            this.lblHeading.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(76, 18);
            this.lblHeading.TabIndex = 4;
            this.lblHeading.Text = "lblHeading";
            // 
            // ChangeCarrierName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 111);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtCarrierName);
            this.Controls.Add(this.btnUpdateCarrier);
            this.Controls.Add(this.lblHeading);
            this.Name = "ChangeCarrierName";
            this.Text = "ChangeCarrierName";
            this.Load += new System.EventHandler(this.ChangeCarrierName_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtCarrierName;
        private System.Windows.Forms.Button btnUpdateCarrier;
        private System.Windows.Forms.Label lblHeading;

    }
}