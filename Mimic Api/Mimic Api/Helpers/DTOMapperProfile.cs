using AutoMapper;
using Mimic_Api.Models.DTO;
using MimicApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimic_Api.Helpers
{
    public class DTOMapperProfile :Profile //tem que herdar de profile
    {

        //em profile vai falar quais objetos
        //vao possibilitar o mapeamento.
        public DTOMapperProfile()
        {
            //primeiro parametros e a classe de origem, depois a classe que quer
            //enviar as informações.
            CreateMap<Palavra, PalavraDTO>();
            CreateMap<PaginationList<Palavra>, PaginationList<PalavraDTO>>();

        }
    }
}


//automapper no caso PalavraDTO e uma copia da classe Palavra
/* em vez de copiarmos cada objeto de Palavra para palavraDTO
 * exemplo
 * PalavraDTO.Id = palavra.ID;
 *PalavraDTO.Nome = palavra.Nome;
 *com o AutoMapper ele faz isso de forma mais simples
 **/