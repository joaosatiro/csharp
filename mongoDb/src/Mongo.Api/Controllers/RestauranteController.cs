using Microsoft.AspNetCore.Mvc;
using Mongo.Api.Controllers.Inputs;
using Mongo.Api.Controllers.Outputs;
using Mongo.Api.Data.Repositories;
using Mongo.Api.Domain.Entities;
using Mongo.Api.Domain.Enums;
using Mongo.Api.Domain.ValueObjects;

namespace Mongo.Api.Controllers;

[ApiController]
public class RestauranteController : ControllerBase
{
    private readonly RestauranteRepository _restauranteRepository;

    public RestauranteController(RestauranteRepository restauranteRepository)
    {
        _restauranteRepository = restauranteRepository;
    }

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

        _restauranteRepository.Inserir(restaurante);

        return Ok(
            new
            {
                data = "Restaurante inserido com sucesso"
            }
        );
    }

    [HttpGet("restaurante/todos")]
    public async Task<ActionResult> ObterRestaurantes()
    {
        var restaurantes = await _restauranteRepository.ObterTodos();

        var listagem = restaurantes.Select(_ => new RestauranteListagem
        {
            Id = _.Id,
            Nome = _.Nome,
            Cozinha = (int)_.Cozinha,
            Cidade = _.Endereco.Cidade
        });

        return Ok(
            new
            {
                data = listagem
            }
        );
    }

    [HttpGet("restaurante/{id}")]
    public ActionResult ObterRestaurante(string id)
    {
        var restaurante = _restauranteRepository.ObterPorId(id);

        if (restaurante == null)
            return NotFound();

        var exibicao = new RestauranteExibicao
        {
            Id = restaurante.Id,
            Nome = restaurante.Nome,
            Cozinha = (int)restaurante.Cozinha,
            Endereco = new EnderecoExibicao
            {
                Logradouro = restaurante.Endereco.Logradouro,
                Numero = restaurante.Endereco.Numero,
                Cidade = restaurante.Endereco.Cidade,
                Cep = restaurante.Endereco.Cep,
                UF = restaurante.Endereco.UF
            }
        };

        return Ok(
            new
            {
                data = exibicao
            }
        );
    }
}
