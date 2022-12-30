using Microsoft.AspNetCore.Mvc;
using Mongo.Api.Controllers.Inputs;
using Mongo.Api.Domain.Entities;
using Mongo.Api.Domain.Enums;
using Mongo.Api.Domain.ValueObjects;

namespace Mongo.Api.Controllers;

[ApiController]
public class RestauranteController : ControllerBase
{
    [HttpPost("restaurante")]
    public ActionResult IncluirRestaurante([FromBody] RestauranteInclusao restauranteInclusao)
    {
        var cozinha = ECozinhaHelper.ConverterDeInteiro(restauranteInclusao.Cozinha);

        var restaurante = new Restaurante(restauranteInclusao.Nome, cozinha);
        var endereco = new Endereco(
            restauranteInclusao.Logradouro,
            restauranteInclusao.Numero,
            restauranteInclusao.Cidade,
            restauranteInclusao.UF,
            restauranteInclusao.Cep);

        restaurante.AtribuirEndereco(endereco);

        if (!restaurante.Validar())
        {
            return BadRequest(
                new
                {
                    errors = restaurante.ValidationResult.Errors.Select(_ => _.ErrorMessage)
                });
        }

        //_restauranteRepository.Inserir(restaurante);

        return Ok(
            new
            {
                data = "Restaurante inserido com sucesso"
            }
        );
    }
}
