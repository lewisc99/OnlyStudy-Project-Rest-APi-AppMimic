using Mimic_Api.Helpers;
using MimicApi.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimic_Api.V1.Repositories.Contracts
{
    public interface IPalavraRepository
    {

        PaginationList<Palavra> ObterPalavras(PalavraUrlQuery query);
       Palavra Obter(int id);



        void Cadastrar(Palavra palavra);

        void Atualizar( Palavra palavra);

        void Deletar(int id);

    }
}
