using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimic_Api.V2.Controllers
{
    //forma simplificada de criar versionamento de versions da API, no caso V1 e a primeira versão e V2 e a segunda Versão.


    //api/v1.0/palavras
    //essa forma abaixo o usuario vai usar a APi 2.0
    // [Route("api/palavras")] //nome api/mais depois o nome do controller Palavras

    //porem podemos deixar para o usuario escolher qual versão deseja escolher.

    [Route("api/v{version:apiVersion}/palavras")]

  //  [Route("api/[Controller]")]



    [ApiController]
    [ApiVersion("2.0")] //so isso de colocar a versão já e suficiente para classicar as versões da APi.



    public class PalavrasController:ControllerBase
    {

        [HttpGet("", Name = "ObterTodas")]
        //  [HttpGet("")]

        public string ObterTodas()
        {
            return "Version 2.0";
           
        }

    }
}
