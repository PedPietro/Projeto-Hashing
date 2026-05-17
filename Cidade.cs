using System;
using System.Collections.Generic;
using System.IO;

namespace apCaminhosEmMarte
{
  public class Cidade : IRegistro<Cidade>,
                        IComparable<Cidade>
  {
    // atributos que formam uma linha do arquivo de cidades
    string nome;
    double x, y;

    public Cidade() { }  // construtor default

    public Cidade(string nome)//Construtor com 1 parâmetros, para o formulário
        {
        this.nome = nome;
    }

    public Cidade(string nome, double x, double y) //Construtor com 3 parâmetros, para o formulário
    {
        this.nome = nome;
        this.x = x;
        this.y = y;
    }

    // propriedades para acessar as coordenadas de fora da classe
    public double X => x;
    public double Y => y;

    public Cidade LerRegistro(StreamReader arquivo)
    {
      if (arquivo != null)  // está aberto
      {
        string linha = arquivo.ReadLine(); // lê uma linha
        // campos separados por ";"
        string[] campos = linha.Split(';');
        nome = campos[0].Trim();
        x = double.Parse(campos[1].Trim());
        y = double.Parse(campos[2].Trim());
        return this;
      }
      return default(Cidade);  // para arquivo não aberto
    }

    public void EscreverRegistro(StreamWriter arquivo)
    {
      if (arquivo != null)
      {
        // grava no mesmo formato do arquivo original
        arquivo.WriteLine($"{nome};{x:0.00000};{y:0.00000}");
      }
    }

    public int CompareTo(Cidade outra)  // <0, ==0, >0
    {
      return this.nome.CompareTo(outra.nome);
    }

    public string Chave => this.nome;

    public bool Equals(Cidade outra)
    {
      if (outra == null || outra.nome == null) return false;
      if (this.nome == null) return false;
      return this.nome.Equals(outra.nome);
    }

    // exibe o nome da cidade na listagem
    public override string ToString()
    {
      return $"{nome} ({x:0.00000}, {y:0.00000})";
    }
  }
}