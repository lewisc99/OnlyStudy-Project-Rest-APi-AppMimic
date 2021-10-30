using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mimic_Api.Helpers;
using Mimic_Api.V1.Models.DTO;
using Mimic_Api.V1.Repositories;
using Mimic_Api.V1.Repositories.Contracts;

using MimicApi.V1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimic_Api.V1.Controllers
{

    //  [Route("api/palavras")] //nome api/mais depois o nome do controller Palavras

    [Route("api/v{version:apiVersion}/palavras")]

    [ApiController] //para usar o versionamento precisa colocar o API Controller.

    // [ApiVersion("1.0")] //indicando a versão da API.

    // [ApiVersion("1.0",Deprecated =true)] //forma de informa que a api está Obsoleta não funciona.

    [ApiVersion("1.0",Deprecated =true)]
    //imagina que você deseja criar uma outra versão da APi no caso 1.1

    //ai acima do metodo informa qual versão deseja que seja padrão para o metodo.
    //exemplo

    /* [MapToApiVersion("1.0")]
     public ActionResult ObterTodas([FromQuery] PalavraUrlQuery query)
     {
    */
    //
    //tambem e preciso em cima da classe informa que 
    [ApiVersion("1.1")]


    public class PalavrasController:ControllerBase
    {
       // private readonly MimicContext _banco; //tira a dependencia do banco
        private readonly IPalavraRepository _repo; //tira a dependencia do banco de dados, e coloca um repositorio especifico para as palavras.
        private readonly IMapper _mapper; //tira a dependencia do banco de dados, e coloca um repositorio especifico para as palavras.

        public PalavrasController(IPalavraRepository banco, IMapper mapper)
        {
            _repo = banco;
            _mapper = mapper;
        }




        //Para App, obter informações de todas palavras/

        //no caso retorna todos // /api/palavras

        //app -- /api/palavras
        //app -- /api/palavras?numero




        //criando paginação
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        //fromQuery quer dizer que veio da url exemplo: localhost:44349/api/palavras?pagNumero=3&pagRegistro=1
        [HttpGet("",Name ="ObterTodas")]
        //  [HttpGet("")]


        //indica que esse metodo vai funcionar nas duas versões.
   

        public ActionResult ObterTodas([FromQuery]PalavraUrlQuery query)
        {

           var item = _repo.ObterPalavras(query);


            /*  if (query.pagNumero > item.Paginacao.TotalPaginas)
              {
                  return NotFound();
              }
             */



            // if (item.Count == 0) //se nap tiver nenhum item em uma pagina, então retorna abaixo.
            if (item.Resuls.Count == 0) 
            {
                return NotFound();
            }



         

          var lista = _mapper.Map<PaginationList<Palavra>,PaginationList<PalavraDTO>>(item);

            //para cada palavra adiciona um link
           // foreach(var palavra in lista)
            foreach(var palavra in lista.Resuls)
            {

                palavra.Links = new List<LinkDTO>();
                palavra.Links.Add(new LinkDTO("self", Url.Link("ObterPalavra", new { id = palavra.Id }), "GET"));
            }
            //se tiver parametros na query da url tipo localhost:44349/api/palavras?pagNumero=1&pagRegistro=2, ele aceita, caso não tenha aparece todas os itens da lista.
            lista.Links.Add(new LinkDTO("self", Url.Link("ObterTodas", query), "GET"));

            if (item.Paginacao != null)
            {
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(item.Paginacao));

                lista.Links.Add(new LinkDTO("self", Url.Link("ObterTodas",query), "GET"));


                if(query.pagNumero + 1 <= item.Paginacao.TotalPaginas)
                {
                    var queryString = new PalavraUrlQuery() { pagNumero = query.pagNumero + 1, pagRegistro = query.pagRegistro, data = query.data };
                    lista.Links.Add(new LinkDTO("next", Url.Link("ObterTodas", queryString), "GET"));
                }


                if (query.pagNumero - 1 > 0 )
                {
                    var queryString = new PalavraUrlQuery() { pagNumero = query.pagNumero - 1, pagRegistro = query.pagRegistro, data = query.data };
                    lista.Links.Add(new LinkDTO("prev", Url.Link("ObterTodas", queryString), "GET"));
                }


            }




            return Ok(lista);
        }


        //  Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginacao));
            //retornando um ok vai retornar
            // para o metodo mais popular no caso JSON
         

           


        

        //metodos para Web,
        //no usando Crud.

         // /api/palavras/1 (1 e o id).
        [HttpGet("{id}",Name ="ObterPalavra")]

        //indica que esse metodo vai funcionar nas duas versões.
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]

        public ActionResult Obter(int id)

        {

            var obj = _repo.Obter( id);


            if (obj ==null)
            {
                return NotFound(); //notfound tem que adotar toda vez que não localizar informação
                //ou pode retornar o status do codigo no caso
                // return StatusCode(404);
                

            }

            //primeiro nos criamos um objeto que herda de PalavraDTO, e mapeia as informações que estão na classe Palavra, e coloca em Palavra DTO,
            //obj e e obj que chamamos atraves do id no parametro Obter
            PalavraDTO palavraDTO = _mapper.Map<Palavra, PalavraDTO>(obj);

            //como temos uma propriedade em palavraDTO a mais que palavra que é LINK, criamos abaixo uma instanciamos, da classe Link, que têm 3 propriedades Rel
            //Href e metodo, criamos uma lista de objeto de link, para que possa ser adicionado varios link dentro do objeto caso venha ter outro links para atualizar editar etc.
         
            // palavraDTO.Links = new List<LinkDTO>();

            //adicionamos na propriedade link da classe palavraDTO uma nova instancia de LINKDTO,
            //e usamos sua propriedade que no caso é rel = para ele mesmo(Self), href= que e o link para o id, e method Get para apenas acessar o id.

            //LINK para obter o ID
            /* antigo codigo funciona da mesma maneira, so que vamos mudar o $(link), para o
             * caso o o localhost seja alterado para algum servidor etc, altera o url automaticamente.
            palavraDTO.Links.Add(new LinkDTO(
                "self",
                $"https://localhost:44349/api/palavras/{palavraDTO.Id}",
                "GET")); */

            //LINK para obter o ID

            palavraDTO.Links.Add(new LinkDTO(
                "self",
                Url.Link("ObterPalavra",new { id = palavraDTO.Id }),
                "GET"));

            //LINK para Editar o ID
            palavraDTO.Links.Add(new LinkDTO(
               "self",
               Url.Link("AtualizarPalavra",new { id = palavraDTO.Id }),
               "PUT"));

            //LINK para excluir o ID.
            palavraDTO.Links.Add(new LinkDTO(
              "self",
              Url.Link("ExcluirPalavra",new { id = palavraDTO.Id }),
              "DELETE"));

            return Ok(palavraDTO); //quando dar certo retorna ok que no caso e 200
        }


         //para cadastrar não precisa acessar nenhum id
        //assim pode acessar igual o metodo para obter todos api/palavras
        [HttpPost]
        public ActionResult Cadastrar([FromBody]Palavra palavra)
        {

            if(palavra ==null)
            {
                return BadRequest(); //se palavra não for preenchida não tiver nada preenchdio
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState); //se o ModelState não for preenchido correto
            }


            palavra.Ativo = true;
            palavra.Criado = DateTime.Now;
            _repo.Cadastrar(palavra);


           PalavraDTO palavraDTO =  _mapper.Map<Palavra, PalavraDTO>(palavra);

            palavraDTO.Links.Add(
                new LinkDTO("self", Url.Link("ObterPalavra", new { id = palavraDTO.Id }), "GET"));


            return Created($"/api/palavras/{palavra.Id}", palavraDTO);
            //return created e usado quando e feito o cadastro de intem, 
            //ai o sistema vai redicionar o usario para o cadastro que mesmo acabou de fazer.
            //pela url /api/palavras/id da palavra cadastra, e no corpo retorna a palavra.

        }

        // -- /api/palavras/1 (put:id,nome,ativo,pontução,criacao).

       // /api/palavras/1 (1 e o id).
        [HttpPut("{id}",Name = "AtualizarPalavra")]
        public ActionResult Atualizar(int id,[FromBody]Palavra palavra)
        {

           

            var obj =  _repo.Obter(id);


            if (palavra == null)
            {
                return BadRequest(); //se palavra não for preenchida não tiver nada preenchdio
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState); //se o ModelState não for preenchido correto
            }


            //codigo antigo   var obj = _banco.Palavras.find(id);
            //   var obj = _banco.Palavras.AsNoTracking().FirstOrDefault(banco => banco.Id == id); //se tiver problema usando o find usa o codigo
            //_banco.Palavras.AsNoTracking().FirstOrdefault(); 
            //porque o entity framework mapeia toda consulta e guarda, e como criamos outro var obj o entity framework informa que tem 2 objetos do mesmo tipo o var obj e o palavra.id abaixo que tentamos consutlar.
            if (obj == null)
            {
                return NotFound(); //notfound tem que adotar toda vez que não localizar informação
                //ou pode retornar o status do codigo no caso
                // return StatusCode(404);


            }

            palavra.Id = id;
            palavra.Ativo = obj.Ativo;
            palavra.Criado = obj.Criado;
            palavra.Atualizado = DateTime.Now;


            _repo.Atualizar(palavra);

            PalavraDTO palavraDTO = _mapper.Map<Palavra, PalavraDTO>(palavra);
             
            palavraDTO.Links.Add(new LinkDTO("self",
                Url.Link("ObterPalavra", new { id = id }), "GET"));


            return Ok();
        }

        // -- /api/palavras/1 (delete)


        //indica que esse metodo vai funcionar apenas na versão da APi 1.1
        
        [MapToApiVersion("1.1")]


        [HttpDelete("{id}",Name = "ExcluirPalavra")]
        public ActionResult  Deletar(int id)
        {

            var obj = _repo.Obter(id);
            if (obj == null)
            {
                return NotFound(); //notfound tem que adotar toda vez que não localizar informação
                //ou pode retornar o status do codigo no caso
                // return StatusCode(404);


            }

            _repo.Deletar(id);
            return Ok();
        }
    }
}
