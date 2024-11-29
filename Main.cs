using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ProdutosApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void cadastrar(object sender, EventArgs e)
        {

            int codigo;
            if (!int.TryParse(inputCodigo.Text, out codigo))
            {
                MessageBox.Show("O campo 'Código' deve ser um número válido.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (codigo <= 0)
            {
                MessageBox.Show("O campo 'Código' deve ser maior que zero.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var titulo = inputTitulo.Text;
            if (string.IsNullOrWhiteSpace(titulo))
            {
                MessageBox.Show("O campo 'Título' não pode estar vazio.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                resetarInputs();
                return;
            }

            

            
            Db.CadastrarOuAtualizarProduto(titulo, codigo);
            resetarInputs();

        }

        private void deletar(object sender, EventArgs e)
        {
            int codigo;
            if (!int.TryParse(inputDeletarCodigo.Text, out codigo))
            {
                MessageBox.Show("O campo 'Código' deve ser um número válido.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (codigo <= 0)
            {
                MessageBox.Show("O campo 'Código' deve ser maior que zero.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Db.DeletarProduto(codigo);
            resetarInputs();

        }



        private void resetarInputs()
        {
            inputTitulo.Text = "";
            inputCodigo.Text = "";
            inputDeletarCodigo.Text = "";
        }

      
    }
}