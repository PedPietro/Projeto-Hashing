using System;
using System.Collections;
using System.Collections.Generic;

public class BucketHash<T> : IHashing<T>
             where T : IRegistro<T>, IEquatable<T>, new()
{
    private const int SIZE = 21;
    ArrayList[] dados;

    public BucketHash()
    {
        dados = new ArrayList[SIZE];        // cria vetor
        
        // cada posição do vetor recebe um ArrayList
        for (int i = 0; i < SIZE; i++)
            dados[i] = new ArrayList(4);    
    }
    
    private int Hash(string chave)
    {
        long tot = 0;
        for (int i = 0; i < chave.Length; i++)
            tot += 37 * tot + (int)chave[i];
        tot = tot % dados.Length;
        if (tot < 0)
            tot += dados.Length;
        return (int)tot;
    }

    public bool Incluiu(T novoDado)
    {
        int indiceDeHash = Hash(novoDado.Chave);
        if (!dados[indiceDeHash].Contains(novoDado))
        {
            dados[indiceDeHash].Add(novoDado);
            return true;
        }
        return false;
    }

    public bool Existe(T procurado, out int onde)
    {
        onde = Hash(procurado.Chave);
        foreach (T item in dados[onde])
        {
            if (item.Chave == procurado.Chave)
            {
                return true;
            }
        }

        return false;
    }

    public bool Excluiu(T dadoAExcluir)
    {
        int onde = 0;
        if (!Existe(dadoAExcluir, out onde))
            return false;
        ArrayList bucket = dados[onde];
        for (int i = 0; i < bucket.Count; i++)
        {
            T item = (T)bucket[i];
            if (item.Chave == dadoAExcluir.Chave)// Remove pela chave
            {
                bucket.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public List<string> LocaisDosDados()
    {
        List<string> saida = new List<string>();
        for (int i = 0; i < dados.Length; i++) // percorre vetor
            if (dados[i].Count > 0)            // há objetos na lista
            {
                string linha = $"{i,5} : ";
                foreach (T dado in dados[i])
                    linha += " | " + dado;
                saida.Add(linha);
            }
        return saida;
    }
    public List<T> Conteudo()
    {
        List<T> saida = new List<T>();
        for (int i = 0; i < dados.Length; i++) // percorre vetor
            foreach (T dado in dados[i])
                saida.Add(dado);
        return saida;
    }
}

