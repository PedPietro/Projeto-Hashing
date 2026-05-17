using System;
using System.Collections.Generic;

public class HashDuplo<T> : IHashing<T>
  where T : IComparable<T>, IRegistro<T>, new()
{
    const int tamanhoPadrao = 23;
    T[] tabelaDeHash;
    T removido;        
    int quantidade;

    public HashDuplo()
    {
        tabelaDeHash = new T[tamanhoPadrao];
        removido = new T();
        quantidade = 0;
    }

    private int Hash1(string chave)
    {
        long tot = 0;
        for (int i = 0; i < chave.Length; i++)
            tot = 37 * tot + (int)chave[i];
        tot = tot % tabelaDeHash.Length;
        if (tot < 0)
            tot += tabelaDeHash.Length;
        return (int)tot;
    }

    private int Hash2(string chave)
    {
        long tot = 0;
        for (int i = 0; i < chave.Length; i++)
            tot = 37 * tot + (int)chave[i];
        if (tot < 0)
            tot = -tot;

        int R = PrimoMenorQue(tabelaDeHash.Length);
        int salto = (int)(R - (tot % R));

        if (salto == 0) salto = 1;
        return salto;
    }

    public bool Incluiu(T novoDado)
    {
        int descarte;
        if (Existe(novoDado, out descarte))
            return false;

        if (quantidade > tabelaDeHash.Length * 0.5)
            Rehashing();

        int pos = Hash1(novoDado.Chave);
        int salto = Hash2(novoDado.Chave);
        int i = 0;

        while (tabelaDeHash[pos] != null &&
               !tabelaDeHash[pos].Equals(removido))
        {
            i++;
            pos = (pos + salto) % tabelaDeHash.Length;
        }

        tabelaDeHash[pos] = novoDado;
        quantidade++;
        return true;
    }

    public bool Existe(T dadoAProcurar, out int onde)
    {
        int pos = Hash1(dadoAProcurar.Chave);
        int salto = Hash2(dadoAProcurar.Chave);
        onde = -1;

        while (tabelaDeHash[pos] != null)
        {
            if (!tabelaDeHash[pos].Equals(removido) &&
                tabelaDeHash[pos].Chave ==  dadoAProcurar.Chave)
            {
                onde = pos;
                return true;
            }
            pos = (pos + salto) % tabelaDeHash.Length;
        }
        return false;
    }

    public bool Excluiu(T dadoAExcluir)
    {
        int onde;
        if (!Existe(dadoAExcluir, out onde))
            return false;

        tabelaDeHash[onde] = removido;
        quantidade--;
        return true;
    }

    private void Rehashing()
    {
        T[] velhaTabela = tabelaDeHash;
        int novoTamanho = ProximoPrimo(tabelaDeHash.Length * 2);
        tabelaDeHash = new T[novoTamanho];
        quantidade = 0;

        for (int i = 0; i < velhaTabela.Length; i++)
            if (velhaTabela[i] != null &&
                !velhaTabela[i].Equals(removido))
                Incluiu(velhaTabela[i]);
    }

    private int PrimoMenorQue(int n)
    {
        n--;
        if (n % 2 == 0) n--;
        while (!EhPrimo(n))
            n -= 2;
        return n;
    }

    private int ProximoPrimo(int n)
    {
        if (n % 2 == 0) n++;
        while (!EhPrimo(n))
            n += 2;
        return n;
    }

    private bool EhPrimo(int n)
    {
        if (n < 2) return false;
        for (int i = 2; i * i <= n; i++)
            if (n % i == 0) return false;
        return true;
    }

    public List<string> LocaisDosDados()
    {
        var saida = new List<string>();
        for (int i = 0; i < tabelaDeHash.Length; i++)
            if (tabelaDeHash[i] != null &&
                !tabelaDeHash[i].Equals(removido))
                saida.Add($"{i,5} : {tabelaDeHash[i]}");
        return saida;
    }

    public List<T> Conteudo()
    {
        var saida = new List<T>();
        for (int i = 0; i < tabelaDeHash.Length; i++)
            if (tabelaDeHash[i] != null &&
                !tabelaDeHash[i].Equals(removido))
                saida.Add(tabelaDeHash[i]);
        return saida;
    }
}