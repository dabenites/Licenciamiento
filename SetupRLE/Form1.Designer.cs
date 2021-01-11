namespace SetupRLE
{
    partial class frm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbp_ValidarUser = new System.Windows.Forms.TabPage();
            this.@__descarga = new System.Windows.Forms.Label();
            this.lbl_descarga = new System.Windows.Forms.Label();
            this.lbl_mensajeCodigo = new System.Windows.Forms.Label();
            this.lbl_mensajeCorreo = new System.Windows.Forms.Label();
            this.pgr_descarga = new System.Windows.Forms.ProgressBar();
            this.btn_Descargar = new System.Windows.Forms.Button();
            this.btn_verificar = new System.Windows.Forms.Button();
            this.txt_codigo = new System.Windows.Forms.TextBox();
            this.lbl_Codigo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Solicitar = new System.Windows.Forms.Button();
            this.txt_correo = new System.Windows.Forms.TextBox();
            this.lbl_correo = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tbp_ValidarUser.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbp_ValidarUser);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(1, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(392, 195);
            this.tabControl1.TabIndex = 0;
            // 
            // tbp_ValidarUser
            // 
            this.tbp_ValidarUser.Controls.Add(this.@__descarga);
            this.tbp_ValidarUser.Controls.Add(this.lbl_descarga);
            this.tbp_ValidarUser.Controls.Add(this.lbl_mensajeCodigo);
            this.tbp_ValidarUser.Controls.Add(this.lbl_mensajeCorreo);
            this.tbp_ValidarUser.Controls.Add(this.pgr_descarga);
            this.tbp_ValidarUser.Controls.Add(this.btn_Descargar);
            this.tbp_ValidarUser.Controls.Add(this.btn_verificar);
            this.tbp_ValidarUser.Controls.Add(this.txt_codigo);
            this.tbp_ValidarUser.Controls.Add(this.lbl_Codigo);
            this.tbp_ValidarUser.Controls.Add(this.label1);
            this.tbp_ValidarUser.Controls.Add(this.btn_Solicitar);
            this.tbp_ValidarUser.Controls.Add(this.txt_correo);
            this.tbp_ValidarUser.Controls.Add(this.lbl_correo);
            this.tbp_ValidarUser.Location = new System.Drawing.Point(4, 22);
            this.tbp_ValidarUser.Name = "tbp_ValidarUser";
            this.tbp_ValidarUser.Padding = new System.Windows.Forms.Padding(3);
            this.tbp_ValidarUser.Size = new System.Drawing.Size(384, 169);
            this.tbp_ValidarUser.TabIndex = 0;
            this.tbp_ValidarUser.Text = "Validacion Usuario";
            this.tbp_ValidarUser.UseVisualStyleBackColor = true;
            // 
            // __descarga
            // 
            this.@__descarga.AutoSize = true;
            this.@__descarga.Location = new System.Drawing.Point(106, 149);
            this.@__descarga.Name = "__descarga";
            this.@__descarga.Size = new System.Drawing.Size(110, 13);
            this.@__descarga.TabIndex = 13;
            this.@__descarga.Text = "__mensajeDesacarga";
            this.@__descarga.Visible = false;
            // 
            // lbl_descarga
            // 
            this.lbl_descarga.AutoSize = true;
            this.lbl_descarga.Location = new System.Drawing.Point(16, 128);
            this.lbl_descarga.Name = "lbl_descarga";
            this.lbl_descarga.Size = new System.Drawing.Size(63, 13);
            this.lbl_descarga.TabIndex = 12;
            this.lbl_descarga.Text = "__descarga";
            this.lbl_descarga.Visible = false;
            // 
            // lbl_mensajeCodigo
            // 
            this.lbl_mensajeCodigo.AutoSize = true;
            this.lbl_mensajeCodigo.Location = new System.Drawing.Point(106, 100);
            this.lbl_mensajeCodigo.Name = "lbl_mensajeCodigo";
            this.lbl_mensajeCodigo.Size = new System.Drawing.Size(51, 13);
            this.lbl_mensajeCodigo.TabIndex = 11;
            this.lbl_mensajeCodigo.Text = "__codigo";
            this.lbl_mensajeCodigo.Visible = false;
            // 
            // lbl_mensajeCorreo
            // 
            this.lbl_mensajeCorreo.AutoSize = true;
            this.lbl_mensajeCorreo.Location = new System.Drawing.Point(106, 55);
            this.lbl_mensajeCorreo.Name = "lbl_mensajeCorreo";
            this.lbl_mensajeCorreo.Size = new System.Drawing.Size(49, 13);
            this.lbl_mensajeCorreo.TabIndex = 10;
            this.lbl_mensajeCorreo.Text = "__correo";
            this.lbl_mensajeCorreo.Visible = false;
            // 
            // pgr_descarga
            // 
            this.pgr_descarga.Location = new System.Drawing.Point(104, 122);
            this.pgr_descarga.Name = "pgr_descarga";
            this.pgr_descarga.Size = new System.Drawing.Size(183, 23);
            this.pgr_descarga.TabIndex = 9;
            // 
            // btn_Descargar
            // 
            this.btn_Descargar.Location = new System.Drawing.Point(293, 123);
            this.btn_Descargar.Name = "btn_Descargar";
            this.btn_Descargar.Size = new System.Drawing.Size(64, 23);
            this.btn_Descargar.TabIndex = 8;
            this.btn_Descargar.Text = "Descargar";
            this.btn_Descargar.UseVisualStyleBackColor = true;
            this.btn_Descargar.Click += new System.EventHandler(this.btn_Descargar_Click);
            // 
            // btn_verificar
            // 
            this.btn_verificar.Location = new System.Drawing.Point(293, 76);
            this.btn_verificar.Name = "btn_verificar";
            this.btn_verificar.Size = new System.Drawing.Size(64, 23);
            this.btn_verificar.TabIndex = 7;
            this.btn_verificar.Text = "Verificar";
            this.btn_verificar.UseVisualStyleBackColor = true;
            this.btn_verificar.Click += new System.EventHandler(this.btn_Ingresar_Click);
            // 
            // txt_codigo
            // 
            this.txt_codigo.Location = new System.Drawing.Point(104, 76);
            this.txt_codigo.Name = "txt_codigo";
            this.txt_codigo.Size = new System.Drawing.Size(183, 20);
            this.txt_codigo.TabIndex = 6;
            // 
            // lbl_Codigo
            // 
            this.lbl_Codigo.AutoSize = true;
            this.lbl_Codigo.Location = new System.Drawing.Point(16, 79);
            this.lbl_Codigo.Name = "lbl_Codigo";
            this.lbl_Codigo.Size = new System.Drawing.Size(84, 13);
            this.lbl_Codigo.TabIndex = 5;
            this.lbl_Codigo.Text = "Ingrese Código :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(350, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "v1.0";
            // 
            // btn_Solicitar
            // 
            this.btn_Solicitar.Location = new System.Drawing.Point(293, 29);
            this.btn_Solicitar.Name = "btn_Solicitar";
            this.btn_Solicitar.Size = new System.Drawing.Size(64, 23);
            this.btn_Solicitar.TabIndex = 3;
            this.btn_Solicitar.Text = "Solicitar";
            this.btn_Solicitar.UseVisualStyleBackColor = true;
            this.btn_Solicitar.Click += new System.EventHandler(this.btn_Solicitar_Click);
            // 
            // txt_correo
            // 
            this.txt_correo.Location = new System.Drawing.Point(104, 31);
            this.txt_correo.Name = "txt_correo";
            this.txt_correo.Size = new System.Drawing.Size(183, 20);
            this.txt_correo.TabIndex = 1;
            // 
            // lbl_correo
            // 
            this.lbl_correo.AutoSize = true;
            this.lbl_correo.Location = new System.Drawing.Point(16, 35);
            this.lbl_correo.Name = "lbl_correo";
            this.lbl_correo.Size = new System.Drawing.Size(82, 13);
            this.lbl_correo.TabIndex = 0;
            this.lbl_correo.Text = "Ingrese Correo :";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(384, 169);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Solicitar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 197);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm";
            this.Text = "RLE-Suite Install";
            this.Load += new System.EventHandler(this.frm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tbp_ValidarUser.ResumeLayout(false);
            this.tbp_ValidarUser.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbp_ValidarUser;
        private System.Windows.Forms.Label lbl_correo;
        private System.Windows.Forms.TextBox txt_correo;
        private System.Windows.Forms.Button btn_Solicitar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_Codigo;
        private System.Windows.Forms.TextBox txt_codigo;
        private System.Windows.Forms.Button btn_verificar;
        private System.Windows.Forms.Button btn_Descargar;
        private System.Windows.Forms.ProgressBar pgr_descarga;
        private System.Windows.Forms.Label lbl_mensajeCorreo;
        private System.Windows.Forms.Label lbl_mensajeCodigo;
        private System.Windows.Forms.Label lbl_descarga;
        private System.Windows.Forms.Label __descarga;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button1;
    }
}

