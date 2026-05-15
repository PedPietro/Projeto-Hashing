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

    IHashing<Cidade> tabelaDeHash;
    string arquivoAberto; 

    private void Form1_Load(object sender, EventArgs e)
    {

      pbMapa.Paint += new PaintEventHandler(pbMapa_Paint);
    }

    private void pbMapa_Paint(object sender, PaintEventArgs e)
    {
      if (tabelaDeHash == null)
        return;

      Graphics g = e.Graphics;

      Brush pincel = Brushes.Red;
      Pen contorno = new Pen(Color.DarkRed, 1);

      // fonte para o nome da cidade
      Font fonte = new Font("Arial", 7, FontStyle.Bold);
      Brush corTexto = Brushes.White;

      
      foreach (Cidade cidade in tabelaDeHash.Conteudo())
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
          tabelaDeHash = new BucketHash<Cidade>();
        else
          if (rbSondagemLinear.Checked)
            tabelaDeHash = new HashLinear<Cidade>();
          else
            if (rbSondagemQuadratica.Checked)
              tabelaDeHash = new HashQuadratico<Cidade>();
            else
              tabelaDeHash = new HashDuplo<Cidade>();

        var asCidades = new StreamReader(arquivoAberto);

        while (!asCidades.EndOfStream)
        {
          Cidade cidade = new Cidade();
          cidade = cidade.LerRegistro(asCidades);
          tabelaDeHash.Incluiu(cidade);
        }

        asCidades.Close(); 

        pbMapa.Invalidate();
      }
    }

    private void btnInserir_Click(object sender, EventArgs e)
    {
      // cria cidade com os dados digitados nos campos
      // sua dupla implementa esta parte (item 4)
    }

    private void btnRemover_Click(object sender, EventArgs e)
    {
      // sua dupla implementa esta parte (item 4)
    }

    private void btnBuscar_Click(object sender, EventArgs e)
    {
      // sua dupla implementa esta parte (item 4)
    }

    private void button1_Click(object sender, EventArgs e)
    {
      // sua dupla implementa esta parte (item 4)
    }

    private void FrmCaminhos_FormClosing(object sender, FormClosingEventArgs e)
    {
      // aqui, a tabela de hash deve ser percorrida e os
      // registros armazenados devem ser gravados no arquivo
      // sua dupla implementa esta parte (item 4)
    }
  }
}