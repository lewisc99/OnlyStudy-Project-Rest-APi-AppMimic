using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicApi.Database;
using MimicApi.Models;
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
        public ActionResult ObterTodas(DateTime? data,int? pagnumero)
        {
            var item = _banco.Palavras.AsQueryable();

            if (data.HasValue)
            {
                item = item.Where(a => a.Criado > data.Value|| a.Atualizado > data.Value);

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
