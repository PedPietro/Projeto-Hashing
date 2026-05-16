using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HashLinear<T> : IHashing<T>
                            where T : IComparable<T>, IRegistro<T>, new()
{
    const int tamanho = 20007; //numero primo
    T[] tabelaDeHash;
    T removido;//valor para campo que teve item removido

    public HashLinear(int tamanhoDesejado)
    {
        if (tamanhoDesejado <= 0)
            throw new Exception("Tamanho da tabela de dados deve maior que zero!");

        tabelaDeHash = new T[tamanhoDesejado];
        removido = new T();
    }
    public HashLinear() : this(tamanho){ }//construtor caso não tenha passado o parâmetro

    private int Hash(string chave)
    {
        long total = 0;
        for(int i = 0; i < chave.Trim().Length; i++)
            total += 37 * (int)chave[i];//Regra de Horner
        total = total % tabelaDeHash.Length;
        if (total < 0)
            total += tabelaDeHash.Length;
        return (int)total;
    }

    public List<T> Conteudo()
    {
        var dados = new List<T>();
        foreach(T valor in tabelaDeHash)
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
        int indiceHash = Hash(item.Chave);
        onde = -1;
        for(int i = 0;i < tabelaDeHash.Length; i++)
        {
            if(tabelaDeHash[indiceHash] == null || tabelaDeHash[indiceHash] == removido)
                return false;//não existe nada na posição ou o item foi removido

            if (tabelaDeHash[indiceHash].Chave == item.Chave)
                onde = indiceHash;
                return true;


            indiceDeHash = ((indice ));
        }
        return false;
    }

    public bool Incluiu(T item)
    {
        int indiceHash = Hash(item.Chave);
        for(int i = 0; i < tabelaDeHash.Length; i++)
        {
            if (tabelaDeHash[indiceHash] == null || tabelaDeHash[indiceHash] == removido)
                tabelaDeHash[indiceHash] = item;          //posição de exclusão tem nada ou já foi removido;
                return true;

            if (tabelaDeHash[indiceHash].Chave == item.Chave)
            {
                return false;//duplicado
            }

            indiceHash++;//se a posiçaõ está ocupada, parte para a próxima
        }
        Rehashing();
        Incluiu(item);
        return true;
    }

    public bool Excluiu(T item)
    {
        int indiceHash = Hash(item.Chave);
        for(int i = 0;i < tabelaDeHash.Length; i++)
        {
            if (tabelaDeHash[indiceHash] == null || tabelaDeHash[indiceHash] == removido)
                return false;                       //posição de exclusão tem nada ou já foi removido;

            if (tabelaDeHash[indiceHash].Chave == item.Chave)
                tabelaDeHash[indiceHash] = removido;//para diferenciar campos que estão vazios e 
                return true;                       //campos que já tiveram valor, uso "removido"
            
            indiceHash++//a posiçaõ está ocupada, porém não pelo número desejado 
            
        }
        return false;
    }

    private int proximoPrimo(int n) //criei essa função para auxiliar o 
    {                               //rehashing a manter um tamanho de tabela primo
        if (n % 2 == 0) n++;
        while (!EhPrimo(n))
            n += 2;
        return n;
    }

    private bool ehPrimo(int n)//verifica se é primo 
    {
        if (n < 2) return false;
        for (int i = 2; i * i <= n; i++)
            if (n % i == 0) return false;
        return true;
    }

    private void Rehashing()
    {
        T[] velhaTabela = tabelaDeHash;

        int novoTamanho = ProximoPrimo(tabelaDeHash.Length * 2);
        tabelaDeHash = new T[novoTamanho];
        quantidade = 0;

        for (int i = 0; i < velhaTabela.Length; i++)
        {
            if (velhaTabela[i] != null &&
                !velhaTabela[i].Equals(removido))
                Incluiu(velhaTabela[i]);
        }
    }
}

