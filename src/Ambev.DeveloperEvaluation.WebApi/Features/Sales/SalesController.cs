// Application (CQRS)
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
// WebApi DTOs (Features) — ajuste os namespaces conforme os seus arquivos
 
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Requests;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Responses;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CreateSaleRequest = Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale.CreateSaleRequest;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller for sales operations
/// </summary>
/// 
//[Authorize] Implementar apenas se necessário
[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of SalesController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new sale
    /// </summary>
    /// <param name="request">The sale payload</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale basic info</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateSaleCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<CreateSaleResponse>
        {
            Success = true,
            Message = "Sale created successfully",
            Data = _mapper.Map<CreateSaleResponse>(result)
        });
    }

    /// <summary>
    /// Gets a sale by its identifier
    /// </summary>
    /// <param name="id">The sale id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale details</returns>
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

    /// <summary>Lista/pagina vendas</summary>
    [HttpGet]
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
        return dto; 
    }


    /// <summary>Atualiza cabeçalho</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateSaleRequest req, CancellationToken ct)
    {
        var ok = await _mediator.Send(new UpdateSaleCommand(id, req.Date, req.CustomerId, req.CustomerName, req.BranchId, req.BranchName), ct);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>Adiciona item</summary>
    [HttpPost("{id:guid}/items")]
    public async Task<IActionResult> AddItem([FromRoute] Guid id, [FromBody] AddOrUpdateSaleItemRequest req, CancellationToken ct)
    {
        var ok = await _mediator.Send(new AddSaleItemCommand(id, req.ProductId, req.ProductName, req.UnitPrice, req.Quantity), ct);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>Atualiza item</summary>
    [HttpPut("{id:guid}/items/{productId:guid}")]
    public async Task<IActionResult> UpdateItem([FromRoute] Guid id, [FromRoute] Guid productId, [FromBody] AddOrUpdateSaleItemRequest req, CancellationToken ct)
    {
        // productId da rota prevalece
        var ok = await _mediator.Send(new UpdateSaleItemCommand(id, productId, req.ProductName, req.UnitPrice, req.Quantity), ct);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>Remove item</summary>
    [HttpDelete("{id:guid}/items/{productId:guid}")]
    public async Task<IActionResult> DeleteItem([FromRoute] Guid id, [FromRoute] Guid productId, CancellationToken ct)
    {
        var ok = await _mediator.Send(new DeleteSaleItemCommand(id, productId), ct);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>Cancela venda</summary>
    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel([FromRoute] Guid id, CancellationToken ct)
    {
        var ok = await _mediator.Send(new CancelSaleCommand(id), ct);
        return ok ? NoContent() : NotFound();
    }
}
