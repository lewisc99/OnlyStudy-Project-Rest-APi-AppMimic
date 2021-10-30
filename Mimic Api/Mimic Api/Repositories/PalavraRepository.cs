using Microsoft.EntityFrameworkCore;
using Mimic_Api.Helpers;
using Mimic_Api.Repositories.Contracts;
using MimicApi.Database;
using MimicApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimic_Api.Repositories
{
    public class PalavraRepository : IPalavraRepository //temos uma classe que implementa uma interface
    {

        private readonly MimicContext _banco;
        public PalavraRepository(MimicContext banco)
        {
            _banco = banco;
        }


        // public List<Palavra> ObterPalavras(PalavraUrlQuery query)
        public PaginationList<Palavra> ObterPalavras(PalavraUrlQuery query)

        {
            //se notar logo acima foi criado uma classe em Helpers
            //com as propriedades que usando no parametro ObterTodas
            //poderiamos ter usado os parametros direto, acima e apenas para deixar o 
            //codigo mais limpo.


            var lista = new PaginationList<Palavra>();

            var item = _banco.Palavras.AsNoTracking().AsQueryable();  //para retornar alguma consulta sql.



            if (query.data.HasValue)
            {
                item = item.Where(a => a.Criado > query.data.Value || a.Atualizado > query.data.Value);

            }
            if (query.pagNumero.HasValue)
            {
                var quantidadeTotalRegistros = item.Count();


                item = item.Skip(query.pagNumero.Value - 1 * query.pagRegistro.Value).Take(query.pagRegistro.Value); //se não tiver registro coloca sempre value que pega nulo.
                //se ta na primeira pagina não precisa pular
                //se ta na segunda pagina  precisa pular todos os registros.  o que estão na primeira.
                //exemplo se a pagina e um subtrai por 1, 0 * 10 registro por pagina e igual a 0, então não pula ninguem.
                // assim mostra 0 a 10 registro na primeira pagina.
                var paginacao = new Paginacao();


                paginacao.NumeroPagina = query.pagNumero.Value;
                paginacao.RegistroPorPagina = query.pagRegistro.Value;
                paginacao.TotalRegistro = quantidadeTotalRegistros;
                paginacao.TotalPaginas = (int)Math.Ceiling((double)quantidadeTotalRegistros / query.pagRegistro.Value);  // 30/10 = 3, 


                lista.Paginacao = paginacao;

            }

            lista.AddRange(item.ToList());

            return lista;
        }


            public Palavra Obter(int id)
        {
            var obj = _banco.Palavras.AsNoTracking().FirstOrDefault(banco => banco.Id == id);

            return obj;
        }

        public void Cadastrar(Palavra palavra)
        {
            _banco.Palavras.Add(palavra);
            _banco.SaveChanges();
            
        }

        public void Atualizar(Palavra palavra)
        {
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();


        }

       

        public void Deletar(int id)
        {

           
           var obj =  Obter(id);
            _banco.Palavras.Remove(obj);
            _banco.SaveChanges();
        }

       

      
    }
}
