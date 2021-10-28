using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mimic_Api.Helpers;
using MimicApi.Database;
using MimicApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimic_Api.Controllers
{

    [Route("api/palavras")] //nome api/mais depois o nome do controller Palavras

    [ApiController]
    public class PalavrasController:ControllerBase
    {
        private readonly MimicContext _banco;
        public PalavrasController(MimicContext  banco)
        {
            _banco = banco;



        }



        //Para App, obter informações de todas palavras/

        //no caso retorna todos // /api/palavras

        //app -- /api/palavras
        //app -- /api/palavras?numero
        [HttpGet]



        //criando paginação

        //fromQuery quer dizer que veio da url exemplo: localhost:44349/api/palavras?pagNumero=3&pagRegistro=1
        public ActionResult ObterTodas([FromQuery]PalavraUrlQuery query)
        {

            //se notar logo acima foi criado uma classe em Helpers
            //com as propriedades que usando no parametro ObterTodas
            //poderiamos ter usado os parametros direto, acima e apenas para deixar o 
            //codigo mais limpo.



          
            var item = _banco.Palavras.AsQueryable();  //para retornar alguma consulta sql.
           


            if (query.data.HasValue)
            {
                item = item.Where(a => a.Criado > query.data.Value|| a.Atualizado > query.data.Value);

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




                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginacao));

                if (query. pagNumero > paginacao.TotalPaginas)

                {
                    return NotFound();
                }




            }



            //retornando um ok vai retornar
            // para o metodo mais popular no caso JSON
            return  Ok(item);

           


        }

        //metodos para Web,
        //no usando Crud.

         // /api/palavras/1 (1 e o id).
        [HttpGet("{id}")]
        public ActionResult Obter(int id)

        {
            var obj = _banco.Palavras.Find(id);
            if (obj ==null)
            {
                return NotFound(); //notfound tem que adotar toda vez que não localizar informação
                //ou pode retornar o status do codigo no caso
                // return StatusCode(404);


            }





            return Ok(_banco.Palavras.Find(id)); //quando dar certo retorna ok que no caso e 200
        }


         //para cadastrar não precisa acessar nenhum id
        //assim pode acessar igual o metodo para obter todos api/palavras
        [HttpPost]
        public ActionResult Cadastrar([FromBody]Palavra Palavra)
        {
            _banco.Palavras.Add(Palavra);
            _banco.SaveChanges();
            return Created($"/api/palavras/{Palavra.Id}",Palavra);
            //return created e usado quando e feito o cadastro de intem, 
            //ai o sistema vai redicionar o usario para o cadastro que mesmo acabou de fazer.
            //pela url /api/palavras/id da palavra cadastra, e no corpo retorna a palavra.

        }

        // -- /api/palavras/1 (put:id,nome,ativo,pontução,criacao).

       // /api/palavras/1 (1 e o id).
        [HttpPut("{id}")]
        public ActionResult Atualizar(int id,[FromBody]Palavra palavra)
        {


            //codigo antigo   var obj = _banco.Palavras.find(id);
            var obj = _banco.Palavras.AsNoTracking().FirstOrDefault(banco => banco.Id == id); //se tiver problema usando o find usa o codigo
            //_banco.Palavras.AsNoTracking().FirstOrdefault(); 
            //porque o entity framework mapeia toda consulta e guarda, e como criamos outro var obj o entity framework informa que tem 2 objetos do mesmo tipo o var obj e o palavra.id abaixo que tentamos consutlar.
            if (obj == null)
            {
                return NotFound(); //notfound tem que adotar toda vez que não localizar informação
                //ou pode retornar o status do codigo no caso
                // return StatusCode(404);


            }

            palavra.Id = id;
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();
            return Ok();
        }

        // -- /api/palavras/1 (delete)

       

        [HttpDelete("{id}")]
        public ActionResult  Deletar(int id)
        {

            var obj = _banco.Palavras.Find(id);
            if (obj == null)
            {
                return NotFound(); //notfound tem que adotar toda vez que não localizar informação
                //ou pode retornar o status do codigo no caso
                // return StatusCode(404);


            }

            _banco.Palavras.Remove(_banco.Palavras.Find(id));
            _banco.SaveChanges();
            return Ok();
        }
    }
}
