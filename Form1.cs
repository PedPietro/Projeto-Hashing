using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace apCaminhosEmMarte
{
  public partial class FrmCaminhos : Form
  {
        public FrmCaminhos()
        {
          InitializeComponent();
        }

        IHashing<Cidade> tabelaHash;
        string arquivoAberto; 

        private void Form1_Load(object sender, EventArgs e)
        {

          pbMapa.Paint += new PaintEventHandler(pbMapa_Paint);
        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
          if (tabelaHash == null)
            return;

          Graphics g = e.Graphics;

          Brush pincel = Brushes.Red;
          Pen contorno = new Pen(Color.DarkRed, 1);

          // fonte para o nome da cidade
          Font fonte = new Font("Arial", 7, FontStyle.Bold);
          Brush corTexto = Brushes.White;

      
          foreach (Cidade cidade in tabelaHash.Conteudo())
          {
            int pixelX = (int)(cidade.X * pbMapa.Width);
            int pixelY = (int)(cidade.Y * pbMapa.Height);

            g.FillEllipse(pincel, pixelX - 5, pixelY - 5, 10, 10);
            g.DrawEllipse(contorno, pixelX - 5, pixelY - 5, 10, 10);

            g.DrawString(cidade.Chave.Trim(), fonte, corTexto, pixelX - 5, pixelY + 7);
          }

          fonte.Dispose();
          contorno.Dispose();
        }

        private void btnAbrirArquivo_Click(object sender, EventArgs e)
        {
          if (dlgAbrir.ShowDialog() == DialogResult.OK)
          {
            arquivoAberto = dlgAbrir.FileName;

            if (rbBucketHash.Checked)
              tabelaHash = new BucketHash<Cidade>();
            else
              if (rbSondagemLinear.Checked)
                tabelaHash = new HashLinear<Cidade>();
              else
                if (rbSondagemQuadratica.Checked)
                  tabelaHash = new HashQuadratico<Cidade>();
                else
                  tabelaHash = new HashDuplo<Cidade>();

            var asCidades = new StreamReader(arquivoAberto);

            while (!asCidades.EndOfStream)
            {
              Cidade cidade = new Cidade();
              cidade = cidade.LerRegistro(asCidades);
              tabelaHash.Incluiu(cidade);
            }

            asCidades.Close(); 

            pbMapa.Invalidate();
          }
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Nome Cidade não Preenchido!");
                return;
            }

            if (udX.Value == default || numericUpDown1.Value == default)
            {
                MessageBox.Show("Coordenada(s) não Preenchida(s)!");
                return;
            }

            Cidade cidadeNova = new Cidade(txtNome.Text, (double)udX.Value, (double)numericUpDown1.Value);
            tabelaHash.Incluiu(cidadeNova);
            txtNome.Text = default;
            txtNome.Focus();    
            udX.Value = default;
            numericUpDown1.Value = default;
            MessageBox.Show($"Cidade {txtNome.Text} Incluída com sucesso!");
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            if (txtNome.Text == default)
            {
                MessageBox.Show("Nome da Cidade não Preenchido!");
                return;
            }   
            Cidade cidadeAExcluir = new Cidade(txtNome.Text);
            tabelaHash.Excluiu(cidadeAExcluir);
            txtNome.Text = default;
            txtNome.Focus();
            MessageBox.Show($"Cidade {txtNome.Text} removida com sucesso!");
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if(tabelaHash == null)//sem nenhuma cidade ainda
            {
                MessageBox.Show("Sem Cidades para listar!");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Nome da cidade não preenchido!");
                return;
            }
            Cidade cidade = new Cidade(txtNome.Text);
            int onde;
            if(!tabelaHash.Existe(cidade,out onde))//lembrete: colocar out, senão, não aceita o onde
            {
                MessageBox.Show($"Cidade não encntrada!");
                return;
            }

            txtNome.Text = default;
            lsbListagem.Items.Clear();
            lsbListagem.Items.Add(cidade);
            MessageBox.Show("Cidade encontrada!");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tabelaHash.Conteudo().Count == 0)
            {
                MessageBox.Show("Sem Cidades para listar!");
                return;
            }
            lsbListagem.Items.Clear();
            foreach (Cidade dadoHash in tabelaHash.Conteudo())
            {
                lsbListagem.Items.Add(dadoHash.Chave);
            }
            MessageBox.Show("Cidade(s) listada(s) com sucesso");
        }

        private void FrmCaminhos_FormClosing(object sender, FormClosingEventArgs e)
        {
          // aqui, a tabela de hash deve ser percorrida e os
          // registros armazenados devem ser gravados no arquivo
          // sua dupla implementa esta parte (item 4)
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            string valorNome = txtNome.Text; 
        }

    }
}