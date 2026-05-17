using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            if (tabelaHash == null)
            {
                MessageBox.Show("Abra um arquivo primeiro!");
                return;
            }

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
            int onde;
            if (tabelaHash.Existe(cidadeNova, out onde) == true)
            {
                MessageBox.Show("Erro: Esta cidade já está cadastrada!", "Duplicata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                if (tabelaHash.Incluiu(cidadeNova))
                {
                    pbMapa.Invalidate();
                    txtNome.Text = default;
                    txtNome.Focus();
                    udX.Value = default;
                    numericUpDown1.Value = default;
                    MessageBox.Show($"Cidade {txtNome.Text} Incluída com sucesso!");
                    return;
                }
                
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            if (tabelaHash == null)
            {
                MessageBox.Show("Abra um arquivo");
                return;
            }
            if (tabelaHash.Conteudo().Count == 0)
            {
                MessageBox.Show("Sem Cidades para remover!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Nome da Cidade não Preenchido!");
                return;
            }
            Cidade cidadeAExcluir = new Cidade(txtNome.Text);
            int onde;
            if (tabelaHash.Existe(cidadeAExcluir, out onde) == true)
            {
                DialogResult escolha = MessageBox.Show($"Remover: {txtNome.Text} ?", "Salvar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (escolha == DialogResult.Yes)
                {

                    if (tabelaHash.Excluiu(cidadeAExcluir))
                    {
                        lsbListagem.Items.Clear();
                        var listaAtualizada = tabelaHash.Conteudo();
                        foreach (var cidadeRestante in listaAtualizada)
                        {
                            lsbListagem.Items.Add(cidadeRestante.Chave);
                        }   
                        txtNome.Focus();
                        MessageBox.Show($"Cidade {txtNome.Text} removida com sucesso!");
                        pbMapa.Invalidate(); //redesenhar
                        txtNome.Text = default;
                        return;
                    }
                }
                else
                {
                    MessageBox.Show($"{txtNome.Text} não removida(o)!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Essa cidade não existe!");
                return;
            }
            return;
           
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (tabelaHash == null)
            {
                MessageBox.Show("Abra um arquivo");
                return;
            }
            if (tabelaHash.Conteudo().Count == 0)
            {
                MessageBox.Show("Sem Cidades para buscar!");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Nome da cidade não preenchido!");
                return;
            }
            Cidade cidade = new Cidade(txtNome.Text);
            int onde;
            if(tabelaHash.Existe(cidade,out onde) == true)//lembrete: colocar out, senão, não aceita o onde
            {
                txtNome.Text = default;
                lsbListagem.Items.Clear();
                lsbListagem.Items.Add(cidade.Chave);
                MessageBox.Show("Cidade encontrada!");
                return;
            }
            MessageBox.Show("Cidade não encontrada!");
            return;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(tabelaHash == null)//verificação pra ver se a tabela existe
            {                     //se o usuário abriu o arquivo
                MessageBox.Show("Abra um arquivo");
                return;
            }
            if (tabelaHash.Conteudo().Count == 0)//verificação para ver se a tabela
            {                                    //tem dados
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
            if (tabelaHash.Conteudo().Count == 0)
            {
                MessageBox.Show("Nenhum dado para ser salvo!");
                return;
            }
            if (arquivoAberto == null)
            {
                MessageBox.Show("Nenhum arquivo para salvar!");
                return;
            }

            DialogResult escolha = MessageBox.Show("Quer salvar as alterações feitas no arquivo? ", "Salvar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (escolha == DialogResult.Yes)
            {
                // Escrever de forma segura, garantindo truncamento do arquivo e fechamento
                using (var arquivo = new StreamWriter(arquivoAberto, false))
                {
                    foreach (Cidade dadoHash in tabelaHash.Conteudo())
                        dadoHash.EscreverRegistro(arquivo);
                }
                // Atualiza a ListBox 
                lsbListagem.Items.Clear();
                foreach (Cidade dadoHash in tabelaHash.Conteudo())
                    lsbListagem.Items.Add(dadoHash.Chave);

                MessageBox.Show("Dados salvos com sucesso!");
            }
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            string valorNome = txtNome.Text; 
        }

    }
}