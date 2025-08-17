using Ambev.DeveloperEvaluation.Application.Sales.AddItem;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteItem;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateItem;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Requests;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Responses;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CreateSaleRequest = Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale.CreateSaleRequest;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Endpoints para gestão de vendas (criar, consultar, listar, atualizar cabeçalho/itens e cancelar).
/// </summary>
/// <remarks>
/// Convenções:
/// - Todas as entradas/saídas usam <c>application/json</c>.
/// - Datas em ISO 8601 (UTC ou com timezone explícito).
/// - Paginação padrão: <c>page</c> (1-based), <c>pageSize</c>.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>Inicializa o controller de vendas.</summary>
    /// <param name="mediator">Instância do MediatR.</param>
    /// <param name="mapper">Instância do AutoMapper.</param>
    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>Cria uma nova venda.</summary>
    /// <param name="request">Payload da venda com cabeçalho e itens.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Dados básicos da venda criada.</returns>
    /// <response code="201">Venda criada com sucesso. Retorna a venda e o header <c>Location</c> apontando para <c>GET /api/sales/{id}</c>.</response>
    /// <response code="400">Payload inválido (erros de validação).</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateSaleCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);
        var response = _mapper.Map<CreateSaleResponse>(result);

        // 201 Created + Location: /api/sales/{id}
        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            new ApiResponseWithData<CreateSaleResponse>
            {
                Success = true,
                Message = "Sale created successfully",
                Data = response
            });
    }

    /// <summary>Obtém uma venda pelo seu identificador.</summary>
    /// <param name="id">ID da venda (GUID).</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Detalhes completos da venda.</returns>
    /// <response code="200">Venda encontrada.</response>
    /// <response code="404">Venda não encontrada.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetSaleQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(new ApiResponseWithData<GetSaleResponse>
        {
            Success = true,
            Message = "Sale fetched successfully",
            Data = _mapper.Map<GetSaleResponse>(result)
        });
    }

    /// <summary>Lista/pagina vendas.</summary>
    /// <param name="number">Filtra por número da venda (igualdade).</param>
    /// <param name="customer">Filtra por nome do cliente (contém).</param>
    /// <param name="dateFrom">Data inicial (inclusive).</param>
    /// <param name="dateTo">Data final (inclusive).</param>
    /// <param name="page">Página (1-based). Padrão: 1.</param>
    /// <param name="pageSize">Tamanho da página. Padrão: 20.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Página de vendas conforme filtros.</returns>
    /// <response code="200">Lista paginada de vendas.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ListSalesResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ListSalesResponse>> List(
        [FromQuery] string? number,
        [FromQuery] string? customer,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var res = await _mediator.Send(new ListSalesQuery(number, customer, dateFrom, dateTo, page, pageSize), ct);
        var dto = _mapper.Map<ListSalesResponse>(res);
        return dto; // 200 OK
    }

    /// <summary>Atualiza o cabeçalho de uma venda.</summary>
    /// <param name="id">ID da venda.</param>
    /// <param name="req">Campos do cabeçalho que serão substituídos.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="204">Atualizado com sucesso.</response>
    /// <response code="404">Venda não encontrada.</response>
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateSaleRequest req, CancellationToken ct)
    {
        var ok = await _mediator.Send(new UpdateSaleCommand(id, req.Date, req.CustomerId, req.CustomerName, req.BranchId, req.BranchName), ct);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>Adiciona um item à venda.</summary>
    /// <param name="id">ID da venda.</param>
    /// <param name="req">Produto, preço unitário e quantidade.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="204">Item adicionado com sucesso.</response>
    /// <response code="404">Venda não encontrada.</response>
    [HttpPost("{id:guid}/items")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddItem([FromRoute] Guid id, [FromBody] AddOrUpdateSaleItemRequest req, CancellationToken ct)
    {
        var ok = await _mediator.Send(new AddSaleItemCommand(id, req.ProductId, req.ProductName, req.UnitPrice, req.Quantity), ct);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>Atualiza um item existente da venda.</summary>
    /// <param name="id">ID da venda.</param>
    /// <param name="productId">ID do produto (chave do item).</param>
    /// <param name="req">Novos dados do item.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="204">Item atualizado com sucesso.</response>
    /// <response code="404">Venda ou item não encontrado.</response>
    [HttpPut("{id:guid}/items/{productId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem([FromRoute] Guid id, [FromRoute] Guid productId, [FromBody] AddOrUpdateSaleItemRequest req, CancellationToken ct)
    {
        // productId da rota prevalece
        var ok = await _mediator.Send(new UpdateSaleItemCommand(id, productId, req.ProductName, req.UnitPrice, req.Quantity), ct);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>Remove um item da venda.</summary>
    /// <param name="id">ID da venda.</param>
    /// <param name="productId">ID do produto (chave do item).</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="204">Item removido com sucesso.</response>
    /// <response code="404">Venda ou item não encontrado.</response>
    [HttpDelete("{id:guid}/items/{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteItem([FromRoute] Guid id, [FromRoute] Guid productId, CancellationToken ct)
    {
        var ok = await _mediator.Send(new DeleteSaleItemCommand(id, productId), ct);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>Cancela uma venda.</summary>
    /// <param name="id">ID da venda.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <response code="204">Venda cancelada com sucesso.</response>
    /// <response code="404">Venda não encontrada.</response>
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel([FromRoute] Guid id, CancellationToken ct)
    {
        var ok = await _mediator.Send(new CancelSaleCommand(id), ct);
        return ok ? NoContent() : NotFound();
    }
}
