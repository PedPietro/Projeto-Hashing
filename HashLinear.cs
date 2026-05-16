using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HashLinear<T> : IHashing<T>
                            where T : IComparable<T>, IRegistro<T>, new()
{
    const int tamanho = 20007; //numero primo
    T[] tabelaHash;
    public HashLinear(int tamanho)
    {
        if (tamanho <= 0)
            throw new Exception("Tamanho da tabela de dados deve maior que zero!");

        tabelaHash = new T[tamanho];
    }

    private int Hash(string chave)
    {
           
    }

    public List<T> Conteudo()
    {
        var dados = new List<T>();
        foreach(T valor in tabelaHash)
            if (valor != null)
                dados.Add(valor);
            return dados;

    }

    public List<string> LocaisDosDados()
    {
        throw new NotImplementedException();
    }

    public bool Existe(T item, out int onde)
    { 
        throw new NotImplementedException();
    }

    public bool Incluiu(T item)
    {
        throw new NotImplementedException();
    }

    public bool Excluiu(T item)
    {
        throw new NotImplementedException();
    }
}

